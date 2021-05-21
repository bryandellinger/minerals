﻿import * as constants from '../../constants';

export default function initState() {
  return {
    contract: { id: 0 },
    contracts: null,
    additionalBonuses: [],
    landLeaseAgreements: [],
    agreement: {},
    originalContract: {},
    storage: { id: 0 },
    contractTypes: null,
    contractSubTypes: null,
    editmode: false,
    updateInd: true,
    insertInd: false,
    tracts: null,
    terminatedTracts: [],
    districts: null,
    districtContractJunctions: null,
    contractNumOverrideIn: false,
    overrideMinimumRoyaltySalesPriceIn: false,
    overrideTotalBonusAmountIn: false,
    elementToFocus: null,
    submitInd: false,
    tractNum: null,
    terminationReasons: [],
    townships: [],
    altIdCategories: [],
    associatedContracts: [],
    associatedTracts: [],
    rowContracts: [],
    wellTractInfos: [],
    wells: [],
    months: [],
    storageWellPaymentMonths: [],
    storageBaseRentalPaymentMonths: [],
    storageRentalPaymentMonths: [],
    contractRentalPaymentMonths: [],
    paymentRequirement: {
      checkSubmissionPeriod: constants.Ninety,
      shutInPaymentInterval: constants.ShutinPaymentInterval_Monthly,
      secondThroughFourthYearShutInRateInterval: constants.ShutinPaymentInterval_Monthly,
      allowableVariancePerAuditFieldPercent: constants.Three,
      fiveYearInflationIntervalPeriodInd: false,
    },
    checkSubmissionPeriods: [
      { value: constants.Thirty, checked: false },
      { value: constants.Sixty, checked: false },
      { value: constants.Ninety, checked: true },
      { value: constants.OneHundredEighty, checked: false },
    ],
    pluggingSuretyDetails: [],
    pluggingSuretyDetailsMeasurementType: constants.MeasurementType_TVD,
    contractEventDetails: [],
    contractEventDetailsHistory: [],
    contractEventDetailReasonsForChange: [],
    lessees: [],
    reasonForChangeId: 0,
    initialAcreage: 0,
    reasonForChangeDescription: null,
    updateWellInfoInd: false,
    wellId: 0,
    storageRentalId: 0,
    contractRentalId: 0,
    suretyId: 0,
    additionalContractInformations: [],
    sureties: [],
  };
}
