import AjaxService from '../../services/ajaxService';

const template = require('../../views/wells/addUpdateWell/WellTractTable.handlebars');

export default class WellTractTableMgr {
  constructor(addUpdateWellMgr) {
    this.addUpdateWellMgr = addUpdateWellMgr;
    this.container = 'wellTractTableContainer';
  }

  render() {
    const {
      editmode, wellTractInfos, tracts, pads, contracts, lessees,
      contractEventDetailReasonsForChange, contractEventDetailReasonsForChangeId,
      wellTractInfosExistInd,
    } = this.addUpdateWellMgr.state;
    let baseUrl = window.location.pathname.replace('Home', '');
    baseUrl = baseUrl.replace('wells', '');
    baseUrl = baseUrl.replace('//', '/');
    if (!baseUrl.endsWith('/')) {
      baseUrl = `${baseUrl}`;
    }
    const handlebarData = {
      baseUrl,
      contractsUrl: baseUrl.replace('Wells', 'Contracts'),
      showReasonForChangeInd: wellTractInfosExistInd && editmode,
      editmode: editmode && (contractEventDetailReasonsForChangeId || !wellTractInfosExistInd),
      contracts,
      contractEventDetailReasonsForChange: contractEventDetailReasonsForChange.map(
        (reason) => (
          {
            ...reason,
            selected: reason.id === contractEventDetailReasonsForChangeId,
          }
        ),
      ),
      wellTractInfos: wellTractInfos.map(
        (wellTractInfo) => (
          {
            ...wellTractInfo,
            tracts: tracts.map(
              (tract) => (
                {
                  ...tract,
                  name: tract.administrative ? `${tract.tractNum}(admin)` : tract.tractNum,
                  selected: tract.id === wellTractInfo.tractId,
                }
              ),
            ),
            pad: pads.find(
              (x) => x.id === wellTractInfo.padId && x.tractId === wellTractInfo.tractId,
            ),
            contracts: contracts.filter((x) => x.tractId === wellTractInfo.tractId).map(
              (contract) => (
                {
                  ...contract,
                  selected: contract.id === wellTractInfo.contractId,
                }
              ),
            ),
            lessees: lessees.map(
              (lessee) => (
                {
                  ...lessee,
                  selected: lessee.id === wellTractInfo.lesseeId,
                }
              ),
            ),
          }
        ),
      ),
    };
    const t = template(handlebarData);
    $(`#${this.container}`).empty().append(t);
    $('#wellTractInfoTbody .tractId').select2({ placeholder: 'Select a tract', width: '100%' });
    $('#wellTractInfoTbody .lesseeId').select2({ placeholder: 'Select an owner', width: '100%' });
    $('.wellOwnerInfoFromContract').select2({ placeholder: 'Automatically create owners by picking a contract', width: '100%' });
    $('#wellTractInfoTbody .contractId').select2({ placeholder: 'Select a tract', width: '100%', minimumResultsForSearch: 10 });
    $('#reasonForChange').select2({ placeholder: 'I am not changing the current ownership information' });
    this.initPadAutoComplete();
    this.initCreateWellTractInfoLinkClick();
    this.initTractSelectChange();
    this.initContractSelectChange();
    this.initLesseeSelectChange();
    this.initReasonForChangeSelectChange();
    this.initWellOwnerInfoFromContractSelectChange();
    this.initPadChange();
    this.initPercentOwnershipChange();
    this.initRoyaltyPercentChange();
    this.initAddWellTractInfo();
    this.initDeleteWellTractInfo();
    if (handlebarData.editmode && $('#wellTractInfoTbody').find('tr').length && $('#wellTractInfoTbody').find('tr').length > 1) {
      $('#wellTractInfoTbody').sortable({
        axis: 'y',
        update: () => {
          this.reorderWellTractInfoTable($('#wellTractInfoTbody').sortable('toArray').map((x) => parseInt(x.split('_').pop(), 10)));
        },
      });
      $('#wellTractInfoTbody').css('cursor', 'grab');
    }
    this.addUpdateWellMgr.nriInformationMgr.render(handlebarData);
  }

  initReasonForChangeSelectChange() {
    $('#reasonForChange').change((e) => {
      const data = $(e.target).select2('data');
      this.addUpdateWellMgr.setState(
        {
          ...this.addUpdateWellMgr.state,
          contractEventDetailReasonsForChangeId: parseInt(data[0].id, 10),
        },
      );
      this.render();
    });
  }

  reorderWellTractInfoTable(indexArray) {
    const { wellTractInfos } = this.addUpdateWellMgr.state;
    const reorderedWellTractInfos = [];
    indexArray.forEach((index) => reorderedWellTractInfos.push(wellTractInfos[index]));
    this.addUpdateWellMgr.setState(
      { ...this.addUpdateWellMgr.state, wellTractInfos: reorderedWellTractInfos },
    );
    this.render();
  }

  initWellOwnerInfoFromContractSelectChange() {
    $('.wellOwnerInfoFromContract').change((e) => {
      const { wellTractInfos, contracts, pads } = this.addUpdateWellMgr.state;
      const data = $(e.target).select2('data');
      $('.spinner').show();
      AjaxService.ajaxGet(`./api/ContractEventDetailMgrApi/currenteventsbycontract/${data[0].id}`)
        .then((eventDetails) => {
          if (eventDetails && eventDetails.length) {
            const contract = contracts.find((x) => x.id === parseInt(data[0].id, 10));
            const filteredPads = pads.filter((x) => x.tractId === contract.tractId);
            eventDetails.forEach((item) => {
              wellTractInfos.push(
                {
                  contractId: contract.id,
                  wellId: 0,
                  padName: filteredPads && filteredPads.length === 1 ? filteredPads[0].padName : null,
                  padId: filteredPads && filteredPads.length === 1 ? filteredPads[0].id : 0,
                  tractId: contract.tractId,
                  lesseeId: item.lesseeId,
                  percentOwnership: item.shareOfLeasePercentage,
                  royaltyPercent: item.royaltyPercent,
                },
              );
            });
            this.addUpdateWellMgr.setState(
              {
                ...this.addUpdateWellMgr.state, wellTractInfos,
              },
            );
            this.render();
          } else {
            window.Toastr.warning('This contract has no lessees');
          }
          $('.spinner').hide();
        });
    });
  }

  initAddWellTractInfo() {
    $('#wellTractInfoTbody .addWellTractInfo').click((e) => {
      this.insertAt($(e.target).closest('tr').index());
    });
  }

  initDeleteWellTractInfo() {
    $('#wellTractInfoTbody .removeWell').click((e) => {
      const { wellTractInfos } = this.addUpdateWellMgr.state;
      const index = $(e.target).closest('tr').index();
      // eslint-disable-next-line no-unused-vars
      const removed = wellTractInfos.splice(index, 1);
      this.addUpdateWellMgr.setState(
        {
          ...this.addUpdateWellMgr.state,
          wellTractInfos,
        },
      );
      this.render();
    });
  }

  initPercentOwnershipChange() {
    $('#wellTractInfoTbody .percentOwnership').change((e) => {
      const { wellTractInfos } = this.addUpdateWellMgr.state;
      const index = $(e.target).closest('tr').index();
      wellTractInfos[index].percentOwnership = parseFloat($(e.target).val());
      this.addUpdateWellMgr.setState(
        {
          ...this.addUpdateWellMgr.state, wellTractInfos,
        },
      );
    });
  }

  initRoyaltyPercentChange() {
    $('#wellTractInfoTbody .royaltyPercent').change((e) => {
      const { wellTractInfos } = this.addUpdateWellMgr.state;
      const index = $(e.target).closest('tr').index();
      wellTractInfos[index].royaltyPercent = parseFloat($(e.target).val());
      this.addUpdateWellMgr.setState(
        {
          ...this.addUpdateWellMgr.state, wellTractInfos,
        },
      );
    });
  }


  initContractSelectChange() {
    $('#wellTractInfoTbody .contractId').change((e) => {
      const { wellTractInfos } = this.addUpdateWellMgr.state;
      const data = $(e.target).select2('data');
      const index = $(e.target).closest('tr').index();
      wellTractInfos[index].contractId = parseInt(data[0].id, 10);
      this.addUpdateWellMgr.setState(
        {
          ...this.addUpdateWellMgr.state, wellTractInfos,
        },
      );
    });
  }

  initLesseeSelectChange() {
    $('#wellTractInfoTbody .lesseeId').change((e) => {
      const { wellTractInfos } = this.addUpdateWellMgr.state;
      const data = $(e.target).select2('data');
      const index = $(e.target).closest('tr').index();
      wellTractInfos[index].lesseeId = parseInt(data[0].id, 10);
      this.addUpdateWellMgr.setState(
        {
          ...this.addUpdateWellMgr.state, wellTractInfos,
        },
      );
    });
  }


  initTractSelectChange() {
    $('#wellTractInfoTbody .tractId').change((e) => {
      const { wellTractInfos, pads, contracts } = this.addUpdateWellMgr.state;
      const data = $(e.target).select2('data');
      const index = $(e.target).closest('tr').index();
      wellTractInfos[index].tractId = parseInt(data[0].id, 10);
      const filteredPads = pads.filter((x) => x.tractId === wellTractInfos[index].tractId);
      const filteredContracts = contracts.filter(
        (x) => x.tractId === wellTractInfos[index].tractId,
      );
      wellTractInfos[index].padId = filteredPads.length === 1 ? filteredPads[0].id : 0;
      wellTractInfos[index].padName = filteredPads.length === 1 ? filteredPads[0].padName : null;
      wellTractInfos[index].contractId = filteredContracts.length === 1
        ? filteredContracts[0].id : 0;
      this.addUpdateWellMgr.setState(
        {
          ...this.addUpdateWellMgr.state, wellTractInfos,
        },
      );
      this.render();
    });
  }

  initPadChange() {
    $('#wellTractInfoTbody .pad').change((e) => {
      const { wellTractInfos, pads } = this.addUpdateWellMgr.state;
      const index = $(e.target).closest('tr').index();
      $('.padNameError').eq(index).hide();
      $('.pad').eq(index).removeClass('is-invalid');
      const pad = pads.filter(
        (x) => x.tractId === wellTractInfos[index].tractId,
      ).find((x) => x.padName === $(e.target).val());
      if (pad) {
        wellTractInfos[index].padId = pad.id;
        wellTractInfos[index].padName = $(e.target).val();
      } else {
        wellTractInfos[index].padId = 0;
        wellTractInfos[index].padName = $(e.target).val();
      }
      this.addUpdateWellMgr.setState(
        {
          ...this.addUpdateWellMgr.state, wellTractInfos,
        },
      );
    });
  }

  initCreateWellTractInfoLinkClick() {
    $('#createWellTractInfoLink').click((e) => {
      e.preventDefault();
      this.insertAt(0);
    });
  }

  initPadAutoComplete() {
    const { pads, wellTractInfos } = this.addUpdateWellMgr.state;
    $('#wellTractInfoTbody .pad').toArray().forEach((element, i) => {
      const source = pads.filter(
        (x) => x.tractId === wellTractInfos[i].tractId,
      ).map((x) => x.padName);
      $(element)
        .autocomplete(
          {
            source,
            minLength: 0,
            select: (event, ui) => {
              wellTractInfos[i].padName = ui.item.label;
              wellTractInfos[i].padId = pads.find(
                (x) => x.tractId === wellTractInfos[i].tractId && x.padName === ui.item.label,
              ).id || 0;
              $('.padNameError').eq(i).hide();
              $('.pad').eq(i).removeClass('is-invalid');
              this.addUpdateWellMgr.setState(
                {
                  ...this.addUpdateWellMgr.state, wellTractInfos,
                },
              );
            },
          },
        )
      // trigger the search on focus
      // eslint-disable-next-line func-names
        .focus(function () {
          $(this).autocomplete('search', $(this).val());
        });
    });
  }

  insertAt(index) {
    const {
      wellTractInfos,
      well,
    } = this.addUpdateWellMgr.state;
    wellTractInfos.splice(index + 1, 0, {
      id: 0,
      wellId: well.id,
      tractId: 0,
      padId: 0,
      contractId: 0,
      royaltyPercent: 0,
    });
    this.addUpdateWellMgr.setState(
      { ...this.addUpdateWellMgr.state, wellTractInfos },
    );
    this.render();
  }
}
