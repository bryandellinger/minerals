/* eslint-disable no-nested-ternary */
/* eslint-disable max-len */
/* eslint-disable no-undef */
// eslint-disable-next-line no-unused-vars
import DropZone from 'dropzone';
import DropzoneMgr from './dropzoneMgr';
import DatePickerMgr from './datePickerMgr';
import CsvPaymentMgr from './csvPaymentMgr';
import CheckNumberMgr from './checkNumberMgr';
import initState from './initState';
import initCardHeader from '../initCardHeader';
import formatReadOnlyNumberInputs from '../../services/formatReadOnlyNumberInputs';
import elementsToBeValidated from './elementsToBeValidated';
import AjaxService from '../../services/ajaxService';

const template = require('../../views/accounting/addUpdateUploadPayment/index.handlebars');

export default class AddUpdateUploadPaymentMgr {
  constructor(sideBarMgr) {
    this.container = $('#addUpdateUploadPaymentContainer');
    this.sideBarMgr = sideBarMgr;
    this.initSideBarButtonClick();
    this.initScrollToBottom();
    this.initScrollToTop();
    this.id = 0;
    this.datePickerMgr = null;
    this.checkNumberMgr = null;
    this.manageUploadPaymentsMgr = null;
    this.csvPaymentMgr = null;
    this.dropzoneMgr = null;
  }

  setState(state) {
    this.state = state;
  }

  init(id) {
    this.id = id || 0;
    this.state = initState();
    this.setState({ ...this.state, editmode: !id });
    this.initEditBtnClick();
    this.initCancelChangesClick();
    this.initAddNewUploadPaymentButtonClick();
    this.initMgrs();
    this.getData(id);
  }

  initMgrs() {
    this.dropzoneMgr = new DropzoneMgr(this);
    this.datePickerMgr = new DatePickerMgr(this);
    this.checkNumberMgr = new CheckNumberMgr(this);
    this.csvPaymentMgr = new CsvPaymentMgr(this);
  }


  addManageUploadPaymentsMgr(manageUploadPaymentsMgr) {
    this.manageUploadPaymentsMgr = manageUploadPaymentsMgr;
  }

  getData(id) {
    $('.spinner').show();
    $.when(
      $.get(`./api/UploadPaymentMgrApi/${id}`, (uploadPayment) => {
        if (uploadPayment && uploadPayment.id) {
          this.setState({
            ...this.state,
            uploadPayment: {
              ...uploadPayment,
              uploadPaymentEntryDate: uploadPayment.uploadPaymentEntryDate
                ? new Date(uploadPayment.uploadPaymentEntryDate) : null,
            },
          });
        }
      }),
      $.get('./api/UploadTemplateMgrApi', (uploadTemplates) => {
        this.setState({
          ...this.state, uploadTemplates,
        });
      }),
      $.get('./api/WellMgrApi', (wells) => {
        this.setState({
          ...this.state, wells,
        });
      }),
      $.get(`./api/FileMgrApi/filesbyuploadpayment/${id}`, (d) => {
        this.setState(
          { ...this.state, files: d || [] },
        );
      }),
      $.get(`./api/CheckMgrApi/GetByUploadPayment/${id}`, (check) => {
        if (check && check.id) {
          this.setState({
            ...this.state,
            check: {
              ...check,
              checkDate: check.checkDate ? new Date(check.checkDate).toLocaleDateString() : null,
              receivedDate: check.receivedDate ? new Date(check.receivedDate).toLocaleDateString() : null,
            },
          });
        }
      }),
      $.get(`./api/LesseeMgrApi/GetByUploadPayment/${id}`, (lessee) => {
        if (lessee) {
          this.setState({ ...this.state, lessee });
        }
      }),
      $.get(`./api/UploadPaymentMgrApi/GetCSVPayments/${id}`, (csvPayments) => {
        if (csvPayments && csvPayments.length) {
          this.setState({ ...this.state, csvPayments });
        }
      }),
    ).then(() => {
      this.render();
      $('.spinner').hide();
    });
  }

  initSideBarButtonClick() {
    $('.sidebar').on('click', '.spa-btn', () => {
      this.init(0);
    });
  }

  render() {
    const {
      editmode, uploadPayment, uploadTemplates,
      check, lessee, showGenerateCheckInd, checkNoFromCSV,
    } = this.state;
    const handlebarData = {
      headerInfo: initCardHeader(),
      editmode,
      uploadPayment,
      check,
      lessee,
      showGenerateCheckInd,
      checkNoFromCSV,
      uploadTemplates: uploadTemplates.map(
        (uploadTemplate) => (
          {
            ...uploadTemplate,
            selected: uploadTemplate.id === uploadPayment.uploadTemplateId,
          }
        ),
      ),
    };
    const t = template(handlebarData);
    this.container.empty().append(t);
    this.dropzoneMgr.init();
    $('#uploadTemplateId').select2({ placeholder: 'Select a template', width: '100%' });
    this.datePickerMgr.init('uploadPaymentEntryDate', 'mm/dd/yy');
    this.checkNumberMgr.init();
    this.csvPaymentMgr.init();
    this.checkNumberMgr.initCheckNumChange();
    this.checkNumberMgr.initCreateCheckBtnClick();
    this.initUploadPaymentInputChange();
    this.initFormSubmit();
    formatReadOnlyNumberInputs('addUpdateUploadPaymentContainer');
  }

  initUploadPaymentInputChange() {
    $('.uploadPaymentInput').change((e) => {
      const { uploadPayment } = this.state;
      const element = $(e.target).attr('name');
      uploadPayment[element] = $(e.target).val();
      this.setState({
        ...this.state,
        uploadPayment: {
          ...uploadPayment,
          uploadTemplateId: parseInt(uploadPayment.uploadTemplateId, 10),
        },
      });
      if (elementsToBeValidated.find((x) => x.element === element)) {
        this.doesElementHaveError(element);
      }
      if (element === 'uploadTemplateId') {
        this.render();
      }
    });
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

    if (!$(`#${element}`)[0].checkValidity()) {
      $(`#${element}`).addClass('is-invalid');
      if ($(`#${element}`).data('select2')) {
        $($(`#${element}`).data('select2').$container).addClass('is-invalid');
      }
      $(`#${element}Error`).show();
      return true;
    }
    return false;
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

  initAddNewUploadPaymentButtonClick() {
    this.container.on('click', '.addNewUploadPayment', () => {
      this.init(0);
    });
  }

  initEditBtnClick() {
    this.container.on('click', '.editUploadPaymentBtn', () => {
      this.setState({ ...this.state, editmode: true });
      this.render();
    });
  }

  initCancelChangesClick() {
    this.container.on('click', '#cancelUploadPaymentChangesBtn', () => {
      this.init(this.id);
    });
  }

  initFormSubmit() {
    $('#addUpdateUploadPaymentForm').submit(() => {
      this.submitForm();
    });
  }

  // eslint-disable-next-line consistent-return
  submitForm() {
    const { uploadPayment, files, csvPayments } = this.state;

    this.setState({ ...this.state, submitInd: true });
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

    if (!(files.length)) {
      window.Toastr.error('Please upload the csv payment file');
      return false;
    }

    const newUploadPayment = {
      ...uploadPayment,
      uploadTemplateId: parseInt(uploadPayment.uploadTemplateId, 10),
      checkId: uploadPayment ? parseInt(uploadPayment.checkId, 10) : null,
      files,
      csvPayments,
    };
    $('.uploadPaymentSubmitbtn').hide();
    $('.uploadPaymentSubmitBtnDisabled').show();
    AjaxService.ajaxPost('./api/UploadPaymentMgrApi', newUploadPayment)
      .then((d) => {
        $('.spinner').hide();
        window.Toastr.success('Your payment has been saved', 'Save Successful', { positionClass: 'toast-top-center' });
        this.manageUploadPaymentsMgr.init();
        this.init(d.id);
      })
      .catch(() => {
        $('.uploadPaymentSubmitbtn').show();
        $('.uploadPaymentSubmitBtnDisabled').hide();
        $('.spinner').hide();
      });
  }
}
