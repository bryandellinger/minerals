import updateCheckSubmissionPeriods from './updateCheckSubmissionPeriods';
import * as constants from '../../constants';

export default class PaymentRequirementmgr {
  constructor(AddUpdateContractMgr) {
    this.addUpdateContractMgr = AddUpdateContractMgr;
    this.container = AddUpdateContractMgr.container;
  }

  init() {
    this.initCheckSubmissionPeriodChange();
    this.initFirstYearRentalBonusAmountChange();
    this.initLeaseExtensionBonusChange();
    this.initSecondThroughFourthYearDelayRentalChange();
    this.initSecondThroughFourthYearShutInRateChange();
    this.initFifthYearOnwardDelayRentalChange();
    this.initFifthYearOnwardShutInRateChange();
    this.initStorageFeeChange();
    this.initSubjectToInflationIndToggle();
    this.initShutInPaymentIntervalToggle();
    this.initSecondThroughFourthYearShutInRateIntervalToggle();
    this.initFiveYearInflationIntervalPeriodIndToggle();
    this.initAllowableVariancePerAuditFieldPercentChange();
    this.initProducerPriceIndexChange();
    this.initTestOfWellEconomyChange();
    this.initPerformanceSuretyChange();
    this.initAgreementFeeChange();
    this.initStorageBaseRentalPayment();
    this.initContractRentalPayment();
    this.initStorageRentalPayment();
    this.initStorageWellPayment();
  }

    commonPaymentRequirementChangeHandler(element, isIntegerInd, renderIn) {
    this.container.on('change', `#${element}`, () => {
      const { paymentRequirement } = this.addUpdateContractMgr.state;
      paymentRequirement[element] = isIntegerInd ? parseInt($(`#${element}`).val(), 10) : parseFloat($(`#${element}`).val());
      this.addUpdateContractMgr.setState(
        { ...this.addUpdateContractMgr.state, paymentRequirement },
      );
        this.addUpdateContractMgr.doesElementHaveError(element, false, false);
        if (renderIn) {
            this.addUpdateContractMgr.render();
        }
    });
  }

    commonStorageChangeHandler(element, isIntegerInd, renderIn) {
      this.container.on('change', `#${element}`, () => {
        const { storage, contract } = this.addUpdateContractMgr.state;
        if (element === 'contractRentalPayment') {
            contract[element] = isIntegerInd ? parseInt($(`#${element}`).val(), 10) : parseFloat($(`#${element}`).val());
        } else {
            storage[element] = isIntegerInd ? parseInt($(`#${element}`).val(), 10) : parseFloat($(`#${element}`).val());
        }
      this.addUpdateContractMgr.setState(
        { ...this.addUpdateContractMgr.state, storage, contract },
      );
        this.addUpdateContractMgr.doesElementHaveError(element, false, false);
          if (renderIn) {
              this.addUpdateContractMgr.render();
          }
    });
  }

  initStorageRentalPayment() { this.commonStorageChangeHandler('storageRentalPayment', false, false); }

    initStorageBaseRentalPayment() { this.commonStorageChangeHandler('storageBaseRentalPayment', false, false); }

    initContractRentalPayment() { this.commonStorageChangeHandler('contractRentalPayment', false, false);  }

    initStorageWellPayment() { this.commonStorageChangeHandler('storageWellPayment', false, false); }

    initFifthYearOnwardShutInRateChange() { this.commonPaymentRequirementChangeHandler('fifthYearOnwardShutInRate', false, false); }

    initFifthYearOnwardDelayRentalChange() { this.commonPaymentRequirementChangeHandler('fifthYearOnwardDelayRental', false, true); }

    initSecondThroughFourthYearShutInRateChange() { this.commonPaymentRequirementChangeHandler('secondThroughFourthYearShutInRate', false, false); }

    initSecondThroughFourthYearDelayRentalChange() { this.commonPaymentRequirementChangeHandler('secondThroughFourthYearDelayRental', false, true); }

    initLeaseExtensionBonusChange() { this.commonPaymentRequirementChangeHandler('leaseExtensionBonus', false, false); }

    initStorageFeeChange() { this.commonPaymentRequirementChangeHandler('storageFee', false, false); }

    initProducerPriceIndexChange() { this.commonPaymentRequirementChangeHandler('producerPriceIndex', false, false); }

    initPerformanceSuretyChange() { this.commonPaymentRequirementChangeHandler('performanceSurety', false, false); }

    initAgreementFeeChange() { this.commonPaymentRequirementChangeHandler('agreementFee', false, false); }

  initAllowableVariancePerAuditFieldPercentChange() {
      this.commonPaymentRequirementChangeHandler('allowableVariancePerAuditFieldPercent', true, false);
  }

    initTestOfWellEconomyChange() { this.commonPaymentRequirementChangeHandler('testOfWellEconomy', true, false); }

  initShutInPaymentIntervalToggle() {
    this.container.on('change', '#shutInPaymentIntervalToggle', (e) => {
      const { paymentRequirement } = this.addUpdateContractMgr.state;
      paymentRequirement.shutInPaymentInterval = $(e.target).prop('checked') ? constants.ShutinPaymentInterval_Annualy : constants.ShutinPaymentInterval_Monthly;
      this.addUpdateContractMgr.setState(
        { ...this.addUpdateContractMgr.state, paymentRequirement },
      );
    });
  }

  initSubjectToInflationIndToggle() {
    this.container.on('change', '#subjectToInflationIndToggle', (e) => {
      const { storage } = this.addUpdateContractMgr.state;
      storage.subjectToInflationInd = $(e.target).prop('checked');
      this.addUpdateContractMgr.setState(
        { ...this.addUpdateContractMgr.state, storage },
      );
    });
  }

  initSecondThroughFourthYearShutInRateIntervalToggle() {
    this.container.on('change', '#secondThroughFourthYearShutInRateIntervalToggle', (e) => {
      const { paymentRequirement } = this.addUpdateContractMgr.state;
      paymentRequirement.secondThroughFourthYearShutInRateInterval = $(e.target).prop('checked') ? constants.ShutinPaymentInterval_Annualy : constants.ShutinPaymentInterval_Monthly;
      this.addUpdateContractMgr.setState(
        { ...this.addUpdateContractMgr.state, paymentRequirement },
      );
    });
  }

  initFiveYearInflationIntervalPeriodIndToggle() {
    this.container.on('change', '#fiveYearInflationIntervalPeriodIndToggle', (e) => {
      const { paymentRequirement } = this.addUpdateContractMgr.state;
      paymentRequirement.fiveYearInflationIntervalPeriodInd = $(e.target).prop('checked');
      this.addUpdateContractMgr.setState(
        { ...this.addUpdateContractMgr.state, paymentRequirement },
      );
    });
  }

  initFirstYearRentalBonusAmountChange() {
    this.container.on('change', '#firstYearRentalBonusAmount', () => {
      const { paymentRequirement } = this.addUpdateContractMgr.state;
      paymentRequirement.firstYearRentalBonusAmount = parseFloat($('#firstYearRentalBonusAmount').val());
      this.addUpdateContractMgr.setState(
        { ...this.addUpdateContractMgr.state, paymentRequirement },
      );
      this.addUpdateContractMgr.doesElementHaveError('firstYearRentalBonusAmount', false, false);
      this.addUpdateContractMgr.totalBonusAmountMgr.calculatetotalBonusAmount();
    });
  }

  initCheckSubmissionPeriodChange() {
    this.container.on('click', 'input[name="checkSubmissionPeriods"]', (e) => {
      const { paymentRequirement, checkSubmissionPeriods } = this.addUpdateContractMgr.state;
      $('input[name="checkSubmissionPeriods"]').removeClass('btn-primary');
      $('input[name="checkSubmissionPeriods"]').addClass('btn-secondary');
      $(e.target).removeClass('btn-secondary');
      $(e.target).addClass('btn-primary');

      paymentRequirement.checkSubmissionPeriod = parseInt($(e.target).val(), 10);
      this.addUpdateContractMgr.setState(
        {
          ...this.addUpdateContractMgr.state,
          paymentRequirement,
          checkSubmissionPeriods: updateCheckSubmissionPeriods(
            paymentRequirement.checkSubmissionPeriod, checkSubmissionPeriods,
          ),
        },
      );
    });
  }
}
