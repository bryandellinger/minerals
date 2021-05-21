import initSuretyDataTable from './initSuretyDataTable';
import initCardHeader from '../initCardHeader';

const template = require('../../views/wells/addUpdateWell/index.handlebars');

export default class RenderMgr {
  constructor(AddUpdateWellMgr) {
    this.addUpdateWellMgr = AddUpdateWellMgr;
  }

  render() {
    const {
      editmode, well, wellStatuses, lessees, wellTypes, townships, producingFormations, deepestFormations,
      contractId, unitId, units, paymentId, suretyId,
    } = this.addUpdateWellMgr.state;
    let baseUrl = window.location.pathname.replace('Home', '');
    baseUrl = baseUrl.replace('wells', '');
    baseUrl = baseUrl.replace('//', '/');
    if (!baseUrl.endsWith('/')) {
      baseUrl = `${baseUrl}`;
    }
    const handlebarData = {
      showUnitsInd: units && units.length,
      headerInfo: initCardHeader(),
      id: this.addUpdateWellMgr.id,
      baseUrl,
      contractsUrl: baseUrl.replace('Wells', 'Contracts'),
      unitsUrl: baseUrl.replace('Wells', 'Units'),
      well,
      editmode,
      contractId,
      unitId,
      paymentId,
      suretyId,
      wellStatuses: wellStatuses.map(
        (wellStatus) => (
          {
            ...wellStatus,
            selected: wellStatus.id === well.wellStatusId,
          }
        ),
      ),
      lessees: lessees.map(
        (lessee) => (
          {
            ...lessee,
            selected: lessee.id === well.lesseeId,
          }
        ),
      ),
      showOperatorLinkInd: lessees.find((x) => x.id === well.lesseeId),
      wellTypes: wellTypes.map(
        (wellType) => (
          {
            ...wellType,
            selected: wellType.id === well.wellTypeId,
          }
        ),
      ),
      townships: townships.map(
        (township) => (
          {
            ...township,
            selected: township.id === well.townshipId,
          }
        ),
      ),
      producingFormations: producingFormations.map(
        (producingFormation) => (
          {
            ...producingFormation,
            selected: producingFormation.id === well.producingFormationId,
          }
        ),
      ),
      deepestFormations: deepestFormations.map(
        (deepestFormation) => (
          {
            ...deepestFormation,
            selected: deepestFormation.id === well.deepestFormationId,
          }
        ),
      ),
    };
    const t = template(handlebarData);
      this.addUpdateWellMgr.container.empty().append(t);
      if (this.addUpdateWellMgr.suretyTable) {
          this.addUpdateWellMgr.suretyTable.destroy();
          this.addUpdateWellMgr.suretyTable = null;
      }
      this.addUpdateWellMgr.suretyTable = initSuretyDataTable(this.addUpdateWellMgr.state.sureties.map(
          (surety) => (
              {
                  ...surety,
                  issueDate: surety.issueDate ? new Date(surety.issueDate) : null,
                  releasedDate: surety.releasedDate ? new Date(surety.releasedDate) : null,
                  wellId: well.id || 0
              }
          ),
      ));
    $('#wellStatusId').select2({ placeholder: 'Select a well status', width: '100%' });
    $('#lesseeId').select2({ placeholder: 'Select an operator', width: '100%' });
    $('#wellTypeId').select2({ placeholder: 'Select a well type', width: '100%' });
    $('#producingFormationId').select2({ placeholder: 'Select a formation', width: '100%' });
    $('#deepestFormationId').select2({ placeholder: 'Select a formation', width: '100%' });
    $('#townshipId').select2({ placeholder: 'Select a municipality', width: '100%', minimumInputLength: 2 });
    this.addUpdateWellMgr.datePickerMgr.init('spudDate', 'mm/dd/yy');
    this.renderUnits(handlebarData.unitsUrl);
    this.addUpdateWellMgr.datePickerMgr.init('initialProductionDate', 'mm/dd/yy');
    this.addUpdateWellMgr.datePickerMgr.init('bofAppDate', 'mm/dd/yy');
    this.addUpdateWellMgr.datePickerMgr.init('plugDate', 'mm/dd/yy');
    this.addUpdateWellMgr.datePickerMgr.init('shutInDate', 'mm/dd/yy');
    this.addUpdateWellMgr.datePickerMgr.init('completionDate', 'mm/dd/yy');
    $('#elevationTypeToggle').bootstrapToggle({
      on: 'Below Ground',
      off: 'Above Sea',
      onstyle: 'secondary',
      offstyle: 'secondary',
      size: 'xs',
      width: '125',
    });
    $('#privatePropertyIndToggle').bootstrapToggle({
      on: 'Yes',
      off: 'No',
      onstyle: 'secondary',
      offstyle: 'secondary',
    });
    $('#autoUpdatedAllowedInd').bootstrapToggle({
      on: 'Automatic Update Enabled',
      off: 'Automatic Update Disabled',
      onstyle: 'secondary',
      offstyle: 'secondary',
      size: 'xs',
      width: '400',
    });
    this.addUpdateWellMgr.initElevationTypeToggleChange();
    this.addUpdateWellMgr.initprivatePropertyIndToggleChange();
    this.addUpdateWellMgr.initautoUpdatedAllowedIndToggleChange();
    this.addUpdateWellMgr.wellTractTableMgr.render();
    this.addUpdateWellMgr.alternateIdMgr.render();
    if (editmode) {
      this.addUpdateWellMgr.apiNumMgr.getCountyCodes();
    }
    this.addUpdateWellMgr.validateAndSubmitMgr.initFormSubmit();
    this.addUpdateWellMgr.historicalOwnershipMgr.init();
    this.addUpdateWellMgr.testLogMgr.render();
  }

  renderUnits(unitsUrl) {
    const {
      units, well,
    } = this.addUpdateWellMgr.state;

    const data = units.map(
      (unit) => (
        {
          id: unit.id,
          text: `<a href="${unitsUrl}?container=addUpdateUnitContainer&unitId=${unit.id}&wellId=${well.id}"><i class="fas fa-link"></i></a> ${unit.unitName}`,
          title: unit.unitName,
          selected: true,
        }
      ),
    );


    $('#units').select2({
      data,
      escapeMarkup(markup) {
        return markup;
      },
      templateSelection(data) {
        return data.text;
      },
    });
  }
}
