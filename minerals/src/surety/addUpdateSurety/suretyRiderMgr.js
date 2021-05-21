/* eslint-disable class-methods-use-this */
/* eslint-disable no-undef */
const template = require('../../views/surety/addUpdateSurety/suretyRider.handlebars');

export default class suretyRiderMgr {
  constructor(addUpdateSuretyMgr) {
    this.addUpdateSuretyMgr = addUpdateSuretyMgr;
    this.container = 'suretyRiderContainer';
  }

  render() {
    const {
      editmode, riderReasons, suretyRiders,
    } = this.addUpdateSuretyMgr.state;
    const handlebarData = {
      editmode,
      suretyRiders: suretyRiders.map((suretyRider) => (
        {
          ...suretyRider,
          riderReasons: riderReasons.map((riderReason) => (
            {
              ...riderReason,
              selected: riderReason.id === parseInt(suretyRider.riderReasonId, 10),
            }
          )),
        }
      )),
    };  
    const t = template(handlebarData);
    $(`#${this.container}`).empty().append(t);
    this.initDatePicker();
    this.initCreateSuretyRiderLinkClick();
    this.initInputChange();
    this.initAddRow();
    this.initDeleteRow();
    if (editmode && $('#suretyRiderTableTbody').find('tr').length && $('#suretyRiderTableTbody').find('tr').length > 1) {
      $('#suretyRiderTableTbody').sortable({
        axis: 'y',
        update: () => {
          this.reorderTable($('#suretyRiderTableTbody').sortable('toArray').map((x) => parseInt(x.split('_').pop(), 10)));
        },
      });
      $('#suretyRiderTableTbody').css('cursor', 'grab');
    }
  }

  reorderTable(indexArray) {
    const { suretyRiders } = this.addUpdateSuretyMgr.state;
    const reorderedSuretyRiders = [];
    indexArray.forEach((index) => reorderedSuretyRiders.push(suretyRiders[index]));
    this.addUpdateContractMgr.setState(
      { ...this.addUpdateSuretyMgr.state, suretyRiders: reorderedSuretyRiders },
    );
    this.render();
  }

  initDatePicker() {
    const self = this;

    const onSelect = (dateText, inst) => {
      const index = parseInt($(inst.input).attr('data-index'), 10);
      const { suretyRiders } = self.addUpdateSuretyMgr.state;
      suretyRiders[index].effectiveDate = new Date(dateText);
      self.addUpdateSuretyMgr.setState(
        { ...self.addUpdateSuretyMgr.state, suretyRiders },
      );
    };

    $('.suretyEffectiveDate').datepicker({
      dateFormat: 'mm/dd/yy',
      changeMonth: true,
      changeYear: true,
      onSelect,
      yearRange: '-100:+100',
      beforeShow: (elem, dp) => {
        $(dp.dpDiv).removeClass('hide-day-calender');
      },
    });

    $('.suretyEffectiveDate').each(function (index) {
      $(this).datepicker('setDate', self.addUpdateSuretyMgr.state.suretyRiders[index].effectiveDate);
    });

    $('.suretyEffectiveDateBtn').click((e) => {
      const index = parseInt($(e.currentTarget).attr('data-index'), 10);
      $(`.suretyEffectiveDate[data-index='${index}']`).datepicker('show');
    });
  }

  initInputChange() {
    $('.suretyRiderInput').change((e) => {
      const index = parseInt($(e.currentTarget).data('index'), 10);
      const element = $(e.currentTarget).data('element');
      const type = $(e.currentTarget).data('type');
      const { suretyRiders } = this.addUpdateSuretyMgr.state;
      suretyRiders[index][element] = type === 'int' ? parseInt($(e.currentTarget).val(), 10) : $(e.currentTarget).val();
      this.addUpdateSuretyMgr.setState(
        { ...this.addUpdateSuretyMgr.state, suretyRiders },
      );
    });
  }

  initCreateSuretyRiderLinkClick() {
    $('#createSuretyRiderLink').click((e) => {
      e.preventDefault();
      this.insertAt(0);
    });
  }

  initAddRow() {
    $('.addRecord').click((e) => {
      this.insertAt(parseInt($(e.currentTarget).data('index'), 10));
    });
  }

  initDeleteRow() {
    $('.removeRecord').click((e) => {
      const { suretyRiders } = this.addUpdateSuretyMgr.state;
      const index = parseInt($(e.currentTarget).data('index'), 10);
      // eslint-disable-next-line no-unused-vars
      const removed = suretyRiders.splice(index, 1);
      this.addUpdateSuretyMgr.setState(
        {
          ...this.addUpdateSuretyMgr.state,
          suretyRiders,
        },
      );
      this.render();
    });
  }

  insertAt(index) {
    const {
      suretyRiders, riderReasons,
    } = this.addUpdateSuretyMgr.state;
    suretyRiders.splice(index + 1, 0, {
      id: 0,
      suretyRiderNotes: null,
      riderReasonId: riderReasons[0].id,
      effectiveDate: new Date(),
    });
    this.addUpdateSuretyMgr.setState(
      { ...this.addUpdateSuretyMgr.state, suretyRiders },
    );
    this.render();
  }
}
