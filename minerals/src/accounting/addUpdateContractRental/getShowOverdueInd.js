/* eslint-disable max-len */
import * as constants from '../../constants';

export default function getShowOverdueInd(
  periodType, contractRental, contractRentalPaymentMonths,
) {
  if (periodType.periodTypeName === constants.PeriodTypeBonus ) {
    return false;
    }
    if (!contractRentalPaymentMonths || !contractRentalPaymentMonths.length) {
        return false;
    }
  if (contractRental && contractRental.contractRentalEntryDate && contractRental.contractPaymentPeriodYear) {
    if (parseInt(contractRental.contractPaymentPeriodYear, 10) < parseInt(contractRental.contractRentalEntryDate.getFullYear(), 10)) {
      return true;
    }
    if (parseInt(contractRental.contractPaymentPeriodYear, 10) > parseInt(contractRental.contractRentalEntryDate.getFullYear(), 10)) {
      return false;
    }
    if (periodType) {
        if (periodType.periodTypeName === constants.PeriodTypeAnnual || periodType.periodTypeName === constants.PeriodTypeShutIn) {
        const paymentMonth = contractRental.contractRentalEntryDate.getMonth() + 1;
        const rentalMonth = contractRentalPaymentMonths[0].monthNum;
        if (paymentMonth > rentalMonth) {
          return true;
        }
      }

      if (periodType.periodTypeName === constants.PeriodTypeQuarter1
         || periodType.periodTypeName === constants.PeriodTypeQuarter2
         || periodType.periodTypeName === constants.PeriodTypeQuarter3
         || periodType.periodTypeName === constants.PeriodTypeQuarter4) {
        const paymentMonth = contractRental.contractRentalEntryDate.getMonth() + 1;

        if (!contractRentalPaymentMonths || contractRentalPaymentMonths.length !== 4) {
          return false;
        }

        let i;
        switch (periodType.periodTypeName) {
          case constants.PeriodTypeQuarter1:
            i = 0;
            break;
          case constants.PeriodTypeQuarter2:
            i = 1;
            break;
          case constants.PeriodTypeQuarter3:
            i = 2;
            break;
          default:
            i = 3;
        }

        if (paymentMonth > contractRentalPaymentMonths[i].monthNum) {
          return true;
        }
      }
    }
  }
  return false;
}
