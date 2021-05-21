import * as constants from '../../constants';

export default function getShowOverdueInd(
  periodType, storageRentalPaymentType, storageBaseRentalPaymentMonths, storageRentalPaymentMonths, storageWellPaymentMonths,
  storageRental,
) {
  if (!storageRentalPaymentType.storageRentalPaymentTypeName) {
    return false;
  }


  if (storageRental && storageRental.storageRentalEntryDate && storageRental.paymentPeriodYear) {
    if (parseInt(storageRental.paymentPeriodYear, 10) < parseInt(storageRental.storageRentalEntryDate.getFullYear(), 10)) {
      return true;
    }
    if (parseInt(storageRental.paymentPeriodYear, 10) > parseInt(storageRental.storageRentalEntryDate.getFullYear(), 10)) {
      return false;
    }
    if (periodType && storageRentalPaymentType) {
      if (periodType.periodTypeName === constants.PeriodTypeAnnual) {
        if (storageRentalPaymentType.storageRentalPaymentTypeName === constants.StorageRentalTypeBaseRental) {
          if (storageBaseRentalPaymentMonths && storageBaseRentalPaymentMonths.length) {
            const paymentMonth = storageRental.storageRentalEntryDate.getMonth() + 1;
            const baseRentalMonth = storageBaseRentalPaymentMonths[0].monthNum;
            if (paymentMonth > baseRentalMonth) {
              return true;
            }
          }
        }

        if (storageRentalPaymentType.storageRentalPaymentTypeName === constants.StorageRentalTypeRental) {
          if (storageRentalPaymentMonths && storageRentalPaymentMonths.length) {
            const paymentMonth = storageRental.storageRentalEntryDate.getMonth() + 1;
            const rentalMonth = storageRentalPaymentMonths[0].monthNum;
            if (paymentMonth > rentalMonth) {
              return true;
            }
          }
        }

        if (storageRentalPaymentType.storageRentalPaymentTypeName === constants.StorageRentalTypeWell) {
          if (storageWellPaymentMonths && storageWellPaymentMonths.length) {
            const paymentMonth = storageRental.storageRentalEntryDate.getMonth() + 1;
            const wellMonth = storageWellPaymentMonths[0].monthNum;
            if (paymentMonth > wellMonth) {
              return true;
            }
          }
        }
      }

      if (periodType.periodTypeName === constants.PeriodTypeQuarter1
         || periodType.periodTypeName === constants.PeriodTypeQuarter2
         || periodType.periodTypeName === constants.PeriodTypeQuarter3
         || periodType.periodTypeName === constants.PeriodTypeQuarter4) {
        const paymentMonth = storageRental.storageRentalEntryDate.getMonth() + 1;
        let storagePaymentMonths;

        switch (storageRentalPaymentType.storageRentalPaymentTypeName) {
          case constants.StorageRentalTypeBaseRental:
            storagePaymentMonths = storageBaseRentalPaymentMonths;
            break;
          case constants.StorageRentalTypeRental:
            storagePaymentMonths = storageRentalPaymentMonths;
            break;
          default:
            storagePaymentMonths = storageWellPaymentMonths;
        }
        if (!storagePaymentMonths || storagePaymentMonths.length !== 4) {
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

        if (paymentMonth > storagePaymentMonths[i].monthNum) {
          return true;
        }
      }
    }
  }
  return false;
}
