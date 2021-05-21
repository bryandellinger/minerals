
export default function initState() {
  return {
    editmode: true,
    contractRental: { id: 0, contractRentalEntryDate: new Date() },
    submitInd: false,
    check: {},
    lessee: {},
    contract: {},
    contracts: [],
    months: [],
    contractRentalPaymentMonths: [],
  };
}
