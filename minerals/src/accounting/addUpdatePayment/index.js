/* eslint-disable max-len */
/* eslint-disable class-methods-use-this */
/* eslint-disable no-undef */
import initState from './initState';
import AjaxService from '../../services/ajaxService';
import initCardHeader from '../initCardHeader';
import MonthPickerMgr from './monthPickerMgr';
import DatePickerMgr from './datePickerMgr';
import GetDataMgr from './getDataMgr';
import AdjustmentMgr from './adjustmentMgr';
import CheckNumberMgr from './checkNumberMgr';
import WellTractInformationMgr from './wellTractInformationMgr';
import elementsToBeValidated from './elementsToBeValidated';
import formatReadOnlyNumberInputs from '../../services/formatReadOnlyNumberInputs';
import getShowGasOilInd from './getShowGasOilInd';
import getShowLiqVolumeInd from './getShowLiqVolumeInd';
import * as constants from '../../constants';

const template = require('../../views/accounting/addUpdatePayment/index.handlebars');

export default class AddUpdatePaymentMgr {
  constructor(sideBarMgr) {
    this.container = $('#addUpdatePaymentContainer');
    this.sideBarMgr = sideBarMgr;
    this.initSideBarButtonClick();
    this.initScrollToBottom();
    this.initScrollToTop();
    this.managePaymentsMgr = null;
    this.monthPickerMgr = null;
    this.checkNumberMgr = null;
    this.datePickerMgr = null;
    this.wellTractInformationMgr = null;
    this.getDataMgr = null;
    this.adjustmentMgr = null;
    this.id = 0;
  }

  setState(state) {
    this.state = state;
  }

  init(id) {
    this.id = id || 0;
    this.state = initState();
    this.setState({ ...this.state, editmode: !id });
    this.initEditBtnClick();
    this.initCloneBtnClick();
    this.initCancelChangesClick();
    this.initAddNewPaymentButtonClick();
    this.initMgrs();
    this.getDataMgr.getData(id);
  }

  initMgrs() {
    this.monthPickerMgr = new MonthPickerMgr(this);
    this.checkNumberMgr = new CheckNumberMgr(this);
    this.datePickerMgr = new DatePickerMgr(this);
    this.wellTractInformationMgr = new WellTractInformationMgr(this);
    this.getDataMgr = new GetDataMgr(this);
    this.adjustmentMgr = new AdjustmentMgr(this);
  }

  initSideBarButtonClick() {
    $('.sidebar').on('click', '.spa-btn', () => {
      this.init(0);
    });
  }

    render(warnings) {
    const {
      editmode, payment, check, paymentTypes, wellTractInformation, oilInd, adjustments,
      adjustmentInd, showSaveAsAdjustmentButtonInd, productTypes, lessee,
    } = this.state;
    const handlebarData = {
      headerInfo: initCardHeader(),
      adjustments,
      adjustmentInd,
      lessee,
      showSaveAsAdjustmentButtonInd,
      warnings,
      editmode,
      payment,
      check,
      oilInd,
      barrelsInd: payment.liqMeasurement === constants.LiqMeasurementBarrels,
      wellTractInformation,
      showChangeInput: editmode && payment.id && adjustmentInd,
      showLesseeNameFromCheckInd: lessee.lesseeName !== check.lesseeName,
      paymentTypes: paymentTypes.map(
        (paymentType) => (
          {
            ...paymentType,
            selected: paymentType.id === payment.paymentTypeId,
          }
        ),
      ),
      productTypes: productTypes.map(
        (productType) => (
          {
            ...productType,
            selected: productType.id === payment.productTypeId,
          }
        ),
      ),
      showGasOilInd: getShowGasOilInd(paymentTypes, payment),
      showLiqVolumeInd: getShowLiqVolumeInd(paymentTypes, payment),
      duplicatePaymentId: null,
    };

    if (!payment.id && check.lesseeId && payment.postMonth
      && payment.postYear && payment.paymentTypeId && wellTractInformation.wellId
    ) {
      const url = `./api/PaymentMgrApi/CheckProdMonth/
        ${check.lesseeId}/${payment.postMonth}/${payment.postYear}/${payment.paymentTypeId}/${wellTractInformation.wellId}`;
      AjaxService.ajaxGet(url)
        .then((d) => {
          if (d && d.id) {
            handlebarData.duplicatePaymentId = d.id;
          }
          const t = template(handlebarData);
          this.container.empty().append(t);
          this.initAfterRender(adjustmentInd, editmode, adjustments);
        });
    } else {
      const t = template(handlebarData);
      this.container.empty().append(t);
      this.initAfterRender(adjustmentInd, editmode, adjustments);
    }
  }

  initAfterRender(adjustmentInd, editmode, adjustments) {
    this.datePickerMgr.init('entryDate', 'mm/dd/yy');
    if (adjustmentInd && editmode) {
      this.datePickerMgr.init('adjustmentEntryDate', 'mm/dd/yy');
    }
    this.checkNumberMgr.init();
    this.checkNumberMgr.initCheckNumChange();
    this.monthPickerMgr.init();
    this.initFormSubmit();
    this.initWellInfoModalShow();
    this.initSaveDuplicatePaymentBtnClick();
    this.initSaveAsAdjustmentBtnClick();
    this.initgasOilToggle();
    this.initFlaringToggle();
    this.initAdjustmentToggle();
    this.initUnitToggle();
    this.initPaymentInputChange();
    this.initPaymentTypeChange();
    if (adjustments && adjustments.length) {
      this.adjustmentMgr.render();
    }
    formatReadOnlyNumberInputs('addUpdatePaymentContainer');
  }


  initPaymentTypeChange() {
    $('#paymentType').change((e) => {
      this.setState({ ...this.state, payment: { ...this.state.payment, paymentTypeId: parseInt($(e.target).val(), 10) } });
      this.render();
    });
  }

  initAdjustmentToggle() {
    $('#adjustmentIndToggle').bootstrapToggle({
      on: 'Adjustment',
      off: 'Correction',
      onstyle: 'secondary',
      offstyle: 'secondary',
    });
    $('#adjustmentIndToggle').change((e) => {
      this.setState({ ...this.state, adjustmentInd: $(e.target).prop('checked') });
      this.render();
    });
  }

  initUnitToggle() {
    $('#unitToggle').bootstrapToggle({
      on: 'Barrels',
      off: 'Gallons',
      onstyle: 'secondary',
      offstyle: 'secondary',
    });
    $('#unitToggle').change((e) => {
      this.setState({
        ...this.state,
        payment: {
          ...this.state.payment,
          liqMeasurement: $(e.target).prop('checked') ? constants.LiqMeasurementBarrels : constants.LiqMeasurementGallons,
        },
      });
      this.render();
    });
  }

  initPaymentInputChange() {
    $('body').on('change', '.paymentinput', (e) => {
      const { payment, originalPayment } = this.state;
      const element = $(e.target).attr('id');
      payment[element] = $(e.target).val();
      this.setState({ ...this.state, payment });
      const elementObject = elementsToBeValidated.find((x) => x.element === element);
      if (elementObject) {
        this.doesElementHaveError(element);
        if (elementObject.hasChange) {
          const change = parseFloat((parseFloat($(e.target).val() || 0) - originalPayment[element] || 0).toFixed(elementObject.precision));
          $(`#${element}Change`).val(change);
          payment[`${element}Change`] = change;
          this.setState({ ...this.state, payment });
          return true;
        }
      }
      const changeElementObject = elementsToBeValidated.find((x) => x.element === element.replace('Change', ''));
      if (changeElementObject && changeElementObject.hasChange) {
        const change = parseFloat((parseFloat($(e.target).val() || 0) + originalPayment[changeElementObject.element] || 0).toFixed(changeElementObject.precision));
        $(`#${changeElementObject.element}`).val(change);
        payment[changeElementObject.element] = change;
        this.setState({ ...this.state, payment });
      }
    });
  }

  initWellInfoModalShow() {
    $('#wellOwnershipModalBtn').click(() => {
      this.wellTractInformationMgr.render();
    });
  }

  initgasOilToggle() {
    $('#gasOilToggle').bootstrapToggle({
      on: 'Oil',
      off: 'Gas',
      onstyle: 'secondary',
      offstyle: 'secondary',
    });
    $('#gasOilToggle').change((e) => {
      this.setState({ ...this.state, oilInd: $(e.target).prop('checked') });
      this.render();
    });
  }

  initFlaringToggle() {
    $('#flaringToggle').bootstrapToggle({
      on: 'Yes',
      off: 'No',
      onstyle: 'secondary',
      offstyle: 'secondary',
    });
    $('#flaringToggle').change((e) => {
      this.setState({ ...this.state, payment: { ...this.state.payment, flaring: $(e.target).prop('checked') } });
    });
  }

  addManagePaymentsMgr(managePaymentsMgr) {
    this.managePaymentsMgr = managePaymentsMgr;
  }

  initScrollToBottom() {
    this.container.on('click', '.scrollToBottomBtn', () => {
      $('html, body').animate({ scrollTop: $(document).height() }, 'slow');
    });
  }

  initScrollToTop() {
    this.container.on('click', '.scrollToTopBtn', () => {
      $('html, body').animate({ scrollTop: 0 }, 'slow');
    });
  }

  initAddNewPaymentButtonClick() {
    this.container.on('click', '.addNewPayment', () => {
      this.init(0);
    });
  }

  initEditBtnClick() {
    this.container.on('click', '.editPaymentBtn', () => {
      this.setState({ ...this.state, editmode: true });
      this.render();
    });
  }

  initCloneBtnClick() {
    this.container.on('click', '.clonePaymentBtn', () => {
      this.id = 0;
      this.setState({ ...this.state, editmode: true, payment: { ...this.state.payment, id: 0 } });
      this.render();
    });
  }

  initCancelChangesClick() {
    this.container.on('click', '#cancelPaymentChangesBtn', () => {
      this.init(this.id);
    });
  }

  getWarnings() {
    const warnings = [];
    elementsToBeValidated.filter((x) => x.showWarning).forEach((item) => {
      if ($(`#${item.element}`).length) {
        const value = $(`#${item.element}`).val();
        if (!value || !parseFloat(value)) {
          warnings.push(item.warningMessage);
        }
      }
    });
    return warnings;
  }

  doesElementHaveError(element) {
    const { submitInd } = this.state;
    const { showOnlyIfFormSubmitted } = elementsToBeValidated.find((x) => x.element === element);
    if (!$(`#${element}`).length) {
      return false;
    }

    $(`#${element}Error`).hide();
    $(`#${element}`).removeClass('is-invalid');
    if ($(`#${element}`).data('select2')) {
      $($(`#${element}`).data('select2').$container).removeClass('is-invalid');
    }

    if (showOnlyIfFormSubmitted && !submitInd) {
      return false;
    }
    $('#prodMonth').attr('readonly', false);
    $('#adjustmentEntryDate').attr('readonly', false);
    $('#wellTractInformationWellId').attr('readonly', false);
    $('#wellTractInformationWellId').attr('disabled', false);
    if (!$(`#${element}`)[0].checkValidity()) {
      $(`#${element}`).addClass('is-invalid');
      if ($(`#${element}`).data('select2')) {
        $($(`#${element}`).data('select2').$container).addClass('is-invalid');
      }
      $(`#${element}Error`).show();
      $('#prodMonth').attr('readonly', true);
      $('#wellTractInformationWellId').attr('readonly', true);
      $('#wellTractInformationWellId').attr('disabled', true);
      return true;
    }
    $('#prodMonth').attr('readonly', true);
    $('#adjustmentEntryDate').attr('readonly', true);
    $('#wellTractInformationWellId').attr('readonly', true);
    $('#wellTractInformationWellId').attr('disabled', true);
    return false;
  }

  initSaveDuplicatePaymentBtnClick() {
    $('#saveDuplicatePaymentBtn').click(() => {
      this.setState({ ...this.state, saveWithWarning: true });
      $('#addUpdatePaymentForm').trigger('submit');
    });
  }

  initSaveAsAdjustmentBtnClick() {
    $('#saveAsAdjustmentBtn').click(() => {
      this.setState({ ...this.state, saveNewPaymentAsAdjustmentInd: true });
      $('#addUpdatePaymentForm').trigger('submit');
    });
  }

  initFormSubmit() {
    // eslint-disable-next-line consistent-return
    $('#addUpdatePaymentForm').submit(() => {
      const {
        payment, saveWithWarning, wellTractInformation, check, oilInd, adjustmentInd, saveNewPaymentAsAdjustmentInd,
      } = this.state;

      this.setState({
        ...this.state, submitInd: true, saveWithWarning: false, showSaveAsAdjustmentButtonInd: false,
      });
      let formHasError = false;

      // eslint-disable-next-line no-restricted-syntax
      for (const item of elementsToBeValidated) {
        if (this.doesElementHaveError(item.element)) {
          formHasError = true;
        }
      }
      if (formHasError) {
        window.Toastr.error('Please fix errors and resubmit');
        return false;
      }

      const newPayment = {
        ...payment,
        nri: payment.nri ? parseFloat(payment.nri) : null,
        oilProd: payment.oilProd && oilInd ? parseFloat(payment.oilProd) : 0,
        gasProd: payment.gasProd ? parseFloat(payment.gasProd) : 0,
        salesPrice: payment.salesPrice ? parseFloat(payment.salesPrice) : 0,
        oilRoyalty: payment.oilRoyalty && oilInd ? parseFloat(payment.oilRoyalty) : 0,
        gasRoyalty: payment.gasRoyalty ? parseFloat(payment.gasRoyalty) : 0,
        deduction: payment.deduction ? parseFloat(payment.deduction) : null,
        transDeduction: payment.transDeduction ? parseFloat(payment.transDeduction) : null,
        compressDeduction: payment.compressDeduction ? parseFloat(payment.compressDeduction) : null,
        liqVolume: payment.liqVolume ? parseFloat(payment.liqVolume) : null,
        liqPayment: payment.liqPayment ? parseFloat(payment.liqPayment) : null,
        productTypeId: payment.productTypeId ? parseInt(payment.productTypeId, 10) : null,
        adjustmentInd,
        saveNewPaymentAsAdjustmentInd,
        lesseeId: check.lesseeId,
        wellId: wellTractInformation.wellId,
      };
      const warnings = this.getWarnings();
      if (saveWithWarning || saveNewPaymentAsAdjustmentInd) {
        this.postPayment(newPayment);
      } else {
        const url = `./api/PaymentMgrApi/CheckProdMonth/
        ${check.lesseeId}/${payment.postMonth}/${payment.postYear}/${payment.paymentTypeId}/${wellTractInformation.wellId}`;
        AjaxService.ajaxGet(url)
          .then((d) => {
            if (d && d.id && !this.id) {
              warnings.push('There is already a production month for this well and company');
              this.setState({ ...this.state, showSaveAsAdjustmentButtonInd: true });
            }
            if (warnings.length) {
              this.render(warnings);
              window.$('#paymentWarningModal').modal('show');
              return false;
            }
            this.postPayment(newPayment);
          });
      }
    });
  }

  postPayment(newPayment) {
    $('.paymentSubmitbtn').hide();
    $('.paymentSubmitBtnDisabled').show();
    AjaxService.ajaxPost('./api/PaymentMgrApi', newPayment)
      .then((d) => {
        window.Toastr.success('Your record has been saved', 'Save Successful', { positionClass: 'toast-top-center' });
        this.managePaymentsMgr.init();
        this.init(d.id);
      })
      .catch(() => {
        $('.paymentSubmitbtn').show();
        $('.paymentSubmitBtnDisabled').hide();
      });
  }
}
