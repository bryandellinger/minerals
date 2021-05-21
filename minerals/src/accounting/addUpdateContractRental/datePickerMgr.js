/* eslint-disable class-methods-use-this */
export default class datePickerMgr {
  constructor(addUpdateContractRentalMgr) {
    this.addUpdateContractRentalMgr = addUpdateContractRentalMgr;
  }

  init(element, dateFormat) {
    const onSelect = (dateText) => {
      const { contractRental } = this.addUpdateContractRentalMgr.state;
      contractRental[element] = new Date(dateText);
      this.addUpdateContractRentalMgr.setState(
        { ...this.addUpdateContractRentalMgr.state, contractRental },
      );
      this.addUpdateContractRentalMgr.render();
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


    if (this.addUpdateContractRentalMgr.state.contractRental[element]) {
      $(`#${element}`).datepicker('setDate', this.addUpdateContractRentalMgr.state.contractRental[element]);
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
    const { contractRental } = this.addUpdateContractRentalMgr.state;
    contractRental[element] = null;
    this.addUpdateContractRentalMgr.setState(
      { ...this.addUpdateContractRentalMgr.state, contractRental },
    );
    this.addUpdateContractRentalMgr.render();
  }
}
