
export default function initState() {
  return {
    editmode: true,
    adjustmentEditMode: false,
      payment: { id: 0, entryDate: new Date() },
    originalPayment: {},
    submitInd: false,
    check: {},
    paymentTypes: [],
    productTypes: [],
    saveWithWarning: false,
    wellTractInformation: {},
    oilInd: false,
    adjustments: [],
    adjustmentInd: true,
    showSaveAsAdjustmentButtonInd: false,
    saveNewPaymentAsAdjustmentInd: false,
    lessee: {},
  };
}
