/* eslint-disable class-methods-use-this */
export default class datePickerMgr {
  constructor(AddUpdateWellMgr) {
    this.addUpdateWellMgr = AddUpdateWellMgr;
  }

  init(element, dateFormat) {
    const onSelect = (dateText) => {
      const { well } = this.addUpdateWellMgr.state;
      well[element] = new Date(dateText);
      this.addUpdateWellMgr.setState(
        { ...this.addUpdateWellMgr.state, well },
      );
    };

    $(`#${element}`).datepicker({
      dateFormat,
      changeMonth: true,
      changeYear: true,
      onSelect,
      yearRange: '-100:+100',
    });

    this.initShowButtonClick(element);
    this.initClearButtonClick(element);
    this.initBackSpaceClick(element);


    if (this.addUpdateWellMgr.state.well[element]) {
      $(`#${element}`).datepicker('setDate', this.addUpdateWellMgr.state.well[element]);
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
    const { well } = this.addUpdateWellMgr.state;
    well[element] = null;
    this.addUpdateWellMgr.setState(
      { ...this.addUpdateContractMgr.state, well },
    );
  }
}
