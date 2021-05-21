/* eslint-disable class-methods-use-this */
export default class datePickerMgr {
  constructor(addUpdateCheckMgr) {
    this.addUpdateCheckMgr = addUpdateCheckMgr;
  }

  init(element, dateFormat) {
    const onSelect = (dateText) => {
      const { check } = this.addUpdateCheckMgr.state;
      check[element] = new Date(dateText);
      this.addUpdateCheckMgr.setState(
        { ...this.addUpdateCheckMgr.state, check },
      );
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


    if (this.addUpdateCheckMgr.state.check[element]) {
      $(`#${element}`).datepicker('setDate', this.addUpdateCheckMgr.state.check[element]);
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
    const { check } = this.addUpdateCheckMgr.state;
    check[element] = null;
    this.addUpdateCheckMgr.setState(
      { ...this.addUpdateCheckMgr.state, check },
    );
  }
}
