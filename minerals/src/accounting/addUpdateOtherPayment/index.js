/* eslint-disable no-nested-ternary */
/* eslint-disable max-len */
/* eslint-disable no-undef */
import initState from './initState';
import initCardHeader from '../initCardHeader';
import CheckNumberMgr from './checkNumberMgr';
import DatePickerMgr from './datePickerMgr';
import formatReadOnlyNumberInputs from '../../services/formatReadOnlyNumberInputs';
import elementsToBeValidated from './elementsToBeValidated';
import AjaxService from '../../services/ajaxService';

const template = require('../../views/accounting/addUpdateOtherPayment/index.handlebars');

export default class AddUpdateOtherPaymentMgr {
  constructor(sideBarMgr) {
    this.container = $('#addUpdateOtherPaymentContainer');
    this.sideBarMgr = sideBarMgr;
    this.initSideBarButtonClick();
    this.initScrollToBottom();
    this.initScrollToTop();
    this.id = 0;
    this.datePickerMgr = null;
    this.manageOtherPaymentsMgr = null;
    this.checkNumberMgr = null;
    this.datePickerMgr = null;
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
    this.initMgrs();
    this.getData(id);
  }

  initMgrs() {
    this.checkNumberMgr = new CheckNumberMgr(this);
    this.datePickerMgr = new DatePickerMgr(this);
  }


  addManageOtherPaymentsMgr(manageOtherPaymentsMgr) {
    this.manageOtherPaymentsMgr = manageOtherPaymentsMgr;
  }

  getData(id) {
    $('.spinner').show();
    $.when(
      $.get(`./api/OtherPaymentMgrApi/${id}`, (otherPayment) => {
        if (otherPayment && otherPayment.id) {
          this.setState({
            ...this.state,
            otherPayment: {
              ...otherPayment,
                otherRentalEntryDate: otherPayment.otherRentalEntryDate ?
                    new Date(otherPayment.otherRentalEntryDate) : null                
            },
          });
        }
      }),
      $.get(`./api/CheckMgrApi/GetByOtherPayment/${id}`, (check) => {
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
      $.get(`./api/LesseeMgrApi/GetByOtherPayment/${id}`, (lessee) => {
        if (lessee) {
          this.setState({ ...this.state, lessee });
        }
      }),
      $.get('./api/OtherPaymentMgrApi/getOtherPaymentTypes', (otherPaymentTypes) => {
        this.setState({ ...this.state, otherPaymentTypes });
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
      editmode, otherPayment, check, lessee,
    } = this.state;
    const handlebarData = {
      headerInfo: initCardHeader(),
      editmode,
      otherPayment,
      check,
      lessee,
      showLesseeNameFromCheckInd: lessee.lesseeName !== check.lesseeName,
    };
    const t = template(handlebarData);
    this.container.empty().append(t);
    this.checkNumberMgr.init();
    this.checkNumberMgr.initCheckNumChange();
    this.initOtherPaymentTypeAutoComplete();
    this.datePickerMgr.init('otherRentalEntryDate', 'mm/dd/yy');
    this.initOtherPaymentInputChange();
    this.initFormSubmit();
    formatReadOnlyNumberInputs('addUpdateOtherPaymentContainer');
  }

  initOtherPaymentInputChange() {
    $('.otherPaymentInput').change((e) => {
      const { otherPayment } = this.state;
      const element = $(e.target).attr('name');
      otherPayment[element] = $(e.target).val();
      this.setState({ ...this.state, otherPayment });
      if (elementsToBeValidated.find((x) => x.element === element)) {
        this.doesElementHaveError(element);
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

  initOtherPaymentTypeAutoComplete() {
    const { otherPaymentTypes } = this.state;
      $('#otherPaymentType')
      .autocomplete(
        {
          source: otherPaymentTypes,
          minLength: 0,
          select: (event, ui) => {
            this.setState({
              ...this.state,
              otherPayment: {
                ...this.state.otherPayment,
                otherPaymentType: ui.item.label,
              },
            });
          },
        },
      )
    // trigger the search on focus
      // eslint-disable-next-line func-names
      .focus(function () {
        $(this).autocomplete('search', $(this).val());
      });
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

  initAddNewOtherPaymentButtonClick() {
    this.container.on('click', '.addNewOtherPayment', () => {
      this.init(0);
    });
  }

  initEditBtnClick() {
    this.container.on('click', '.editOtherPaymentBtn', () => {
      this.setState({ ...this.state, editmode: true });
      this.render();
    });
  }

  initCancelChangesClick() {
    this.container.on('click', '#cancelOtherPaymentChangesBtn', () => {
      this.init(this.id);
    });
  }

  initFormSubmit() {
    // eslint-disable-next-line consistent-return
    $('#addUpdateOtherPaymentForm').submit(() => {
      const { otherPayment } = this.state;

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

      const newOtherPayment = {
        ...otherPayment,
        otherRentPay: parseFloat(otherPayment.otherRentPay),
        checkId: parseInt(otherPayment.checkId, 10),
      };

      $('.otherPaymentSubmitbtn').hide();
      $('.otherPaymentSubmitBtnDisabled').show();
      AjaxService.ajaxPost('./api/OtherPaymentMgrApi', newOtherPayment)
        .then((d) => {
          window.Toastr.success('Your payment has been saved', 'Save Successful', { positionClass: 'toast-top-center' });
          this.manageOtherPaymentsMgr.init();
          this.init(d.id);
        })
        .catch(() => {
          $('.otherPaymentSubmitbtn').show();
          $('.otherPaymentSubmitBtnDisabled').hide();
        });
    });
  }
}
