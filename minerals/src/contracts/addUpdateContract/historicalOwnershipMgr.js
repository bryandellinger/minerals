const template = require('../../views/contracts/addUpdateContract/addWellInfoTableButtons.handlebars');

import AjaxService from '../../services/ajaxService';
import initOwnershipHistoryDataTable from './initOwnershipHistoryDataTable';


export default class HistoricalOwnershipMgr {
  constructor(addUpdateContractMgr) {
    this.addUpdateContractMgr = addUpdateContractMgr;
    this.historicalOwnershipTable = null;
  }

  init() {
    if (this.historicalOwnershipTable) {
      this.historicalOwnershipTable.destroy();
    }   
      this.historicalOwnershipTable = initOwnershipHistoryDataTable(this.getData());
      this.initAutoUpdatedAllowedIndClick();
      this.addButtons();
      this.initDisableAllClick();
      this.initEnableAllClick();
    }

    initDisableAllClick() {
        $('#disableAllBtn').click((event) => {
            event.preventDefault();
            event.stopPropagation();
            $('.spinner').show();
            const { contract, wells } = this.addUpdateContractMgr.state;
            AjaxService.ajaxPut('./api/WellMgrApi/updateByContract', { contractId: contract.id, autoUpdatedAllowedInd: false })
                .then((d) => {
                    $('.spinner').hide();
                    const newWells = wells.map(
                        (item) => (
                            {
                                ...item,
                                autoUpdatedAllowedInd: false
                            }),
                    );
                    this.redrawTable(newWells);
                })
                .catch(() => {
                    $('.spinner').hide();
                });
        });
    }

    initEnableAllClick() {
        $('#enableAllBtn').click((event) => {
            event.preventDefault();
            event.stopPropagation();
            $('.spinner').show();
            const { contract, wells } = this.addUpdateContractMgr.state;
            AjaxService.ajaxPut('./api/WellMgrApi/updateByContract', { contractId: contract.id, autoUpdatedAllowedInd: true })
                .then((d) => {
                    $('.spinner').hide();
                    const newWells = wells.map(
                        (item) => (
                            {
                                ...item,
                                autoUpdatedAllowedInd: true
                            }),
                    );
                    this.redrawTable(newWells);
                })
                .catch(() => {
                    $('.spinner').hide();
                });
        });
    }

   addButtons(){
        const isIE = window.navigator.userAgent.match(/MSIE|Trident/) !== null;
       const t = template({ isIE });
           $('#historicalOwnershipTable_wrapper .dt-buttons').prepend(t)
     }

    redrawTable(wells){
    this.addUpdateContractMgr.setState({ ...this.addUpdateContractMgr.state, wells });
    this.historicalOwnershipTable.clear();
    this.historicalOwnershipTable.rows.add(this.getData());
    this.historicalOwnershipTable.draw(false);
    }

    initAutoUpdatedAllowedIndClick() {
        $('#historicalOwnershipTable tbody').on('click', '.autoUpdatedAllowedInd', (event) => {
            $('.spinner').show();
            const data = this.historicalOwnershipTable.row($(event.target).parents('tr')).data();
            AjaxService.ajaxPut('./api/WellMgrApi', { wellId: data.id })
                .then((d) => {
                    const { wells } = this.addUpdateContractMgr.state;
                    const newWells = wells.map(
                        (item) => (
                            {
                                ...item,
                                autoUpdatedAllowedInd: item.id === d.wellId ? d.autoUpdatedAllowedInd : item.autoUpdatedAllowedInd
                            }),
                    );
                    this.redrawTable(newWells)
                    $('.spinner').hide();
                })
                .catch(() => {
                    $('.spinner').hide();
                });

        });
    }

    getData() {
        const {
            wellTractInfos, tracts, contracts, wells,
        } = this.addUpdateContractMgr.state;
        let data = [];
        if (wellTractInfos && wellTractInfos.length) {
            data = wellTractInfos.map(
                (item) => (
                    {
                        id: item.wellId,
                        link: `<a href="${window.location.href.split('?')[0].replace('Contracts', 'Wells')}?container=addUpdateWellContainer&wellId=${item.wellId}&contractId=${item.contractId}"><i class="fas fa-link"></i> view well</a>`,
                        wellNum: wells.find((x) => x.id === item.wellId) ? wells.find((x) => x.id === item.wellId).wellNum : '',
                        apiNum: wells.find((x) => x.id === item.wellId) ? wells.find((x) => x.id === item.wellId).apiNum : '',
                        tractNum: this.getTractNum(item),
                        contractNum: contracts.find((x) => x.id === item.contractId) ? contracts.find((x) => x.id === item.contractId).contractNum : '',
                        lesseeName: item.lesseeId ? `<a target="_blank" href="${window.location.href.split('?')[0]}?container=manageLesseesContainer&lesseeId=${item.lesseeId}"><i class="fas fa-link"></i>${item.lesseeName}</a>` : '',
                        percentOwnership: item.percentOwnership || '',
                        royaltyPercent: item.royaltyPercent,
                        autoUpdatedAllowedInd: wells.find((x) => x.id === item.wellId) && wells.find((x) => x.id === item.wellId).autoUpdatedAllowedInd ?
                            '<i class="far fa-check-square autoUpdatedAllowedInd"></i>' : '<i class="far fa-square autoUpdatedAllowedInd"></i>',
                    }
                ),
            );
        }
        return data;
    }
    getTractNum(item) {
        let retval = '';
        const {tracts} = this.addUpdateContractMgr.state;
        const tractNum = tracts.find((x) => x.id === item.tractId) ? tracts.find((x) => x.id === item.tractId).tractNum : '';
        const baseUrl = window.location.href.split('?')[0];
        if (tractNum) {
            retval = `<a target="_blank" href="${baseUrl}?container=manageTractsContainer&tractNum=${tractNum}"><i class="fas fa-link"></i>${tractNum}</a>`
        }
        return retval;       
    }
}
