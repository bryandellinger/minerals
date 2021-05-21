/* eslint-disable class-methods-use-this */
const template = require('../../views/wells/addUpdateWell/nriInformation.handlebars');
const nriWarningTemplate = require('../../views/wells/addUpdateWell/nriWarning.handlebars');
const nonUnitizedNRITemplate = require('../../views/wells/addUpdateWell/nonUnitizedNRI.handlebars');

export default class NRIInformationMgr {
  constructor(addUpdateWellMgr) {
    this.addUpdateWellMgr = addUpdateWellMgr;
    this.container = 'nriInformationContainer';
    this.wellTractInfoHandlebarData = null;
  }

  render(wellTractInfoHandlebarData) {
    if (wellTractInfoHandlebarData) {
      this.wellTractInfoHandlebarData = { ...wellTractInfoHandlebarData };
    }

    if (this.wellTractInfoHandlebarData.editmode) {
      $(`#${this.container}`).empty().append(nriWarningTemplate());
    } else {
      this.renderNRITable();
    }
  }

  renderNRITable() {
    const {
      contractsUrl, baseUrl, wellTractInfos,
    } = this.wellTractInfoHandlebarData;
    const {
      well, wellboreShares, editmode,
    } = this.addUpdateWellMgr.state;
    let paymentRequirements = [];
    let unitsAndTracts = [];
    let multipleUnitsInd = false;
    let nriInfosByTract = [];
    const contractIds = wellTractInfos.map((x) => x.contractId);
    if (contractIds.length && well.id) {
      $.when(
        $.get(`./api/PaymentRequirementMgrApi/paymentRequirementsbycontract/${contractIds.toString() || '0'}`, (d) => {
          paymentRequirements = d.map(
            (paymentRequirement) => (
              {
                ...paymentRequirement,
              }
            ),
          );
        }),
        $.get(`./api/TractUnitJunctionWellJunctionMgrApi/unitsandtractsbywell/${well.id}`, (d) => {
          unitsAndTracts = d;
          multipleUnitsInd = [...new Set(d.map((x) => x.id))].length > 1;
        }),
        $.get(`./api/TractUnitJunctionWellJunctionMgrApi/nriTractInfobywell/${well.id}`, (d) => {
          nriInfosByTract = d;
        }),
      ).then(() => {
        if (unitsAndTracts.length) {
          const nriInformationsWithAcres = this.wellTractInfoHandlebarData.wellTractInfos.map(
            (wellTractInfo) => (
              {
                royaltyPercentAsString: wellTractInfo.royaltyPercent.toFixed(2),
                royaltyPercent: wellTractInfo.royaltyPercent,
                tract: wellTractInfo.tracts.find((x) => x.selected),
                contract: wellTractInfo.contracts.find((x) => x.selected),
                lessee: wellTractInfo.lessees.find((x) => x.selected),
                paymentRequirement: paymentRequirements.find((p) => p.id
                 === wellTractInfo.contracts.find((x) => x.selected).paymentRequirementId),
                percentOwnership: wellTractInfo.percentOwnership,
                percentOwnershipAsString: wellTractInfo.percentOwnership.toFixed(2),
                units: unitsAndTracts.filter(
                  (x) => x.tractId === wellTractInfo.tracts.find((y) => y.selected).id,
                ),
                get copAcresByTract() {
                  return nriInfosByTract.find((x) => x.tractId === this.tract.id).copAcres;
                },
                get dpuAcresByTract() {
                  return nriInfosByTract.find((x) => x.tractId === this.tract.id).dpuAcres;
                },
              }
            ),
          );

          const nriInformations = nriInformationsWithAcres.map(
            (nriInformationWithAcres) => (
              {
                ...nriInformationWithAcres,
                unitOwnership: well.wellboreLengthInd && multipleUnitsInd
                  ? (this.calculateWellboreOwnership(nriInformationWithAcres) * 100).toFixed(9)
                  : this.calculateBasicOwnership(nriInformationWithAcres).toFixed(9),
                nri: well.wellboreLengthInd && multipleUnitsInd
                  ? this.calculateWellboreNRI(nriInformationWithAcres).toFixed(9)
                  : this.calculateBasicNRI(nriInformationWithAcres).toFixed(9),
              }
            ),
          );
          const totalLength = well.totalBoreholeLengthOverrideInd
            ? well.totalBoreholeLength
            : wellboreShares.map((x) => x.lengthInUnit).reduce((a, b) => a + b, 0);
          const handlebarData = {
            contractsUrl,
            unitsUrl: baseUrl.replace('Wells', 'Units'),
            nriInformations,
            totalNRI: nriInformations.map((x) => parseFloat(x.nri)).reduce((a, b) => a + b, 0).toFixed(9),
            well,
            multipleUnitsInd,
            wellboreShares: wellboreShares.map(
              (wellboreShare) => (
                {
                  ...wellboreShare,
                  percentInUnit: totalLength ? ((wellboreShare.lengthInUnit / totalLength) * 100).toFixed(2) : '0.00',
                }
              ),
            ),
            totalLength,
            editmode,
          };
          const t = template(handlebarData);
          $(`#${this.container}`).empty().append(t);

          $('#wellboreLengthIndToggle').bootstrapToggle({
            on: 'Yes',
            off: 'No',
            onstyle: 'secondary',
            offstyle: 'secondary',
          });
          this.initWellboreLengthIndToggleChange();
          this.initlengthInUnitChange();
          this.initTotalBoreholeLengthOverrideIndChange();
          this.initTotalLengthChange();
        } else {
          this.renderNonUnitizedNRI(paymentRequirements);
        }
      });
    }
  }

  renderNonUnitizedNRI(paymentRequirements) {
    const { wellTractInfos, contractsUrl } = this.wellTractInfoHandlebarData;
    if (wellTractInfos.length && paymentRequirements.length) {
      const nonUnitizedWellTractInfos = wellTractInfos.map(
        (wellTractInfo) => (
          {
            tract: wellTractInfo.tracts.find((x) => x.selected),
            paymentRequirement: paymentRequirements.find((p) => p.id
                 === wellTractInfo.contracts.find((x) => x.selected).paymentRequirementId),
            lessee: wellTractInfo.lessees.find((x) => x.selected),
            percentOwnershipAsString: wellTractInfo.percentOwnership.toFixed(2),
            percentOwnership: wellTractInfo.percentOwnership,
            royaltyPercent: wellTractInfo.royaltyPercent,
            royaltyPercentAsString: wellTractInfo.royaltyPercent.toFixed(2),
            nri: ((wellTractInfo.royaltyPercent / 100) * (wellTractInfo.percentOwnership / 100)).toFixed(9),
          }
        ),
      );
      const handlebarData = {
        contractsUrl,
        nonUnitizedWellTractInfos,
        totalNRI: nonUnitizedWellTractInfos
          .map((x) => parseFloat(x.nri)).reduce((a, b) => a + b, 0).toFixed(9),
      };
      const t = nonUnitizedNRITemplate(handlebarData);
      $(`#${this.container}`).empty().append(t);
    }
  }

  initTotalLengthChange() {
    $('#totalLength').change(() => {
      const { well } = this.addUpdateWellMgr.state;
      well.totalBoreholeLength = parseFloat($('#totalLength').val());
      this.addUpdateWellMgr.setState({ ...this.addUpdateWellMgr.state, well });
      this.render();
    });
  }

  initTotalBoreholeLengthOverrideIndChange() {
    $('#totalBoreholeLengthOverrideInd').click((e) => {
      const { well } = this.addUpdateWellMgr.state;
      well.totalBoreholeLengthOverrideInd = $(e.target).prop('checked');
      well.totalBoreholeLength = well.totalBoreholeLengthOverrideInd ? parseFloat($('#totalLength').val()) : null;
      this.addUpdateWellMgr.setState({ ...this.addUpdateWellMgr.state, well });
      this.render();
    });
  }

  calculateBasicNRI(nriInformationWithAcres) {
    return (nriInformationWithAcres.royaltyPercent / 100)
                   * (nriInformationWithAcres.percentOwnership / 100)
                   * (nriInformationWithAcres.copAcresByTract / nriInformationWithAcres.dpuAcresByTract);
  }

  calculateWellboreNRI(nriInformationWithAcres) {
    return (nriInformationWithAcres.royaltyPercent / 100)
    * (nriInformationWithAcres.percentOwnership / 100)
    * this.calculateWellboreOwnership(nriInformationWithAcres);
  }

  calculateBasicOwnership(nriInformationWithAcres) {
    return (nriInformationWithAcres.copAcresByTract / nriInformationWithAcres.dpuAcresByTract) * 100;
  }

  calculateWellboreOwnership(nriInformationWithAcres) {
    const { well } = this.addUpdateWellMgr.state;
    const { units } = nriInformationWithAcres;
    const { wellboreShares } = this.addUpdateWellMgr.state;
    const totalLength = well.totalBoreholeLengthOverrideInd
      ? well.totalBoreholeLength
      : wellboreShares.map((x) => x.lengthInUnit).reduce((a, b) => a + b, 0);
    const unitsWithWellboreShare = units.map(
      (unit) => (
        {
          ...unit,
          wellboreShare: wellboreShares.find((x) => x.unitId === unit.id),
        }
      ),
    );
    const unitsWithCalculation = unitsWithWellboreShare.map(
      (unit) => (
        {
          ...unit,
          calculation: (unit.copAcresByUnit / unit.dpuAcresByUnit) * (unit.wellboreShare.lengthInUnit / totalLength),
        }
      ),
    );
    return unitsWithCalculation.map((x) => x.calculation).reduce((a, b) => a + b, 0);
  }

  initlengthInUnitChange() {
    $('.lengthInUnit').change((e) => {
      const index = $(e.target).closest('tr').index();
      const { wellboreShares } = this.addUpdateWellMgr.state;
      wellboreShares[index].lengthInUnit = parseFloat($(e.target).val());
      this.addUpdateWellMgr.setState({ ...this.addUpdateWellMgr.state, wellboreShares });
      this.render();
    });
  }


  initWellboreLengthIndToggleChange() {
    $('#wellboreLengthIndToggle').change((e) => {
      const { well } = this.addUpdateWellMgr.state;
      well.wellboreLengthInd = $(e.target).prop('checked');
      this.addUpdateWellMgr.setState({
        ...this.addUpdateWellMgr.state, well,
      });
      this.render();
    });
  }
}
