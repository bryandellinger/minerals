/* eslint-disable class-methods-use-this */
export default class datePickerMgr {
    constructor(ManageTractsMgr) {
        this.manageTractsMgr = ManageTractsMgr;
  }

  init(element, dateFormat) {
    const onSelect = (dateText) => {
    const { tract } = this.manageTractsMgr.state;
      tract[element] = new Date(dateText);
        this.manageTractsMgr.setState(
            { ...this.manageTractsMgr.state, tract },
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


      if (this.manageTractsMgr.state.tract[element]) {
          $(`#${element}`).datepicker('setDate', this.manageTractsMgr.state.tract[element]);
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
      const { tract } = this.manageTractsMgr.state;
    tract[element] = null;
      this.manageTractsMgr.setState(
          { ...this.manageTractsMgr.state, tract },
    );
  }
}
