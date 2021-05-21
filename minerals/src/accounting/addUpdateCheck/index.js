/* eslint-disable no-undef */
import DropZone from 'dropzone';
import initState from './initState';
import initDataTable from './initDataTable';
import initAdjustmentDataTable from './initAdjustmentDataTable';
import initStorageFieldPaymentDataTable from './initStorageFieldPaymentDataTable';
import initContractRentalPaymentDataTable from './initContractRentalPaymentDataTable';
import initOtherPaymentDataTable from './initOtherPaymentDataTable';
import initCardHeader from '../initCardHeader';
import elementsToBeValidated from './elementsToBeValidated';
import AjaxService from '../../services/ajaxService';
import formatReadOnlyNumberInputs from '../../services/formatReadOnlyNumberInputs';
import DatePickerMgr from './datePickerMgr';
import DropzoneMgr from './dropzoneMgr';
import 'datatables.net/js/jquery.dataTables';
import 'datatables.net-buttons/js/dataTables.buttons';
import 'datatables.net-buttons/js/buttons.html5';
import 'datatables.net-buttons/js/buttons.flash';
import 'datatables.net-dt/css/jquery.dataTables.css';
import '../../datatablebutton.css';


const template = require('../../views/accounting/addUpdateCheck/index.handlebars');

export default class AddUpdateCheckMgr {
    constructor(sideBarMgr, addUpdatePaymentMgr,
        addUpdateStorageFieldMgr, addUpdateContractRentalMgr, addUpdateOtherPaymentMgr) {
    this.container = $('#addUpdateCheckContainer');
    this.sideBarMgr = sideBarMgr;
    this.addUpdatePaymentMgr = addUpdatePaymentMgr;
    this.addUpdateStorageFieldMgr = addUpdateStorageFieldMgr;
    this.addUpdateContractRentalMgr = addUpdateContractRentalMgr;
    this.addUpdateOtherPaymentMgr = addUpdateOtherPaymentMgr;
    this.initSideBarButtonClick();
    this.initScrollToBottom();
    this.initScrollToTop();
    this.manageChecksMgr = null;
    this.id = 0;
    this.paymentId = 0;
    this.storageRentalId = 0;
    this.contractRentalId = 0;
    this.otherPaymentId = 0;
    this.uploadPaymentId = 0;
    this.datePickerMgr = null;
    this.dropzoneMgr = null;
    this.table = null;
    this.adjustmentTable = null;
    this.storageFieldPaymentTable = null;
    this.contractRentalPaymentTable = null;
    this.otherPaymentTable = null
  }

  setState(state) {
    this.state = state;
  }

    init(id, paymentId, storageRentalId, contractRentalId, otherPaymentId, uploadPaymentId) {
    this.id = id || 0;
    this.paymentId = paymentId || 0;
    this.storageRentalId = storageRentalId || 0;
     this.contractRentalId = contractRentalId || 0;
     this.otherPaymentId = otherPaymentId || 0;
    this.uploadPaymentId = uploadPaymentId || 0;
    this.state = initState();
    this.setState({ ...this.state, editmode: !id });
    this.initEditBtnClick();
    this.initCancelChangesClick();
    this.initCheckInputChange();
    this.initAddNewCheckButtonClick();
    this.initMgrs();
    this.getData(id);
  }

  initSaveDuplicateCheckBtnClick() {
    $('#saveDuplicateCheckBtn').click(() => {
      this.setState({ ...this.state, saveWithWarning: true });
      $('#addUpdateCheckForm').trigger('submit');
    });
  }


  initMgrs() {
    this.datePickerMgr = new DatePickerMgr(this);
    this.dropzoneMgr = new DropzoneMgr(this);
  }

  getData(id) {
    $.when(
      $.get(`./api/CheckMgrApi/${id}`, (check) => {
        if (check && check.id) {
          this.setState({
            ...this.state,
            check: {
              ...check,
              checkDate: check.checkDate ? new Date(check.checkDate) : null,
              receivedDate: check.receivedDate ? new Date(check.receivedDate) : null,
            },
          });
        }
      }),
      $.get('./api/LesseeMgrApi', (lessees) => {
        this.setState({ ...this.state, lessees });
      }),
      $.get(`./api/FileMgrApi/filesbycheck/${id}`, (d) => {
        this.setState(
          { ...this.state, files: d || [] },
        );
      }),
      $.get(`./api/PaymentMgrApi/paymentsbycheck/${id}`, (payments) => {
        this.setState(
          {
            ...this.state,
            payments: payments.map(
              (payment) => (
                {
                  ...payment,
                  postMonthYear: new Date(payment.postYear, payment.postMonth - 1),
                }
              ),
            ),
          },
        );
      }),
        $.get(`./api/StorageRentalMgrApi/getByCheck/${id}`, (storageRentals) => {
        if (id) {
          this.setState(
            {
              ...this.state,
              storageRentals: storageRentals.map(
                (storageRental) => (
                  {
                    ...storageRental,
                          storageRentalEntryDate: storageRental.storageRentalEntryDate ? new Date(storageRental.storageRentalEntryDate) : null,
                          effectiveDate: storageRental.effectiveDate ? new Date(storageRental.effectiveDate) : null,
                  }
                ),
              ),
            },
          );
        }
        }),
        $.get(`./api/ContractRentalMgrApi/getByCheck/${id}`, (contractRentals) => {
            if (id) {
                this.setState(
                    {
                        ...this.state,
                        contractRentals: contractRentals.map(
                            (contractRental) => (
                                {
                                    ...contractRental,
                                    contractRentalEntryDate: contractRental.contractRentalEntryDate ? new Date(contractRental.contractRentalEntryDate) : null,
                                    effectiveDate: contractRental.effectiveDate ? new Date(contractRental.effectiveDate) : null,
                                }
                            ),
                        ),
                    },
                );
            }
        }),
        $.get(`./api/OtherPaymentMgrApi/getByCheck/${id}`, (otherPayments) => {
            if (id) {
                this.setState(
                    {
                        ...this.state,
                        otherPayments: otherPayments.map(
                            (otherPayment) => (
                                {
                                    ...otherPayment,
                                    otherRentalEntryDate: otherPayment.otherRentalEntryDate ? new Date(otherPayment.otherRentalEntryDate) : null,
                                }
                            ),
                        ),
                    },
                );
            }
        }),
        $.get(`./api/PaymentAdjustmentMgrApi/adjustmentsbycheck/${id}`, (adjustments) => {
            if (id) {
                this.setState(
                    {
                        ...this.state,
                        adjustments: adjustments.map(
                            (adjustment) => (
                                {
                                    ...adjustment,
                                    postMonthYear: new Date(adjustment.postYear, adjustment.postMonth - 1),
                                }
                            ),
                        ),
                    },
                );
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
      editmode, check, lessees, payments, adjustments, storageRentals, contractRentals, otherPayments
      } = this.state;
    const handlebarData = {
      headerInfo: initCardHeader(),
      editmode,
      check,
      paymentId: this.paymentId,
      storageRentalId: this.storageRentalId,
      contractRentalId: this.contractRentalId,
      otherPaymentId: this.otherPaymentId,
      uploadPaymentId: this.uploadPaymentId,
      lessees: lessees.map(
        (lessee) => (
          {
            ...lessee,
            selected: lessee.id === check.lesseeId,
          }
        ),
      ),
    };

    const t = template(handlebarData);
    this.container.empty().append(t);
    $('#lesseeId').select2({ placeholder: 'Select a company', width: '100%' });
    $('#lesseeId').on('select2:select', (e) => {
      // eslint-disable-next-line no-shadow
      const { check } = this.state;
      const leseeName = e.params.data.text.trim();
      check.lesseeName = leseeName;
      this.setState({ ...this.state, check });
      $('#lesseeName').val(leseeName);
    });
    this.datePickerMgr.init('checkDate', 'mm/dd/yy');
    this.datePickerMgr.init('receivedDate', 'mm/dd/yy');
    this.initFormSubmit();
    this.initSaveDuplicateCheckBtnClick();
    formatReadOnlyNumberInputs('addUpdateCheckContainer');
    this.dropzoneMgr.init();
    if (this.table) {
      this.table.destroy();
    }
    if (this.adjustmentTable) {
      this.adjustmentTable.destroy();
      }
    if (this.storageFieldPaymentTable) {
          this.storageFieldPaymentTable.destroy();
      }
      if (this.contractRentalPaymentTable) {
          this.contractRentalPaymentTable.destroy();
      }
      if (this.otherPaymentTable) {
          this.otherPaymentTable.destroy();
      }
    this.table = initDataTable(payments);
    this.adjustmentTable = initAdjustmentDataTable(adjustments);
    this.storageFieldPaymentTable = initStorageFieldPaymentDataTable(storageRentals);
    this.contractRentalPaymentTable = initContractRentalPaymentDataTable(contractRentals);
    this.otherPaymentTable = initOtherPaymentDataTable(otherPayments);
    this.initSelectClick();
  }

  initSelectClick() {
    $('#checkPaymentsTable tbody').on('click', 'button', (event) => {
      const data = this.table.row($(event.target).parents('tr')).data();
      this.addUpdatePaymentMgr.init(data.id);
      this.sideBarMgr.setVisibleContainer('addUpdatePaymentContainer');
    });
    $('#checkAdjustmentsTable tbody').on('click', 'button', (event) => {
      const data = this.adjustmentTable.row($(event.target).parents('tr')).data();
      this.addUpdatePaymentMgr.init(data.id);
      this.sideBarMgr.setVisibleContainer('addUpdatePaymentContainer');
    });
    $('#checkStorageFieldsTable tbody').on('click', 'button', (event) => {
        const data = this.storageFieldPaymentTable.row($(event.target).parents('tr')).data();
        this.addUpdateStorageFieldMgr.init(data.id);
        this.sideBarMgr.setVisibleContainer('addUpdateStorageFieldContainer');
    });
      $('#checkContractRentalsTable tbody').on('click', 'button', (event) => {
          const data = this.contractRentalPaymentTable.row($(event.target).parents('tr')).data();
          this.addUpdateContractRentalMgr.init(data.id);
          this.sideBarMgr.setVisibleContainer('addUpdateContractRentalContainer');
      });
      $('#checkOtherPaymentsTable tbody').on('click', 'button', (event) => {
          const data = this.otherPaymentTable.row($(event.target).parents('tr')).data();
          this.addUpdateOtherPaymentMgr.init(data.id);
          this.sideBarMgr.setVisibleContainer('addUpdateOtherPaymentContainer');
      });
  }

  addManageChecksMgr(manageChecksMgr) {
    this.manageChecksMgr = manageChecksMgr;
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

  initAddNewCheckButtonClick() {
    this.container.on('click', '.addNewCheck', () => {
        this.init(0, this.paymentId, this.storageRentalId, this.contractRentalId, this.otherPaymentId, this.uploadPaymentId);
    });
  }


  initEditBtnClick() {
    this.container.on('click', '.editCheckBtn', () => {
      this.setState({ ...this.state, editmode: true });
      this.render();
    });
  }

  initCancelChangesClick() {
    this.container.on('click', '#cancelChangesBtn', () => {
        this.init(this.id, this.paymentId, this.storageRentalId, this.contractRentalId, this.otherPaymentId, this.uploadPaymentId);
    });
  }

  initCheckInputChange() {
    this.container.on('change', '.checkinput', (e) => {
      const { check } = this.state;
      const element = $(e.target).attr('id');
      check[element] = $(e.target).val();
      this.setState({ ...this.state, check });
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

  initFormSubmit() {
    // eslint-disable-next-line consistent-return
    $('#addUpdateCheckForm').submit(() => {
      const {
        check, files, saveWithWarning,
      } = this.state;

      this.setState({ ...this.state, submitInd: true, saveWithWarning: false });
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

      const newCheck = {
        ...check,
        totalAmount: parseFloat(check.totalAmount),
        lesseeId: parseInt(check.lesseeId, 10),
        files,
      };
      if (this.id || saveWithWarning) {
        this.postCheck(newCheck);
      } else {
        AjaxService.ajaxGet(`./api/CheckMgrApi/GetCheckByCheckNum/${check.checkNum}`)
          .then((d) => {
            if (d && d.id) {
              window.$('#warningModal').modal('show');
              return false;
            }
            this.postCheck(newCheck);
          });
      }
    });
  }


  postCheck(newCheck) {
    $('.checkSubmitbtn').hide();
    $('.checkSubmitBtnDisabled').show();
    AjaxService.ajaxPost('./api/CheckMgrApi', newCheck)
      .then((d) => {
        window.Toastr.success('Your check has been saved', 'Save Successful', { positionClass: 'toast-top-center' });
        this.manageChecksMgr.init();
        this.init(d.id, this.paymentId, this.storageRentalId, this.contractRentalId, this.otherPaymentId, this.uploadPaymentId);
      })
      .catch(() => {
        $('.checkSubmitbtn').show();
        $('.checkSubmitBtnDisabled').hide();
      });
  }
}
