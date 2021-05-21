export default function initState() {
  return {
    lessee: {},
    lessees: [],
    lesseeContacts: [],
    lesseeContact: { id: 0 },
    contactEditMode: true,
    historyFormEditMode: false,
    states: [],
    contactSubmitInd: false,
    legalNameChangeInd: true,
    addCloseButton: false,
  };
}
