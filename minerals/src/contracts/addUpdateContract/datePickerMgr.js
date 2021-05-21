/* eslint-disable class-methods-use-this */
export default class datePickerMgr {
  constructor(AddUpdateContractMgr) {
    this.addUpdateContractMgr = AddUpdateContractMgr;
  }

  init(element, dateFormat, changeMonth, changeYear, landleasein) {
    const onSelectPaymentRequirement = (dateText) => {
      const { paymentRequirement } = this.addUpdateContractMgr.state;
      paymentRequirement[element] = new Date(dateText);
      this.addUpdateContractMgr.setState(
        { ...this.addUpdateContractMgr.state, paymentRequirement },
      );
    };

    const onSelectLandLease = (dateText) => {
      const { agreement } = this.addUpdateContractMgr.state;
      agreement[element] = new Date(dateText);
      this.addUpdateContractMgr.setState(
        { ...this.addUpdateContractMgr.state, agreement },
      );
      if (element === 'terminationDate') {
        this.addUpdateContractMgr.render();
      }
    };

    $(`#${element}`).datepicker({
      dateFormat,
      changeMonth,
      changeYear,
      onSelect: landleasein ? onSelectLandLease : onSelectPaymentRequirement,
      yearRange: '-100:+100',
    });

    this.initShowButtonClick(element);
    this.initClearButtonClick(element, landleasein);
    this.initBackSpaceClick(element, landleasein);

    if (landleasein) {
      if (this.addUpdateContractMgr.state.agreement[element]) {
        $(`#${element}`).datepicker('setDate', this.addUpdateContractMgr.state.agreement[element]);
      }
    }

    if (!landleasein) {
      if (this.addUpdateContractMgr.state.paymentRequirement[element]) {
        $(`#${element}`).datepicker('setDate', this.addUpdateContractMgr.state.paymentRequirement[element]);
      }
    }
  }

  initShowButtonClick(element) {
    $(`#${element}Btn`).click(() => { $(`#${element}`).datepicker('show'); });
  }

  initClearButtonClick(element, landleasein) {
    $(`#${element}ClearBtn`).click(() => {
      this.clear(element, landleasein);
    });
  }

  initBackSpaceClick(element, landleasein) {
    $(`#${element}`).keydown((e) => {
      if (e.keyCode === 8) {
        e.preventDefault();
        this.clear(element, landleasein);
      }
    });
  }

  clear(element, landleasein) {
    $(`#${element}`).datepicker('setDate', null);
    const { agreement, paymentRequirement } = this.addUpdateContractMgr.state;
    if (landleasein) {
      agreement[element] = null;
      if (element === 'terminationDate') {
        agreement.terminationReasonId = null;
      }
      this.addUpdateContractMgr.setState(
        { ...this.addUpdateContractMgr.state, agreement },
      );
      this.addUpdateContractMgr.render();
    } else {
      paymentRequirement[element] = null;
      this.addUpdateContractMgr.setState(
        { ...this.addUpdateContractMgr.state, paymentRequirement },
      );
    }
  }
}
