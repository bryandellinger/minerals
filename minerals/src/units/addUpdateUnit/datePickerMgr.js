/* eslint-disable class-methods-use-this */
export default class datePickerMgr {
  constructor(addUpdateUnitMgr) {
    this.addUpdateUnitMgr = addUpdateUnitMgr;
  }

  init(element, dateFormat) {
    const onSelect = (dateText) => {
      const { unit } = this.addUpdateUnitMgr.state;
      unit[element] = new Date(dateText);
      this.addUpdateUnitMgr.setState(
        { ...this.addUpdateUnitMgr.state, unit },
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


    if (this.addUpdateUnitMgr.state.unit[element]) {
      $(`#${element}`).datepicker('setDate', this.addUpdateUnitMgr.state.unit[element]);
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
    const { unit } = this.addUpdateUnitMgr.state;
    unit[element] = null;
    this.addUpdateUnitMgr.setState(
      { ...this.addUpdateUnitMgr.state, unit },
    );
  }
}
