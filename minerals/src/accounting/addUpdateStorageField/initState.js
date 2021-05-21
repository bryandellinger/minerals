
export default function initState() {
  return {
    editmode: true,
    storageRental: { id: 0, storageRentalEntryDate: new Date() },
    submitInd: false,
    check: {},
    lessee: {},
    contract: {},
    contracts: [],
    months: [],
    periodTypes: [],
    storageRentalPaymentTypes: [],
    storageWellPaymentMonths: [],
    storageBaseRentalPaymentMonths: [],
    storageRentalPaymentMonths: [],
  };
}
