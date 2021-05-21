
export default function initState() {
  return {
    editmode: true,
    unit: { id: 0 },
    submitInd: false,
    tracts: [],
    tractUnitJunctions: [],
    wells: [],
    wellId: 0,
    paymentId: 0,
    unitGroups: [],
    amendmentInd: true,
    files: [],
    wellStatuses: [],
  };
}
