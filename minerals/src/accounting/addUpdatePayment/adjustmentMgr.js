/* eslint-disable no-param-reassign */
/* eslint-disable class-methods-use-this */
/* eslint-disable no-nested-ternary */
import initAdjustmentDataTable from './initAdjustmentDataTable';
import formatReadOnlyNumberInputs from '../../services/formatReadOnlyNumberInputs';
import numberWithCommas from '../../services/numberWithCommas';
import getShowGasOilInd from './getShowGasOilInd';
import getShowLiqVolumeInd from './getShowLiqVolumeInd';
import AjaxService from '../../services/ajaxService';

const template = require('../../views/accounting/addUpdatePayment/adjustmentDetails.handlebars');

export default class adjustmentMgr {
  constructor(addUpdatePaymentMgr) {
    this.addUpdatePaymentMgr = addUpdatePaymentMgr;
    this.table = null;
    this.container = '#adjustmentContainer';
    this.expandedAdjustmentInformation = null;
  }

  render() {
    const {
      adjustments, oilInd, paymentTypes, payment,
    } = this.addUpdatePaymentMgr.state;
    if (this.table) {
      this.table.destroy();
    }
    const adjustments1 = adjustments.map(
      (adjustment) => (
        {
          ...adjustment,
          volume: getShowLiqVolumeInd(paymentTypes, payment) ? adjustment.liqVolume || 0 : oilInd ? adjustment.oilProd : adjustment.gasProd,
          payment: getShowLiqVolumeInd(paymentTypes, payment) ? adjustment.liqPayment || 0 : oilInd ? adjustment.oilRoyalty : adjustment.gasRoyalty,
          paymentVolume: getShowLiqVolumeInd(paymentTypes, payment) ? payment.liqVolume || 0 : oilInd ? payment.oilProd : payment.gasProd,
          paymentPayment: getShowLiqVolumeInd(paymentTypes, payment) ? payment.liqPayment || 0 : oilInd ? payment.oilRoyalty : payment.gasRoyalty,
          paymentNri: payment.nri,
          paymentSalesPrice: payment.salesPrice,
          paymentDeduction: payment.deduction,
          paymentTransDeduction: payment.transDeduction,
          paymentCompressDeduction: payment.compressDeduction,
        }
      ),
    );

    this.table = initAdjustmentDataTable(
      adjustments1.map(
        (adjustment, index) => (
          {
            ...adjustment,
            volumeWithChange: this.getChange(adjustments1, index, 'volume', 'paymentVolume', 2),
            paymentWithChange: this.getChange(adjustments1, index, 'payment', 'paymentPayment', 2),
            nriWithChange: this.getChange(adjustments1, index, 'nri', 'paymentNri', 9),
            salesPriceWithChange: this.getChange(adjustments1, index, 'salesPrice', 'paymentSalesPrice', 4),
            deductionWithChange: this.getChange(adjustments1, index, 'deduction', 'paymentDeduction', 2),
            transDeductionWithChange: this.getChange(adjustments1, index, 'transDeduction', 'paymentTransDeduction', 2),
            compressDeductionWithChange: this.getChange(adjustments1, index, 'compressDeduction', 'paymentCompressDeduction', 2),
          }
        ),
      ),
    );
    this.initSelectClick();
  }

  getChange(adjustments, index, adjustmentElement, paymentElement, precision) {
    const adjustmentValue = adjustments[index][adjustmentElement] || 0;
    const paymentValue = adjustments[index][paymentElement] || 0;
    if (adjustments[index].legacyDataInd) {
      return numberWithCommas(adjustmentValue.toFixed(precision));
    }
    if (adjustments[index - 1]) {
      const prevAdjustmentValue = adjustments[index - 1][adjustmentElement] || 0;
      return `${numberWithCommas(adjustmentValue.toFixed(precision))} (${numberWithCommas((prevAdjustmentValue - adjustmentValue).toFixed(precision))}) `;
    }
    return `${numberWithCommas(adjustmentValue.toFixed(precision))} (${numberWithCommas((paymentValue - adjustmentValue).toFixed(precision))}) `;
  }

  initSelectClick() {
    $('#adjustmentsTable tbody').on('click', 'button', (event) => {
      const data = this.table.row($(event.target).parents('tr')).data();
      this.addUpdatePaymentMgr.setState({ ...this.addUpdatePaymentMgr.state, adjustmentEditMode: false });
      this.expandedAdjustmentInformation = { ...data };
      this.renderDetails(data.id);
    });
  }

  renderDetails(id) {
    const {
      adjustments, oilInd, payment, paymentTypes, adjustmentEditMode,
    } = this.addUpdatePaymentMgr.state;
    const adjustment = adjustments.find((x) => x.id === id);
    const handlebarData = {
      adjustmentEditMode,
      expandedAdjustmentInformation: this.expandedAdjustmentInformation,
      payment: {
        ...payment,
        currentVolume: numberWithCommas(
          (
            getShowLiqVolumeInd(paymentTypes, payment) ? payment.liqVolume : oilInd ? payment.oilProd : payment.gasProd || 0
          ).toFixed(2),
        ),
        currentPayment: numberWithCommas(
          (
            getShowLiqVolumeInd(paymentTypes, payment) ? payment.liqPayment : oilInd ? payment.oilRoyalty : payment.gasRoyalty || 0
          ).toFixed(2),
        ),
        currentNri: numberWithCommas((payment.nri || 0).toFixed(9)),
        currentSalesPrice: numberWithCommas((payment.salesPrice || 0).toFixed(4)),
        currentDeduction: numberWithCommas((payment.deduction || 0).toFixed(2)),
        currentTransDeduction: numberWithCommas((payment.transDeduction || 0).toFixed(2)),
        currentCompressDeduction: numberWithCommas((payment.compressDeduction || 0).toFixed(2)),
        currentLiqVolume: numberWithCommas((payment.liqVolume || 0).toFixed(2)),
      },
      adjustment: {
        ...adjustment,
        entryDate: adjustment.entryDate.toLocaleDateString(),
        checkDate: adjustment.checkDate ? new Date(adjustment.checkDate).toLocaleDateString() : null,
        receivedDate: adjustment.receivedDate ? new Date(adjustment.receivedDate).toLocaleDateString() : null,
        volume: oilInd ? adjustment.oilProd : adjustment.gasProd,
        payment: getShowLiqVolumeInd(paymentTypes, payment) ? adjustment.liqPayment : oilInd ? adjustment.oilRoyalty : adjustment.gasRoyalty,
      },
      showGasOilInd: getShowGasOilInd(paymentTypes, payment),
      showLiqVolumeInd: getShowLiqVolumeInd(paymentTypes, payment),
    };
    const t = template(handlebarData);
    $(`${this.container}`).empty().append(t);
    $('html, body').animate({ scrollTop: $(document).height() }, 'slow');
    formatReadOnlyNumberInputs('adjustmentContainer');
    this.initEditBtnClick(id);
    this.initCheckSelect(adjustment);
    this.initCheckNumberChange(adjustment, id);
    this.initVolumeChange(adjustment, oilInd, handlebarData.showLiqVolumeInd);
    this.initPaymentChange(adjustment, oilInd, handlebarData.showLiqVolumeInd);
    this.initAdjustmentInputChange(adjustment);
    this.initFlaringToggle(adjustment);
    this.initFormSubmit(id);
    this.initDeleteAdjustment(id);
  }

  initFlaringToggle(adjustment) {
    $('#adjustmentflaringToggle').bootstrapToggle({
      on: 'Yes',
      off: 'No',
      onstyle: 'secondary',
      offstyle: 'secondary',
    });
    $('#adjustmentflaringToggle').change((e) => {
      adjustment.flaring = $(e.target).prop('checked');
      const {
        adjustments,
      } = this.addUpdatePaymentMgr.state;
      adjustments.map((obj) => (adjustment.id === obj.id ? adjustment : obj));
      this.addUpdatePaymentMgr.setState({ ...this.addUpdatePaymentMgr.state, adjustments });
    });
  }

  initAdjustmentInputChange(adjustment) {
    $('.adjustmentInput').change((e) => {
      const element = $(e.target).attr('id').split('_')[1];
      adjustment[element] = parseFloat($(e.target).val());
      const {
        adjustments,
      } = this.addUpdatePaymentMgr.state;
      adjustments.map((obj) => (adjustment.id === obj.id ? adjustment : obj));
      this.addUpdatePaymentMgr.setState({ ...this.addUpdatePaymentMgr.state, adjustments });
    });
  }

  initPaymentChange(adjustment, oilInd, showLiqVolumeInd) {
    $('#adjustmentPayment').change((e) => {
      if (showLiqVolumeInd) {
        adjustment.liqPayment = parseFloat($(e.target).val());
      } else if (oilInd) {
        adjustment.oilRoyalty = parseFloat($(e.target).val());
      } else {
        adjustment.gasRoyalty = parseFloat($(e.target).val());
      }
      const {
        adjustments,
      } = this.addUpdatePaymentMgr.state;
      adjustments.map((obj) => (adjustment.id === obj.id ? adjustment : obj));
      this.addUpdatePaymentMgr.setState({ ...this.addUpdatePaymentMgr.state, adjustments });
    });
  }

  initVolumeChange(adjustment, oilInd, showLiqVolumeInd) {
    $('#adjustmentVolume').change((e) => {
      if (showLiqVolumeInd) {
        adjustment.liqVolume = parseFloat($(e.target).val());
      } else if (oilInd) {
        adjustment.oilProd = parseFloat($(e.target).val());
      } else {
        adjustment.gasProd = parseFloat($(e.target).val());
      }
      const {
        adjustments,
      } = this.addUpdatePaymentMgr.state;
      adjustments.map((obj) => (adjustment.id === obj.id ? adjustment : obj));
      this.addUpdatePaymentMgr.setState({ ...this.addUpdatePaymentMgr.state, adjustments });
    });
  }

  initEditBtnClick(id) {
    $('.editAdjustmentBtn').click(() => {
      this.addUpdatePaymentMgr.setState({ ...this.addUpdatePaymentMgr.state, adjustmentEditMode: true });
      this.renderDetails(id);
    });
  }

  initCheckNumberChange(adjustment, id) {
    $('#ajaxSelect2Adjustment').on('select2:select', (e) => {
      AjaxService.ajaxGet(`./api/CheckMgrApi/${e.params.data.id}`)
        .then((check) => {
          if (check && check.id) {
            adjustment.checkId = check.id;
            adjustment.checkNum = check.checkNum;
            adjustment.checkDate = check.checkDate;
            adjustment.receivedDate = check.receivedDate;
            adjustment.totalAmount = check.totalAmount;
          }
          const {
            adjustments,
          } = this.addUpdatePaymentMgr.state;
          adjustments.map((obj) => (adjustment.id === obj.id ? adjustment : obj));
          this.addUpdatePaymentMgr.setState({ ...this.addUpdatePaymentMgr.state, adjustments });
          this.renderDetails(id);
        });
    });
  }


  initCheckSelect(adjustment) {
    $('#ajaxSelect2Adjustment').select2({
      placeholder: 'Select a Check Number',
      minimumInputLength: 2,
      ajax: {
        url: './api/CheckMgrApi/GetSelect2Data',
        contentType: 'application/json; charset=utf-8',
        data(params) {
          const query = {
            id: params.term,
          };
          return query;
        },
        processResults: (results) => ({
          results,
        }),
      },
    });
    if (adjustment && adjustment.checkId) {
      const data = {
        id: adjustment.checkId,
        text: adjustment.checkNum,
      };
      if ($('#ajaxSelect2Adjustment').find(`option[value='${data.id}']`).length) {
        $('ajaxSelect2Adjustment').val(data.id).trigger('change');
      } else {
      // Create a DOM Option and pre-select by default
        const newOption = new Option(data.text, data.id, true, true);
        // Append it to the select
        $('#ajaxSelect2Adjustment').append(newOption).trigger('change');
      }
    }
  }

  initFormSubmit(id) {
    // eslint-disable-next-line consistent-return
    $('#updateAdjustmentForm').submit(() => {
      const {
        adjustments, payment,
      } = this.addUpdatePaymentMgr.state;

      const adjustment = adjustments.find((x) => x.id === id);

      $('.adjustmentSubmitbtn').hide();
      $('.adjustmentSubmitBtnDisabled').show();
      AjaxService.ajaxPut(`./api/RoyaltyAdjustmentMgrApi/${id}`, adjustment)
        .then(() => {
          window.Toastr.success('Your adjustment has been updated', 'Save Successful', { positionClass: 'toast-top-center' });
          this.addUpdatePaymentMgr.init(payment.id);
        })
        .catch(() => {
          $('.adjustmentSubmitbtn').show();
          $('.adjustmentSubmitBtnDisabled').hide();
        });
    });
  }

  initDeleteAdjustment(id) {
    $('#deleteAdjustmentBtn').click(() => {
      $('.adjustmentSubmitbtn').hide();
      $('.adjustmentSubmitBtnDisabled').show();
      AjaxService.ajaxDelete(`./api/RoyaltyAdjustmentMgrApi/${id}`)
        .then(() => {
          window.Toastr.success('Your adjustment has been deleted', 'Save Successful', { positionClass: 'toast-top-center' });
          const {
            payment,
          } = this.addUpdatePaymentMgr.state;
          this.addUpdatePaymentMgr.init(payment.id);
        })
        .catch(() => {
          $('.adjustmentSubmitbtn').show();
          $('.adjustmentSubmitBtnDisabled').hide();
        });
    });
  }
}
