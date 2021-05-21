export default function initState() {
  return {
    districts: [],
    districtTractJunctions: [],
    tracts: [],
    tract: {},
    editmode: false,
    tractId: 0,
    submitInd: false,
    tractNum: '',
    addCloseButton: false,
  };
}
