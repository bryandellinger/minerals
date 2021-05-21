
export default function initState() {
  return {
    editmode: true,
    check: { id: 0 },
    submitInd: false,
    lessees: [],
    files: [],
    payments: [],
    adjustments: [],
    storageRentals: [],
    contractRentals: [],
    otherPayments: [],
    saveWithWarning: false,
  };
}
