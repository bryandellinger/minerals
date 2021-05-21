/* eslint-disable max-len */
/* eslint-disable no-restricted-syntax */
import * as constants from '../../constants';
import initCardHeader from '../initCardHeader';
import getFilteredContractSubTypes from './getFilteredContractSubtypes';
import getRentalPayment from '../../services/getRentalPayment';
import elementsToBeValidated from './elementsToBeValidated';
import getShowAltIdInformationInd from './getShowAltIdInformationInd';
import getShowRowContractInd from './getShowRowContractInd';
import getShowCheckSubmissionPeriodInd from './getShowCheckSubmissionPeriodInd';
import getShowStorageFeeInd from './getShowStorageFeeInd';
import getShowAllowableVariancePerAuditFieldPercentInd from './getShowAllowableVariancePerAuditFieldPercentInd';
import getShowTestOfWellEcomonyInd from './getShowTestOfWellEconomyInd';
import getShowAlternateIdInd from './getShowAlternateIdInd';
import getShowDateFieldsInd from './getShowDateFieldsInd';
import formatReadOnlyNumberInputs from '../../services/formatReadOnlyNumberInputs';
import getContractLabel from './getContractLabel';
import getShowTownshipsInd from './getShowTownshipsInd';
import getShowAssociatedContractInd from './getShowAssociatedContractInd';
import getShowSuretyInd from './getShowSuretyInd';
import getShowProducerPriceIndexInd from './getShowProducerPriceIndexInd';
import getShowInflationIntervalPeriodInd from './getShowInflationIntervalPeriodInd';
import getShowReversionDateInd from './getShowReversionDateInd';
import getShowAgreementFeeInd from './getShowAgreementFeeInd';
import getShowAssociatedTractInd from './getShowAssociatedTractInd';
import getShowShutInInd from './getShowShutInInd';
import getShowDataReceivedDateInd from './getShowDataReceivedDateInd';
import getShowStorageFieldInd from './getShowStorageFieldInd';
import getShowNSDAcreageInd from './getShowNSDAcreageInd';
import initSuretyDataTable from './initSuretyDataTable';

const template = require('../../views/contracts/addUpdateContract/index.handlebars');

export default class RenderMgr {
  constructor(AddUpdateContractMgr) {
    this.addUpdateContractMgr = AddUpdateContractMgr;
  }

  render() {
    this.deriveContractNum();
    const {
      contract, contracts, contractTypes, contractSubTypes, editmode,
      updateInd, insertInd, tracts, districts, contractNumOverrideIn,
      elementToFocus, submitInd, tractNum, agreement, terminationReasons,
      townships, altIdCategories, associatedContracts, associatedTracts, rowContracts, paymentRequirement,
        checkSubmissionPeriods, wellId, storage, storageRentalId, contractRentalId, suretyId, months, storageWellPaymentMonths,
        storageBaseRentalPaymentMonths, storageRentalPaymentMonths, contractRentalPaymentMonths, 
      additionalContractInformations, sureties
    } = this.addUpdateContractMgr.state;
    const contractTypesWithSelection = contractTypes.map(
      (contractType) => (
        { ...contractType, selected: contract.contractTypeId === contractType.id }
      ),
    );

    const altIdCategoriesWithSelection = altIdCategories.map(
      (altIdCategory) => (
        {
          ...altIdCategory,
          selected: agreement && agreement.altIdCategoryId === altIdCategory.id,
        }
      ),
    );

    const terminationReasonsWithSelection = terminationReasons.map(
      (terminationReason) => (
        {
          ...terminationReason,
          selected: agreement
                        && terminationReason.id === agreement.terminationReasonId,
        }
      ),
    );

    getRentalPayment({
      acreage: agreement.acreage,
      contractId: contract.id,
      effectiveDate: agreement.effectiveDate,
      secondThroughFourthYearDelayRental: paymentRequirement.secondThroughFourthYearDelayRental,
      fifthYearOnwardDelayRental: paymentRequirement.fifthYearOnwardDelayRental,
    })
      .then((rentalPayment) => {
        if (!contract.contractRentalPaymentOverride) {
          contract.contractRentalPayment = rentalPayment.rentalPaymentDue;
          this.addUpdateContractMgr.setState({
            ...this.addUpdateContractMgr.state, contract,
          });
        }
          const handlebarData = {
          sureties,
          rentalPayment,
          wellId,
          storageRentalId,
          contractRentalId,
          suretyId,
          storage,
          wellUrl: `${window.location.href.split('?')[0].replace('Contracts', 'Wells')}?container=addUpdateWellContainer&wellId=${wellId}`,
          baseUrl: window.location.href.split('?')[0],
          headerInfo: initCardHeader(),
          title: this.addUpdateContractMgr.id ? 'View/Edit Contract' : 'Add Contract',
          contract,
          contractTypesWithSelection,
          terminationReasonsWithSelection,
          filteredContractSubTypes: getFilteredContractSubTypes(
            contractSubTypes, contract.contractTypeId, contract.contractSubTypeId,
          ),
          editmode,
          id: this.addUpdateContractMgr.id,
          updateInd,
          insertInd,
          tracts,
          districts,
          enableContractEdit: editmode && contractNumOverrideIn,
          contractNumOverrideIn,
          contractNumAlreadyExists: !this.addUpdateContractMgr.id
                        && contracts.filter((x) => x.contractNum === contract.contractNum).length,
          contractNumisEmpty: submitInd && !contract.contractNum,
          tractNum,
          showSequenceInd: this.addUpdateContractMgr.isSequence(),
          showLandLeaseInd: contractTypes.find((x) => x.id === contract.contractTypeId && x.contractTypeName === 'Land Lease'),
          agreement,
          townships,
          altIdCategoriesWithSelection,
          showAltIdInformationInd: getShowAltIdInformationInd(agreement, altIdCategories),
          associatedContracts,
          associatedTracts,
          rowContracts,
          showStorageFieldInd: getShowStorageFieldInd(contractTypes, contractSubTypes, contract),
          showAlternateIdInd: getShowAlternateIdInd(contractTypes, contractSubTypes, contract),
          showRowContractInd: getShowRowContractInd(contractTypes, contractSubTypes, contract),
          showDateFieldsInd: getShowDateFieldsInd(contractTypes, contractSubTypes, contract),
          showTownshipsInd: getShowTownshipsInd(contractTypes, contractSubTypes, contract),
          showAssociatedContractInd: getShowAssociatedContractInd(contractTypes, contractSubTypes, contract),
          showAssociatedTractInd: getShowAssociatedTractInd(contractTypes, contractSubTypes, contract),
          showSuretyInd: getShowSuretyInd(contractTypes, contractSubTypes, contract),
          showProducerPriceIndexInd: getShowProducerPriceIndexInd(contractTypes, contractSubTypes, contract),
          showInflationIntervalPeriodInd: getShowInflationIntervalPeriodInd(contractTypes, contractSubTypes, contract),
          showReversionDateInd: getShowReversionDateInd(contractTypes, contractSubTypes, contract),
          showAgreementFeeInd: getShowAgreementFeeInd(contractTypes, contractSubTypes, contract),
          showShutInInd: getShowShutInInd(contractTypes, contractSubTypes, contract),
          showStorageFeeInd: getShowStorageFeeInd(contractTypes, contractSubTypes, contract),
          showDataReceivedDateInd: getShowDataReceivedDateInd(contractTypes, contractSubTypes, contract),
          showNSDAcreageInd: getShowNSDAcreageInd(contractTypes, contractSubTypes, contract),
          showAllowableVariancePerAuditFieldPercentInd: getShowAllowableVariancePerAuditFieldPercentInd(
            contractTypes, contractSubTypes, contract,
          ),
          showTestOfWellEcomonyInd: getShowTestOfWellEcomonyInd(
            contractTypes, contractSubTypes, contract,
          ),
          contractLabel: getContractLabel(contractTypes, contractSubTypes, contract),
          paymentRequirement,
          checkSubmissionPeriods,
          showCheckSubmissionPeriodInd: getShowCheckSubmissionPeriodInd(
            contractTypes, contractSubTypes, contract,
          ),
          shutInPaymentIntervalToggleIsChecked: paymentRequirement
                        && paymentRequirement.shutInPaymentInterval === constants.ShutinPaymentInterval_Annualy,
          secondThroughFourthYearShutInRateIntervalToggleIsChecked: paymentRequirement
                        && paymentRequirement.secondThroughFourthYearShutInRateInterval
                        === constants.ShutinPaymentInterval_Annualy,
          storageWellPaymentMonths: months.map((month) => (
            {
              ...month,
              selected: storageWellPaymentMonths.map((x) => x.id).includes(month.id),
            }
          )),
          storageBaseRentalPaymentMonths: months.map((month) => (
            {
              ...month,
              selected: storageBaseRentalPaymentMonths.map((x) => x.id).includes(month.id),
            }
          )),
          storageRentalPaymentMonths: months.map((month) => (
            {
              ...month,
              selected: storageRentalPaymentMonths.map((x) => x.id).includes(month.id),
            }
          )),
          contractRentalPaymentMonths: months.map((month) => (
            {
              ...month,
              selected: contractRentalPaymentMonths.map((x) => x.id).includes(month.id),
            }
          )),
            additionalContractInformations: additionalContractInformations.map((item) => (
                {
                    ...item,
                    selected: contract.additionalContractInformationId === item.id
                }
            )),
          };
        const t = template(handlebarData);
        this.addUpdateContractMgr.container.empty().append(t);
        this.addUpdateContractMgr.pluggingSuretyMgr.render();
        this.addUpdateContractMgr.pluggingSuretyMgr.init();
        this.addUpdateContractMgr.totalBonusAmountMgr.render();
        this.addUpdateContractMgr.totalBonusAmountMgr.init();
          this.addUpdateContractMgr.contractEventDetailsMgr.render();
          if (this.addUpdateContractMgr.suretyTable) {
              this.addUpdateContractMgr.suretyTable.destroy();
              this.addUpdateContractMgr.suretyTable = null;
          }
          this.addUpdateContractMgr.suretyTable = initSuretyDataTable(this.addUpdateContractMgr.state.sureties.map(
              (surety) => (
                  {
                      ...surety,
                      issueDate: surety.issueDate ? new Date(surety.issueDate) : null,
                      releasedDate: surety.releasedDate ? new Date(surety.releasedDate) : null,
                      contractId: contract.id || 0
                  }
              ),
          ));
        $('#subjectToInflationIndToggle').bootstrapToggle({
          on: 'Yes',
          off: 'No',
          onstyle: 'secondary',
          offstyle: 'secondary',
        });
        $('#shutInPaymentIntervalToggle').bootstrapToggle({
          on: constants.ShutinPaymentInterval_Annualy,
          off: constants.ShutinPaymentInterval_Monthly,
          onstyle: 'secondary',
          offstyle: 'secondary',
        });
        $('#secondThroughFourthYearShutInRateIntervalToggle').bootstrapToggle({
          on: constants.ShutinPaymentInterval_Annualy,
          off: constants.ShutinPaymentInterval_Monthly,
          onstyle: 'secondary',
          offstyle: 'secondary',
        });
        $('#fiveYearInflationIntervalPeriodIndToggle').bootstrapToggle({
          on: '5 Years',
          off: 'N/A',
          onstyle: 'secondary',
          offstyle: 'secondary',
        });
        $('#nsdAcreageAppliesToAllInd').bootstrapToggle({
                on: 'Yes',
            off: 'No',
              onstyle: 'secondary',
              offstyle: 'secondary',
          });
        $('#chkToggle2').bootstrapToggle();
        $('#tractid').select2({ placeholder: 'Please select a tract' });
        this.addUpdateContractMgr.initTractSelect();
        $('#districtid').select2({ placeholder: 'Select all that apply', width: '100%' });
        $('#wellPaymentsDue').select2({ placeholder: 'Select all that apply', width: '100%' });
        $('#baseRentalPaymentsDue').select2({ placeholder: 'Select all that apply', width: '100%' });
        $('#contractRentalPaymentsDue').select2({ placeholder: 'Select all that apply', width: '100%' });
        $('#rentalPaymentsDue').select2({ placeholder: 'Select all that apply', width: '100%' });
        $('#townshipid').select2({ placeholder: 'Start typing county or municipality', width: '100%', minimumInputLength: 2 });
        $('#associatedContract').select2({ placeholder: 'Select all that apply', width: '100%' });
        $('#associatedTract').select2({ placeholder: 'Select all that apply', width: '100%' });
        $('#rowcontractselect').select2({ width: '100%' });
        $('#additionalContractInformation').select2({
              width: '100%',
              placeholder: 'N/A',
              allowClear: true,
              minimumResultsForSearch: -1});
        this.addUpdateContractMgr.initDistrictChange();
        this.addUpdateContractMgr.initDistrictClose();
        this.addUpdateContractMgr.initWellPaymentsDueChange();
        this.addUpdateContractMgr.initBaseRentalPaymentsDueChange();
        this.addUpdateContractMgr.initRentalPaymentsDueChange();
        this.addUpdateContractMgr.initContractRentalPaymentsDueChange();
        this.addUpdateContractMgr.initTownshipChange();
        this.addUpdateContractMgr.initAssociatedContractChange();
        this.addUpdateContractMgr.initAssociatedTractChange();
        this.addUpdateContractMgr.initLeaseNumChange();
        this.addUpdateContractMgr.initContractRentalPaymentOverrideChange();
        this.addUpdateContractMgr.initAdditionalContractInformationChange();
        this.initTractAutoComplete();
        this.initTerminationReasonAutoComplete();
        this.addUpdateContractMgr.datePickerMgr.init('effectiveDate', 'mm/dd/yy', true, true, true);
        this.addUpdateContractMgr.datePickerMgr.init('reversionDate', 'mm/dd/yy', true, true, true);
        this.addUpdateContractMgr.datePickerMgr.init('dataReceivedDate', 'mm/dd/yy', true, true, true);
        this.addUpdateContractMgr.datePickerMgr.init('terminationDate', 'mm/dd/yy', true, true, true);
        this.addUpdateContractMgr.datePickerMgr.init('rentalShutInDueDate', 'MM d', true, false, false);
        this.addUpdateContractMgr.datePickerMgr.init('expirationDate', 'mm/dd/yy', true, true, true);
        for (const item of elementsToBeValidated) {
          this.addUpdateContractMgr.doesElementHaveError(
            item.element, item.showOnlyIfFormSubmitted, item.showOnlyIfNoOverride,
          );
        }
        if (elementToFocus && document.getElementById(elementToFocus)) {
          document.getElementById(elementToFocus).focus();
          if ($(`#${elementToFocus}`).data('select2')) {
            $(`#${elementToFocus}`).select2('focus');
          }
          this.addUpdateContractMgr.setState(
            { ...this.addUpdateContractMgr.state, elementToFocus: null },
          );
        }
        this.addUpdateContractMgr.initFormSubmit();
        formatReadOnlyNumberInputs('addUpdateContractContainer');
      });
  }

  initTractAutoComplete() {
    const { tracts } = this.addUpdateContractMgr.state;
    const source = Array.from(new Set(tracts.map((x) => (x.administrative ? `${x.tractNum}(admin)` : x.tractNum))));
    const sourceWithNoNulls = source.filter((e) => e === 0 || e);

    $('#tractNumber').autocomplete({
      source: sourceWithNoNulls,
      minLength: 2,
      select: (event, ui) => {
        this.addUpdateContractMgr.setState(
          {
            ...this.addUpdateContractMgr.state, tractNum: ui.item.label,
          },
        );
      },
    });
  }

  initTerminationReasonAutoComplete() {
    const { terminationReasons } = this.addUpdateContractMgr.state;
    const source = Array.from(new Set(terminationReasons.map((x) => (x.reason))));
    const sourceWithNoNulls = source.filter((e) => e === 0 || e).sort();
    $('#terminationReason').autocomplete({
      source: sourceWithNoNulls,
      minLength: 0,
      select: (event, ui) => {
        const { agreement } = this.addUpdateContractMgr.state;
        agreement.terminationReason = ui.item.label;
        this.addUpdateContractMgr.setState(
          { ...this.addUpdateContractMgr.state, agreement },
        );
      },
    // eslint-disable-next-line func-names
    }).focus(function () {
      $(this).autocomplete('search', '');
    });
  }

  deriveContractNum() {
    const {
      contract, contractTypes, contractSubTypes, tractNum, districtContractJunctions,
      districts, contractNumOverrideIn,
    } = this.addUpdateContractMgr.state;
    if (!contractNumOverrideIn) {
      let derivedContractNum = '';
      let contractNumPrefix = '';
      if (contract.contractTypeId) {
        contractNumPrefix = contractTypes.filter(
          (x) => x.id === contract.contractTypeId,
        ).map((y) => y.contractNumPrefix).join();
        derivedContractNum = contractNumPrefix;
      }
      if (contract.contractSubTypeId && !contractNumPrefix) {
        contractNumPrefix = contractSubTypes.filter(
          (x) => x.id === contract.contractSubTypeId,
        ).map((y) => y.contractNumPrefix).join();
        derivedContractNum = contractNumPrefix;
      }
      if (this.addUpdateContractMgr.isSequence()) {
        if (contract.sequence) {
          derivedContractNum = `${derivedContractNum}${contract.sequence}`;
        }
      } else if (tractNum) {
        derivedContractNum = `${derivedContractNum}${tractNum.replace('(admin)', '')}`;
      }
      const filteredDistrictContractJunctions = districtContractJunctions.filter(
        (x) => x.contractId === contract.id,
      ).map((c) => c.districtId);
      if (filteredDistrictContractJunctions.length) {
        derivedContractNum = `${derivedContractNum}-${districts.filter((x) => filteredDistrictContractJunctions.includes(x.id))
          .map((y) => y.districtId.toString().padStart(2, '0')).join('-')}`;
      }

      contract.contractNum = derivedContractNum;
      this.addUpdateContractMgr.setState({ ...this.addUpdateContractMgr.state, contract });
    }
  }
}
