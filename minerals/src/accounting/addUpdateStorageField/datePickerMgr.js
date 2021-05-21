/* eslint-disable class-methods-use-this */
export default class datePickerMgr {
  constructor(addUpdateStorageFieldMgr) {
    this.addUpdateStorageFieldMgr = addUpdateStorageFieldMgr;
  }

  init(element, dateFormat) {
    const onSelect = (dateText) => {
      const { storageRental } = this.addUpdateStorageFieldMgr.state;
      storageRental[element] = new Date(dateText);
      this.addUpdateStorageFieldMgr.setState(
        { ...this.addUpdateStorageFieldMgr.state, storageRental },
      );
      this.addUpdateStorageFieldMgr.render();
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


    if (this.addUpdateStorageFieldMgr.state.storageRental[element]) {
      $(`#${element}`).datepicker('setDate', this.addUpdateStorageFieldMgr.state.storageRental[element]);
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
    const { storageRental } = this.addUpdateStorageFieldMgr.state;
    storageRental[element] = null;
    this.addUpdateStorageFieldMgr.setState(
      { ...this.addUpdateStorageFieldMgr.state, storageRental },
    );
    this.addUpdateStorageFieldMgr.render();
  }
}
