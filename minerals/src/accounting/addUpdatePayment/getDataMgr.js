import * as constants from '../../constants';

export default class getDataMgr {
  constructor(addUpdatePaymentMgr) {
    this.addUpdatePaymentMgr = addUpdatePaymentMgr;
  }


  getData(id) {
    $.when(
      $.get(`./api/PaymentMgrApi/${id}`, (payment) => {
        if (payment && payment.id) {
          this.addUpdatePaymentMgr.setState({
            ...this.addUpdatePaymentMgr.state,
            oilInd: payment.oilProd,
            payment: {
              ...payment,
              entryDate: payment.entryDate ? new Date(payment.entryDate) : null,
              adjustmentEntryDate: null,
              gasProdChange: 0,
              oilProdChange: 0,
              gasRoyaltyChange: 0,
              oilRoyaltyChange: 0,
              nriChange: 0,
              salesPriceChange: 0,
              deductionChange: 0,
              transDeductionChange: 0,
              compressDeductionChange: 0,
              liqVolumeChange: 0,
              liqPaymentChange: 0,
            },
            originalPayment: { ...payment },
          });
        }
      }),
      $.get(`./api/checkMgrApi/getByPayment/${id}`, (check) => {
        if (check && check.id) {
          this.addUpdatePaymentMgr.setState({
            ...this.addUpdatePaymentMgr.state,
            check: {
              ...check,
              checkDate: check.checkDate ? new Date(check.checkDate).toLocaleDateString() : null,
              receivedDate:
               check.receivedDate ? new Date(check.receivedDate).toLocaleDateString() : null,
            },
          });
        }
      }),
        $.get(`./api/lesseeMgrApi/getByPayment/${id}`, (lessee) => {
            this.addUpdatePaymentMgr.setState({
                ...this.addUpdatePaymentMgr.state, lessee,
            });
       }),
      $.get('./api/PaymentTypeMgrApi', (paymentTypes) => {
        this.addUpdatePaymentMgr.setState({
          ...this.addUpdatePaymentMgr.state, paymentTypes,
        });
      }),
      $.get('./api/ProductTypeMgrApi', (productTypes) => {
        this.addUpdatePaymentMgr.setState({
          ...this.addUpdatePaymentMgr.state, productTypes,
        });
      }),
      $.get(`./api/WellTractInformationMgrApi/welltractinfobyroyalty/${id}`, (wellTractInformation) => {
        this.addUpdatePaymentMgr.setState({
          ...this.addUpdatePaymentMgr.state, wellTractInformation,
        });
      }),
      $.get(`./api/PaymentAdjustmentMgrApi/adjustmentsbypayment/${id}`, (d) => {
        if (d && d.length) {
          this.addUpdatePaymentMgr.setState({
            ...this.addUpdatePaymentMgr.state,
            adjustments: d.map(
              (adjustment) => (
                {
                  ...adjustment,
                  entryDate: adjustment.entryDate ? new Date(adjustment.entryDate) : null,
                  legacyDataInd: !adjustment.lastUpdateDate,
                }
              ),
            ),
          });
        }
      }),
    ).then(() => {
      const {
        payment, paymentTypes, productTypes,
      } = this.addUpdatePaymentMgr.state;
      if (!payment.paymentTypeId) {
        const oilGasPaymentType = paymentTypes.find(
          (x) => x.paymentTypeName === constants.PaymentTypeOilAndGas,
        );
        payment.paymentTypeId = oilGasPaymentType.id;
        this.addUpdatePaymentMgr.setState({
          ...this.addUpdatePaymentMgr.state, payment,
        });
      }
      if (!payment.productTypeId) {
        const nglProductType = productTypes.find(
          (x) => x.productTypeName === constants.ProductTypeNGL,
        );
        payment.productTypeId = nglProductType.id;
        this.addUpdatePaymentMgr.setState({
          ...this.addUpdatePaymentMgr.state, payment,
        });
      }
      if (!payment.liqMeasuremnt) {
        this.addUpdatePaymentMgr.setState({
          ...this.addUpdatePaymentMgr.state,
          payment: {
            ...this.addUpdatePaymentMgr.state.payment,
            liqMeasurment: constants.LiqMeasurementGallons,
          },
        });
      }
      this.addUpdatePaymentMgr.render();
      $('.spinner').hide();
    });
  }
}
