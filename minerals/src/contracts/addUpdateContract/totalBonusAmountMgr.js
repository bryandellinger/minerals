import roundDecimals from '../../services/roundDecimals';
import formatReadOnlyNumberInputs from '../../services/formatReadOnlyNumberInputs';

const template = require('../../views/contracts/addUpdateContract/totalBonusAmount.handlebars');
const additionalBonusesTemplate = require('../../views/contracts/addUpdateContract/additionalBonuses.handlebars');

export default class TotalBonusAmountMgr {
  constructor(AddUpdateContractMgr) {
    this.addUpdateContractMgr = AddUpdateContractMgr;
    this.container = 'totalBonusAmountContainer';
  }

  init() {
    this.initTotalBonusAmountCheckbox();
    this.initTotalBonusAmountChange();
  }

  initAddAdditionalBonusClick() {
    $('.addAdditionalBonus').click((e) => {
      const index = $(e.target).attr('data-index');
      e.preventDefault();
      e.stopPropagation();
      const { additionalBonuses } = this.addUpdateContractMgr.state;
      const newBonus = { id: 0, additionalBonusAmount: 0, paymentRequirementId: 0 };
      if (index) {
        additionalBonuses.splice(parseInt(index, 10) + 1, 0, newBonus);
      } else {
        additionalBonuses.push(newBonus);
      }
      this.addUpdateContractMgr.setState(
        { ...this.addUpdateContractMgr.state, additionalBonuses },
      );
      this.render();
    });
  }

  initDeleteAdditionalBonusClick() {
    $('.deleteAdditionalBonus').click((e) => {
      const index = $(e.target).attr('data-index');
      e.preventDefault();
      e.stopPropagation();
      const { additionalBonuses } = this.addUpdateContractMgr.state;
      additionalBonuses.splice(index, 1);
      this.addUpdateContractMgr.setState(
        { ...this.addUpdateContractMgr.state, additionalBonuses },
      );
      this.render();
    });
  }


  initTotalBonusAmountChange() {
    $(`#${this.container}`).on('change', '#totalBonusAmount', () => {
      const { paymentRequirement } = this.addUpdateContractMgr.state;
      paymentRequirement.totalBonusAmount = parseFloat($('#totalBonusAmount').val() || 0);
      this.addUpdateContractMgr.setState(
        { ...this.addUpdateContractMgr.state, paymentRequirement },
      );
      this.addUpdateContractMgr.doesElementHaveError('totalBonusAmount', false, false);
    });
  }

  intAdditionalBonusChange() {
    $('.additionalBonus').change((e) => {
      const index = $(e.target).attr('data-index');
      const { additionalBonuses } = this.addUpdateContractMgr.state;
      additionalBonuses[index].additionalBonusAmount = parseFloat($(e.target).val());
      this.addUpdateContractMgr.setState(
        { ...this.addUpdateContractMgr.state, additionalBonuses },
      );
    });
  }


  calculatetotalBonusAmount() {
    const {
      paymentRequirement,
      agreement,
      overrideTotalBonusAmountIn,
    } = this.addUpdateContractMgr.state;
    if (!overrideTotalBonusAmountIn) {
      const operandsHaveErrors = this.addUpdateContractMgr.doesElementHaveError('firstYearRentalBonusAmount', false, false)
      || this.addUpdateContractMgr.doesElementHaveError('acreage', false, false);
      if (operandsHaveErrors
        || !(paymentRequirement.firstYearRentalBonusAmount && agreement.acreage)) {
        paymentRequirement.totalBonusAmount = null;
      } else {
        paymentRequirement.totalBonusAmount = roundDecimals(
          paymentRequirement.firstYearRentalBonusAmount * agreement.acreage, 2,
        );
      }
      this.addUpdateContractMgr.setState(
        {
          ...this.addUpdateContractMgr.state,
          paymentRequirement,
        },
      );
      this.render();
    }
  }

  initTotalBonusAmountCheckbox() {
    $(`#${this.container}`).on('change', '#overrideTotalBonusAmountIn', (e) => {
      this.addUpdateContractMgr.setState(
        {
          ...this.addUpdateContractMgr.state,
          overrideTotalBonusAmountIn: $(e.target).is(':checked'),
        },
      );
      this.calculatetotalBonusAmount();
      this.render();
    });
  }

  render() {
    const {
      editmode, overrideTotalBonusAmountIn, paymentRequirement, additionalBonuses,
    } = this.addUpdateContractMgr.state;
    const handlebarData = {
      overrideTotalBonusAmountIn,
      enableTotalBonusAmountEdit: editmode && overrideTotalBonusAmountIn,
      paymentRequirement,
      editmode,
      additionalBonuses,
    };
    $('.additionalBonusContainer').remove();
    const t = template(handlebarData);
    const t2 = additionalBonusesTemplate(handlebarData);
    $(`#${this.container}`).empty().append(t);
    $(t2).insertAfter($(`#${this.container}`));
    this.initAddAdditionalBonusClick();
    this.initDeleteAdditionalBonusClick();
    this.intAdditionalBonusChange();
    formatReadOnlyNumberInputs(this.container);
  }
}
