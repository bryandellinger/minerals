import formatReadOnlyNumberInputs from '../../services/formatReadOnlyNumberInputs';
import roundDecimals from '../../services/roundDecimals';

const template = require('../../views/contracts/addUpdateContract/minimumRoyaltySalesPrice.handlebars');

export default class MinimumRoyaltySalesPriceMgr {
  constructor(AddUpdateContractMgr) {
    this.addUpdateContractMgr = AddUpdateContractMgr;
    this.container = 'minimumRoyaltySalesPriceContainer';
  }

  init() {
    this.initMimimumRoyaltySalesPriceChange();
    this.initMinimumRoyaltySalesPriceCheckbox();
  }

  initMimimumRoyaltySalesPriceChange() {
    $(`#${this.container}`).on('change', '#minimumRoyaltySalesPrice', () => {
      const { paymentRequirement } = this.addUpdateContractMgr.state;
      paymentRequirement.minimumRoyaltySalesPrice = parseFloat($('#minimumRoyaltySalesPrice').val());
      this.addUpdateContractMgr.setState(
        { ...this.addUpdateContractMgr.state, paymentRequirement },
      );
      this.addUpdateContractMgr.doesElementHaveError('minimumRoyaltySalesPrice', false, false);
    });
  }

  calculateMimimumRoyaltySalesPrice() {
    const self = this;
    const {
      paymentRequirement,
      overrideMinimumRoyaltySalesPriceIn,
    } = self.addUpdateContractMgr.state;
    if (!overrideMinimumRoyaltySalesPriceIn) {
      const operandsHaveErrors = self.addUpdateContractMgr.doesElementHaveError('minimumRoyalty', false, false)
                || this.addUpdateContractMgr.doesElementHaveError('royaltyPercent', false, false);
      if (operandsHaveErrors
                || !(paymentRequirement.royaltyPercent && paymentRequirement.minimumRoyalty)) {
        paymentRequirement.minimumRoyaltySalesPrice = null;
      } else {
        paymentRequirement.minimumRoyaltySalesPrice = roundDecimals(
          paymentRequirement.minimumRoyalty / (
            paymentRequirement.royaltyPercent / 100
          ),
          4,
        );
      }
      self.addUpdateContractMgr.setState(
        {
          ...self.addUpdateContractMgr.state,
          paymentRequirement,
        },
      );
    }
    this.render();
  }

  initMinimumRoyaltySalesPriceCheckbox() {
    $(`#${this.container}`).on('change', '#overrideMinimumRoyaltySalesPriceIn', (e) => {
      this.addUpdateContractMgr.setState(
        {
          ...this.addUpdateContractMgr.state,
          overrideMinimumRoyaltySalesPriceIn: $(e.target).is(':checked'),
        },
      );
      this.calculateMimimumRoyaltySalesPrice();
    });
  }

  render() {
    const {
      editmode, overrideMinimumRoyaltySalesPriceIn, paymentRequirement,
    } = this.addUpdateContractMgr.state;
    const handlebarData = {
      overrideMinimumRoyaltySalesPriceIn,
      enableMinimumRoyaltySalesPriceEdit: editmode && overrideMinimumRoyaltySalesPriceIn,
      paymentRequirement,
      editmode,
    };
    const t = template(handlebarData);
    $(`#${this.container}`).empty().append(t);
    formatReadOnlyNumberInputs(this.container);
  }
}
