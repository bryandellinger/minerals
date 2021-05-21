
export default function initState() {
  return {
    editmode: true,
    uploadPayment: { id: 0, uploadPaymentEntryDate: new Date() },
    submitInd: false,
    files: [],
    uploadTemplates: [],
    checkDate: new Date(),
    checkNoFromCSV: null,
    showGenerateCheckInd: false,
    csvPayments: [],
    csvPayment: {},
    wells: [],
  };
}
