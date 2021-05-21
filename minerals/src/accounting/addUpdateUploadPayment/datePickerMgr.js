/* eslint-disable class-methods-use-this */
export default class datePickerMgr {
  constructor(addUpdateUploadMgr) {
    this.addUpdateUploadMgr = addUpdateUploadMgr;
  }

  init(element, dateFormat) {
    const onSelect = (dateText) => {
      const { uploadPayment } = this.addUpdateUploadMgr.state;
      uploadPayment[element] = new Date(dateText);
      this.addUpdateUploadMgr.setState(
        { ...this.addUpdateUploadMgr.state, uploadPayment },
      );
      this.addUpdateUploadMgr.render();
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


    if (this.addUpdateUploadMgr.state.uploadPayment[element]) {
      $(`#${element}`).datepicker('setDate', this.addUpdateUploadMgr.state.uploadPayment[element]);
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
    const { uploadPayment } = this.addUpdateUploadMgr.state;
    uploadPayment[element] = null;
    this.addUpdateUploadMgr.setState(
      { ...this.addUpdateUploadMgr.state, uploadPayment },
    );
    this.addUpdateUploadMgr.render();
  }
}
