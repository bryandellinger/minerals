/* eslint-disable class-methods-use-this */
/* eslint-disable no-undef */
import numberWithCommas from '../../services/numberWithCommas';

const template = require('../../views/surety/addUpdateSurety/SuretyWell.handlebars');

export default class suretyWellMgr {
  constructor(addUpdateSuretyMgr) {
    this.addUpdateSuretyMgr = addUpdateSuretyMgr;
    this.container = 'suretyWellContainer';
  }

  render() {
    const {
      editmode, suretyWells, wells,
    } = this.addUpdateSuretyMgr.state;
    const handlebarData = {
      editmode,
      suretyWells: suretyWells.map((suretyWell) => (
        {
          ...suretyWell,
          wells: wells.map((well) => (
            {
              ...well,
              selected: well.id === parseInt(suretyWell.wellId, 10),
            }
          )),
        }
      )),
      showFooterInd: suretyWells.length > 1,
      totalAmountForWells: numberWithCommas(suretyWells.map((x) => x.suretyWellValue || 0).reduce((a, b) => a + b, 0).toFixed(2)),
    };
    const t = template(handlebarData);
    $(`#${this.container}`).empty().append(t);
    this.initCreateSuretyWellLinkClick();
    $('.suretyWells').select2({ placeholder: 'Select a well', width: '100%' });
    this.initInputChange();
    this.initAddRow();
    this.initDeleteRow();
    if (editmode && $('#suretyWellTableTbody').find('tr').length && $('#suretyWellTableTbody').find('tr').length > 1) {
      $('#suretyWellTableTbody').sortable({
        axis: 'y',
        update: () => {
          this.reorderTable($('#suretyWellTableTbody').sortable('toArray').map((x) => parseInt(x.split('_').pop(), 10)));
        },
      });
      $('#suretyWellTableTbody').css('cursor', 'grab');
    }
  }

  reorderTable(indexArray) {
    const { suretyWells } = this.addUpdateSuretyMgr.state;
    const reorderedSuretyWells = [];
    indexArray.forEach((index) => reorderedSuretyWells.push(suretyWells[index]));
    this.addUpdateContractMgr.setState(
      { ...this.addUpdateSuretyMgr.state, suretyWells: reorderedSuretyWells },
    );
    this.render();
  }

  initInputChange() {
    $('.suretyWellInput').change((e) => {
      const index = parseInt($(e.currentTarget).data('index'), 10);
      const element = $(e.currentTarget).data('element');
      const type = $(e.currentTarget).data('type');
      const { suretyWells } = this.addUpdateSuretyMgr.state;
      suretyWells[index][element] = type === 'int' ? parseInt($(e.currentTarget).val(), 10) : parseFloat($(e.currentTarget).val());
      this.addUpdateSuretyMgr.setState(
        { ...this.addUpdateSuretyMgr.state, suretyWells },
      );
      if (element === 'suretyWellValue' && suretyWells.length > 1) {
        this.render();
      }
    });
  }

  initCreateSuretyWellLinkClick() {
    $('#createSuretyWellLink').click((e) => {
      e.preventDefault();
      this.insertAt(0);
    });
  }

  initAddRow() {
    $('.addWellRecord').click((e) => {
      this.insertAt(parseInt($(e.currentTarget).data('index'), 10));
    });
  }

  initDeleteRow() {
    $('.removeWellRecord').click((e) => {
      const { suretyWells } = this.addUpdateSuretyMgr.state;
      const index = parseInt($(e.currentTarget).data('index'), 10);
      // eslint-disable-next-line no-unused-vars
      const removed = suretyWells.splice(index, 1);
      this.addUpdateSuretyMgr.setState(
        {
          ...this.addUpdateSuretyMgr.state,
          suretyWells,
        },
      );
      this.render();
    });
  }

  insertAt(index) {
    const {
      suretyWells,
    } = this.addUpdateSuretyMgr.state;
    suretyWells.splice(index + 1, 0, {
      id: 0,
      wellId: null,
      suretyWellValue: null,
    });
    this.addUpdateSuretyMgr.setState(
      { ...this.addUpdateSuretyMgr.state, suretyWells },
    );
    this.render();
  }
}
