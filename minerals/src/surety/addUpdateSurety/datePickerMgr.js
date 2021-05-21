/* eslint-disable class-methods-use-this */
export default class datePickerMgr {
  constructor(addUpdateSuretyMgr) {
    this.addUpdateSuretyMgr = addUpdateSuretyMgr;
  }

  init(element, dateFormat) {
    const onSelect = (dateText) => {
      const { surety } = this.addUpdateSuretyMgr.state;
      surety[element] = new Date(dateText);
      this.addUpdateSuretyMgr.setState(
        { ...this.addUpdateSuretyMgr.state, surety },
      );
      this.addUpdateSuretyMgr.render();
    };

    $(`#${element}`).datepicker({
      dateFormat,
      changeMonth: true,
      changeYear: true,
      onSelect,
      yearRange: '-100:+100',
      beforeShow: (elem, dp) => {
        $(dp.dpDiv).removeClass('hide-day-calender');
      },
    });

    this.initShowButtonClick(element);
    this.initClearButtonClick(element);
    this.initBackSpaceClick(element);


    if (this.addUpdateSuretyMgr.state.surety[element]) {
      $(`#${element}`).datepicker('setDate', this.addUpdateSuretyMgr.state.surety[element]);
    }
  }

  initShowButtonClick(element) {
    $(`#${element}Btn`).click(() => { $(`#${element}`).datepicker('show'); });
  }

  initClearButtonClick(element) {
    $(`#${element}ClearBtn`).click(() => {
      this.clear(element);
    });
  }

  initBackSpaceClick(element) {
    $(`#${element}`).keydown((e) => {
      if (e.keyCode === 8) {
        e.preventDefault();
        this.clear(element);
      }
    });
  }

  clear(element) {
    $(`#${element}`).datepicker('setDate', null);
    const { surety } = this.addUpdateSuretyMgr.state;
    surety[element] = null;
    this.addUpdateSuretyMgr.setState(
      { ...this.addUpdateSuretyMgr.state, surety },
    );
    this.addUpdateSuretyMgr.render();
  }
}
