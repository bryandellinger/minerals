
export default function initState() {
  return {
    editmode: true,
    submitInd: false,
    surety: { id: 0, suretyStatus: 'Active' },
    suretyTypes: [],
    bondCategories: [],
    lessees: [],
    insurers: [],
    suretyStatuses: ['Active', 'Released', 'Replaced'],
    suretyRiders: [],
    riderReasons: [],
    suretyWells: [],
    wells: [],
    contracts: [],
  };
}
