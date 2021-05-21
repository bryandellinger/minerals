/* eslint-disable class-methods-use-this */
export default class datePickerMgr {
  constructor(addUpdateOtherPaymentMgr) {
    this.addUpdateOtherPaymentMgr = addUpdateOtherPaymentMgr;
  }

  init(element, dateFormat) {
    const onSelect = (dateText) => {
      const { otherPayment } = this.addUpdateOtherPaymentMgr.state;
      otherPayment[element] = new Date(dateText);
      this.addUpdateOtherPaymentMgr.setState(
        { ...this.addUpdateOtherPaymentMgr.state, otherPayment },
      );
      this.addUpdateOtherPaymentMgr.render();
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


    if (this.addUpdateOtherPaymentMgr.state.otherPayment[element]) {
      $(`#${element}`).datepicker('setDate', this.addUpdateOtherPaymentMgr.state.otherPayment[element]);
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
    const { otherPayment } = this.addUpdateOtherPaymentMgr.state;
    otherPayment[element] = null;
    this.addUpdateOtherPaymentMgr.setState(
      { ...this.addUpdateOtherPaymentMgr.state, otherPayment },
    );
    this.addUpdateOtherPaymentMgr.render();
  }
}
