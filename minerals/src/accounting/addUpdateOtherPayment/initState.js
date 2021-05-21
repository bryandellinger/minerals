
export default function initState() {
  return {
    editmode: true,
    otherPayment: { id: 0, otherRentalEntryDate: new Date() },
    submitInd: false,
    check: {},
    lessee: {},
    otherPaymentTypes: [],
  };
}
