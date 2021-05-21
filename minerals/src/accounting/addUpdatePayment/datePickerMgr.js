/* eslint-disable class-methods-use-this */
export default class datePickerMgr {
  constructor(addUpdatePaymentMgr) {
    this.addUpdatePaymentMgr = addUpdatePaymentMgr;
  }

  init(element, dateFormat) {
    const onSelect = (dateText) => {
      const { payment } = this.addUpdatePaymentMgr.state;
      payment[element] = new Date(dateText);
      this.addUpdatePaymentMgr.setState(
        { ...this.addUpdatePaymentMgr.state, payment },
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


    if (this.addUpdatePaymentMgr.state.payment[element]) {
      $(`#${element}`).datepicker('setDate', this.addUpdatePaymentMgr.state.payment[element]);
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
    const { payment } = this.addUpdatePaymentMgr.state;
    payment[element] = null;
    this.addUpdatePaymentMgr.setState(
      { ...this.addUpdatePaymentMgr.state, payment },
    );
  }
}
