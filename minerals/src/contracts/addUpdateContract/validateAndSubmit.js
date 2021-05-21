/* eslint-disable no-restricted-syntax */
import elementsToBeValidated from './elementsToBeValidated';
import AjaxService from '../../services/ajaxService';


export default class ValidateandSubmitMgr {
  constructor(AddUpdateContractMgr) {
    this.addUpdateContractMgr = AddUpdateContractMgr;
  }

  doesElementHaveError(element, showOnlyIfFormSubmitted, showOnlyIfNoOverride) {
    const { submitInd, contractNumOverrideIn } = this.addUpdateContractMgr.state;
    if (!$(`#${element}`).length) {
      return false;
    }
    $(`#${element}Error`).hide();
    $(`#${element}`).removeClass('is-invalid');
    if ($(`#${element}`).data('select2')) {
      $($(`#${element}`).data('select2').$container).removeClass('is-invalid');
    }

    if (showOnlyIfFormSubmitted && !submitInd) {
      return false;
    }
    if (showOnlyIfNoOverride && (this.addUpdateContractMgr.id !== 0 || contractNumOverrideIn)) {
      return false;
    }
    if (!$(`#${element}`)[0].checkValidity()) {
      $(`#${element}`).addClass('is-invalid');
      if ($(`#${element}`).data('select2')) {
        $($(`#${element}`).data('select2').$container).addClass('is-invalid');
      }
      $(`#${element}Error`).show();
      return true;
    }
    return false;
  }

  initFormSubmit() {
    // eslint-disable-next-line consistent-return
    // eslint-disable-next-line func-names
    $('#addUpdatePaymentForm button[type=submit]').click(function () {
      $('button[type=submit]', $(this).parents('form')).removeAttr('clicked');
      $(this).attr('clicked', 'true');
    });

    // eslint-disable-next-line consistent-return
    $('#addUpdatePaymentForm').submit(() => {
      const continueInd = $('button[type=submit][clicked=true]').data('continue');

      this.addUpdateContractMgr.setState({ ...this.addUpdateContractMgr.state, submitInd: true });
      const {
        contract, contracts, tractNum, districtContractJunctions,
        agreement, townships, rowContracts,
        paymentRequirement, pluggingSuretyDetails, associatedContracts,
        associatedTracts, contractNumOverrideIn, contractEventDetails,
        reasonForChangeId, initialAcreage, reasonForChangeDescription,
          updateWellInfoInd, wellId, storage, storageRentalId, contractRentalId, suretyId, storageWellPaymentMonths,
        storageBaseRentalPaymentMonths, storageRentalPaymentMonths,
        contractRentalPaymentMonths, additionalBonuses,
      } = this.addUpdateContractMgr.state;
      let formHasError = false;
      if (!this.addUpdateContractMgr.id && contracts.filter(
        (x) => x.contractNum === contract.contractNum,
      ).length) {
        formHasError = true;
      }
      if (!contract.contractNum) {
        this.addUpdateContractMgr.render();
        formHasError = true;
      }
      for (const item of elementsToBeValidated) {
        if (this.addUpdateContractMgr.doesElementHaveError(
          item.element, item.showOnlyIfFormSubmitted, item.showOnlyIfNoOverride,
        )) {
          formHasError = true;
        }
      }
      for (const item of $('.currentEventValidatedField').toArray()) {
        if (this.addUpdateContractMgr.contractEventDetailsMgr.doesElementHaveError(item)) {
          formHasError = true;
        }
      }

      if ($('.currentEventValidatedField').length) {
        if ($('#lesseeEffectiveDate').datepicker('getDate') === null) {
          $('#lesseeEffectiveDate').addClass('is-invalid');
          $('#lesseeEffectiveDateError').show();
          formHasError = true;
        }
      }

      if (formHasError) {
        window.Toastr.error('Please fix errors and resubmit');
        return false;
      }

      $('.spinner').show();
      $('#contractSubmitbtn').hide();
      $('#contractSubmitBtnDisabled').show();
      $(':submit').attr('disabled', 'disabled');
      const data = {
        id: this.addUpdateContractMgr.id,
        storage,
        contractNum: contract.contractNum,
        tractNum: tractNum ? tractNum.replace('(admin)', '') : null,
        contractTypeId: contract.contractTypeId,
        contractSubTypeId: contract.contractSubTypeId,
        districtIds: districtContractJunctions.filter(
          (x) => x.contractId === contract.id,
        ).map((c) => c.districtId),
        townshipIds: townships.filter((x) => x.selected).map((c) => c.id),
        associatedContracts: associatedContracts.filter((x) => x.selected).map((c) => c.id),
        associatedTracts: associatedTracts.filter((x) => x.selected).map((c) => c.id),
        sequence: contract.sequence,
        contractNumOverride: contractNumOverrideIn,
        effectiveDate: agreement.effectiveDate,
        terminationDate: agreement.terminationDate,
        terminationReason: agreement.terminationReason,
        inPoolAcreage: agreement.inPoolAcreage ? parseFloat(agreement.inPoolAcreage) : null,
        bufferAcreage: agreement.bufferAcreage ? parseFloat(agreement.bufferAcreage) : null,
        nsdAcreage: agreement.nsdAcreage ? parseFloat(agreement.nsdAcreage) : null,
        nsdAcreageAppliesToAllInd: agreement.nsdAcreageAppliesToAllInd,
        acreage: parseFloat(agreement.acreage),
        altIdInformation: agreement.altIdInformation,
        altIdCategoryId: agreement.altIdCategoryId,
        reversionDate: agreement.reversionDate,
        dataReceivedDate: agreement.dataReceivedDate,
        lesseeEffectiveDate: agreement.lesseeEffectiveDate,
        expirationDate: agreement.expirationDate,
        rowContracts,
        checkSubmissionPeriod: paymentRequirement.checkSubmissionPeriod,
        firstYearRentalBonusAmount: paymentRequirement.firstYearRentalBonusAmount,
        totalBonusAmount: paymentRequirement.totalBonusAmount,
        leaseExtensionBonus: paymentRequirement.leaseExtensionBonus,
        rentalShutInDueDate: paymentRequirement.rentalShutInDueDate,
        secondThroughFourthYearDelayRental: paymentRequirement.secondThroughFourthYearDelayRental,
        secondThroughFourthYearShutInRate: paymentRequirement.secondThroughFourthYearShutInRate,
        fifthYearOnwardDelayRental: paymentRequirement.fifthYearOnwardDelayRental,
        fifthYearOnwardShutInRate: paymentRequirement.fifthYearOnwardShutInRate,
        shutInPaymentInterval: paymentRequirement.shutInPaymentInterval,
        secondThroughFourthYearShutInRateInterval:
         paymentRequirement.secondThroughFourthYearShutInRateInterval,
        storageFee: paymentRequirement.storageFee,
        allowableVariancePerAuditFieldPercent:
         paymentRequirement.allowableVariancePerAuditFieldPercent,
        producerPriceIndex: paymentRequirement.producerPriceIndex,
        fiveYearInflationIntervalPeriodInd: paymentRequirement.fiveYearInflationIntervalPeriodInd,
        testOfWellEconomy: paymentRequirement.testOfWellEconomy,
        PerformanceSurety: paymentRequirement.performanceSurety,
        AgreementFee: paymentRequirement.agreementFee,
        pluggingSuretyDetails,
        contractEventDetails,
        reasonForChangeId,
        initialAcreage: parseFloat(initialAcreage),
        reasonForChangeDescription,
        updateWellInfoInd,
        storageWellPaymentMonthIds: storageWellPaymentMonths.map((x) => x.id),
        storageBaseRentalPaymentMonthIds: storageBaseRentalPaymentMonths.map((x) => x.id),
        storageRentalPaymentMonthIds: storageRentalPaymentMonths.map((x) => x.id),
        contractRentalPaymentMonthIds: contractRentalPaymentMonths.map((x) => x.id),
        additionalBonuses: additionalBonuses.map((x) => parseFloat(x.additionalBonusAmount)),
        contractRentalPayment: contract.contractRentalPayment ? parseFloat(contract.contractRentalPayment) : null,
        contractRentalPaymentOverride: contract.contractRentalPaymentOverride,
        additionalContractInformationId: contract.additionalContractInformationId ? parseInt(contract.additionalContractInformationId, 10) : null, 
      };
      AjaxService.ajaxPost('./api/ContractMgrApi', data)
        .then((d) => {
          $('.spinner').hide();
          window.Toastr.success('Your contract has been saved', 'Save Successful', { positionClass: 'toast-top-center' });
          this.addUpdateContractMgr.manageContractsMgr.init();
            this.addUpdateContractMgr.init(d.id, wellId, storageRentalId, contractRentalId, suretyId);
          this.addUpdateContractMgr.manageTractsMgr.init();
          if (continueInd) {
            this.addUpdateContractMgr.setState(
              { ...this.addUpdateContractMgr.state, editmode: true },
            );
            this.addUpdateContractMgr.render();
          }
        })
        .catch(() => {
          $('#contractSubmitbtn').show();
          $('#contractSubmitBtnDisabled').hide();
          $('.spinner').hide();
        });
    });
  }
}
