/* eslint-disable no-nested-ternary */
/* eslint-disable max-len */
/* eslint-disable no-undef */
import initState from './initState';
import initCardHeader from '../initCardHeader';
import formatReadOnlyNumberInputs from '../../services/formatReadOnlyNumberInputs';
import CheckNumberMgr from './checkNumberMgr';
import initContractDataTable from './initContractDataTable';
import elementsToBeValidated from './elementsToBeValidated';
import getShowOverdueInd from './getShowOverdueInd';
import * as constants from '../../constants';
import DatePickerMgr from './datePickerMgr';
import AjaxService from '../../services/ajaxService';

const template = require('../../views/accounting/addUpdateStorageField/index.handlebars');

export default class AddUpdateStorageFieldMgr {
  constructor(sideBarMgr) {
    this.container = $('#addUpdateStorageFieldContainer');
    this.sideBarMgr = sideBarMgr;
    this.initSideBarButtonClick();
    this.initScrollToBottom();
    this.initScrollToTop();
    this.checkNumberMgr = null;
    this.id = 0;
    this.table = null;
    this.manageStorageFieldsMgr = null;
  }

  setState(state) {
    this.state = state;
  }

  init(id) {
    if (this.table) {
      this.table.destroy();
    }
    this.table = null;
    this.id = id || 0;
    this.state = initState();
    this.setState({ ...this.state, editmode: !id });
    this.initEditBtnClick();
    this.initCancelChangesClick();
    this.datePickerMgr = null;
    this.initMgrs();
    this.getData(id);
  }

  initMgrs() {
    this.checkNumberMgr = new CheckNumberMgr(this);
    this.datePickerMgr = new DatePickerMgr(this);
  }


  getData(id) {
    $('.spinner').show();
    $.when(
      $.get(`./api/StorageRentalMgrApi/${id}`, (storageRental) => {
        if (storageRental && storageRental.id) {
          this.setState({
            ...this.state,
            storageRental: {
              ...storageRental,
              storageRentalEntryDate: storageRental.storageRentalEntryDate ? new Date(storageRental.storageRentalEntryDate) : null,
            },

          });
        }
      }),
      $.get(`./api/CheckMgrApi/GetByStorageRental/${id}`, (check) => {
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
      $.get(`./api/LesseeMgrApi/GetByStorageRental/${id}`, (lessee) => {
        if (lessee) {
          this.setState({ ...this.state, lessee });
        }
      }),
      $.get(`./api/ContractMgrApi/GetByStorageRental/${id}`, (contract) => {
        if (contract) {
          this.setState({
            ...this.state,
            contract: {
              ...contract,
              terminationDate: contract.terminationDate ? new Date(contract.terminationDate) : null,
              effectiveDate: contract.effectiveDate ? new Date(contract.effectiveDate) : null,
              expirationDate: contract.expirationDate ? new Date(contract.expirationDate) : null,
            },
          });
        }
      }),
      $.get('./api/ContractMgrApi/GetForStorageRental', (contracts) => {
        if (contracts) {
          this.setState({
            ...this.state,
            contracts:
            contracts.map((contract) => (
              {
                ...contract,
                terminationDate: contract.terminationDate ? new Date(contract.terminationDate) : null,
                effectiveDate: contract.effectiveDate ? new Date(contract.effectiveDate) : null,
                expirationDate: contract.expirationDate ? new Date(contract.expirationDate) : null,
              }
            )),
          });
        }
      }),
      $.get('./api/MonthMgrApi', (months) => {
        this.setState({
          ...this.state, months,
        });
      }),
      $.get('./api/StorageRentalPaymentTypeMgrApi', (storageRentalPaymentTypes) => {
        this.setState({
          ...this.state, storageRentalPaymentTypes,
        });
      }),
      $.get('./api/PeriodTypeMgrApi', (periodTypes) => {
        this.setState({
          ...this.state, periodTypes,
        });
      }),
      $.get(`./api/StorageWellPaymentMonthJunctionMgrApi/StorageWellPaymentMonthsByStorageRental/${id}`, (storageWellPaymentMonths) => {
        this.setState({
          ...this.state, storageWellPaymentMonths,
        });
      }),
      $.get(`./api/StorageBaseRentalPaymentMonthJunctionMgrApi/StorageBaseRentalPaymentMonthsByStorageRental/${id}`, (storageBaseRentalPaymentMonths) => {
        this.setState({
          ...this.state, storageBaseRentalPaymentMonths,
        });
      }),
      $.get(`./api/StorageRentalPaymentMonthJunctionMgrApi/StorageRentalPaymentMonthsByStorageRental/${id}`, (storageRentalPaymentMonths) => {
        this.setState({
          ...this.state, storageRentalPaymentMonths,
        });
      }),
    ).then(() => {
      const {
        storageRental, storageRentalPaymentTypes, periodTypes,
      } = this.state;
      if (!storageRental.storageRentalPaymentTypeId) {
        storageRental.storageRentalPaymentTypeId = storageRentalPaymentTypes.find((x) => x.storageRentalPaymentTypeName === constants.StorageRentalTypeBaseRental).id;
        this.setState({
          ...this.state, storageRental,
        });
      }
      if (!storageRental.periodTypeId) {
        storageRental.periodTypeId = periodTypes.find((x) => x.periodTypeName === constants.PeriodTypeAnnual).id;
        this.setState({
          ...this.state, storageRental,
        });
      }
      if (!storageRental.paymentPeriodYear) {
        const d = new Date();
        storageRental.paymentPeriodYear = d.getFullYear();
        this.setState({
          ...this.state, storageRental,
        });
      }
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
      editmode, storageRental, check, lessee, contracts, contract,
      storageBaseRentalPaymentMonths,
      storageRentalPaymentMonths, storageWellPaymentMonths, months,
      storageRentalPaymentTypes, periodTypes,
    } = this.state;
    const d = new Date();
    const fullYear = d.getFullYear();
    const start = fullYear - 50;
    const end = fullYear + 50;
    const paymentPeriodYears = Array(end - start + 1).fill().map((_, idx) => start + idx);
    const handlebarData = {
      headerInfo: initCardHeader(),
      showExpirationInd: contract && contract.expirationDate && contract.expirationDate < new Date(),
      showTerminationInd: contract && contract.terminationDate && contract.terminationDate < new Date(),
      editmode,
      storageRental,
      check,
      lessee,
      contract: {
        ...contract,
        terminationDate: contract.terminationDate ? contract.terminationDate.toLocaleDateString() : null,
        effectiveDate: contract.effectiveDate ? contract.effectiveDate.toLocaleDateString() : null,
        expirationDate: contract.expirationDate ? contract.expirationDate.toLocaleDateString() : null,
      },
      showLesseeNameFromCheckInd: lessee.lesseeName !== check.lesseeName,
      storageBaseRentalPeriod: storageBaseRentalPaymentMonths && storageBaseRentalPaymentMonths.length
        ? storageBaseRentalPaymentMonths.length === 1 ? 'Annual'
              : storageBaseRentalPaymentMonths.length === 4 ? 'Quarterly' : 'Other'
        : 'N/A',
      storageRentalPeriod: storageRentalPaymentMonths && storageRentalPaymentMonths.length
        ? storageRentalPaymentMonths.length === 1 ? 'Annual'
              : storageRentalPaymentMonths.length === 4 ? 'Quarterly' : 'Other'
        : 'N/A',
      storageWellRentalPeriod: storageWellPaymentMonths && storageWellPaymentMonths.length
        ? storageWellPaymentMonths.length === 1 ? 'Annual'
              : storageWellPaymentMonths.length === 4 ? 'Quarterly' : 'Other'
        : 'N/A',
      storageBaseRentalPaymentMonths: months.map((month) => (
        {
          ...month,
          selected: storageBaseRentalPaymentMonths.map((x) => x.id).includes(month.id),
        }
      )),
      storageRentalPaymentMonths: months.map((month) => (
        {
          ...month,
          selected: storageRentalPaymentMonths.map((x) => x.id).includes(month.id),
        }
      )),
      storageWellPaymentMonths: months.map((month) => (
        {
          ...month,
          selected: storageWellPaymentMonths.map((x) => x.id).includes(month.id),
        }
      )),
      storageRentalPaymentTypes: storageRentalPaymentTypes.map((storageRentalPaymentType) => (
        {
          ...storageRentalPaymentType,
          selected: storageRentalPaymentType.id === parseInt(storageRental.storageRentalPaymentTypeId, 10),
        }
      )),
      periodTypes: periodTypes.map((periodType) => (
        {
          ...periodType,
          selected: periodType.id === parseInt(storageRental.periodTypeId, 10),
        }
      )),
      showWellInd: parseInt(storageRental.storageRentalPaymentTypeId, 10)
         === storageRentalPaymentTypes.find((x) => x.storageRentalPaymentTypeName === constants.StorageRentalTypeWell).id,
      paymentPeriodYears: paymentPeriodYears.map((paymentPeriodYear) => (
        {
          id: paymentPeriodYear,
          paymentPeriodYearName: paymentPeriodYear,
          selected: paymentPeriodYear === parseInt(storageRental.paymentPeriodYear, 10),
        }
      )),
    };
    const t = template(
      {
        ...handlebarData,
        showOverdueInd: getShowOverdueInd(
          handlebarData.periodTypes.find((x) => x.selected),
          handlebarData.storageRentalPaymentTypes.find((x) => x.selected),
          handlebarData.storageBaseRentalPaymentMonths.filter((x) => x.selected),
          handlebarData.storageRentalPaymentMonths.filter((x) => x.selected),
          handlebarData.storageWellPaymentMonths.filter((x) => x.selected),
          handlebarData.storageRental,
        ),
      },
    );
    this.container.empty().append(t);
    if (this.table) {
      this.table.destroy();
      }
      if (check && check.lesseeId) {
          this.table = initContractDataTable(contracts.filter(x => x.lesseeId === check.lesseeId));
      } else {
          this.table = initContractDataTable(contracts);
      }
 
    this.checkNumberMgr.init();
    this.checkNumberMgr.initCheckNumChange();
    this.initSelectClick();
    $('#baseRentalPaymentsDue').select2({ placeholder: 'N/A', width: '100%' });
    $('#rentalPaymentsDue').select2({ placeholder: 'N/A', width: '100%' });
    $('#wellPaymentsDue').select2({ placeholder: 'N/A', width: '100%' });
    this.datePickerMgr.init('storageRentalEntryDate', 'mm/dd/yy');
    formatReadOnlyNumberInputs('addUpdateStorageFieldContainer');
    this.initFormSubmit();
    this.initStorageRentalInputChange();
  }


  initSelectClick() {
    $('#storageFieldsModalTable tbody').on('click', 'button', (event) => {
      $('.spinner').show();
      document.getElementById('contractModalCloseBtn').click();
      const contract = this.table.row($(event.target).parents('tr')).data();
      this.setState({
        ...this.state,
        contract,
        storageRental: { ...this.state.storageRental, contractId: contract.contractId },
      });
      $.when(
        $.get(`./api/StorageWellPaymentMonthJunctionMgrApi/StorageWellPaymentMonthsByContract/${contract.contractId}`, (storageWellPaymentMonths) => {
          this.setState({
            ...this.state, storageWellPaymentMonths,
          });
        }),
        $.get(`./api/StorageBaseRentalPaymentMonthJunctionMgrApi/StorageBaseRentalPaymentMonthsByContract/${contract.contractId}`, (storageBaseRentalPaymentMonths) => {
          this.setState({
            ...this.state, storageBaseRentalPaymentMonths,
          });
        }),
        $.get(`./api/StorageRentalPaymentMonthJunctionMgrApi/StorageRentalPaymentMonthsByContract/${contract.contractId}`, (storageRentalPaymentMonths) => {
          this.setState({
            ...this.state, storageRentalPaymentMonths,
          });
        }),
      ).then(() => {
        this.render();
        $('.spinner').hide();
      });
    });
  }

  addManageStorageFieldsMgr(manageStorageFieldsMgr) {
    this.manageStorageFieldsMgr = manageStorageFieldsMgr;
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

  initAddNewStorageFieldButtonClick() {
    this.container.on('click', '.addNewStorageField', () => {
      this.init(0);
    });
  }

  initEditBtnClick() {
    this.container.on('click', '.editStorageFieldBtn', () => {
      this.setState({ ...this.state, editmode: true });
      this.render();
    });
  }

  initCancelChangesClick() {
    this.container.on('click', '#cancelStorageFieldChangesBtn', () => {
      this.init(this.id);
    });
  }

  initStorageRentalInputChange() {
    $('.storageRentalInput').change((e) => {
      const { storageRental } = this.state;
      const element = $(e.target).attr('id');
      storageRental[element] = $(e.target).val();
      this.setState({ ...this.state, storageRental });
      if (elementsToBeValidated.find((x) => x.element === element)) {
        this.doesElementHaveError(element);
      }
      if (element === 'storageRentalPaymentTypeId'
      || element === 'paymentPeriodYear'
      || element === 'storageRentalPaymentTypeId'
      || element === 'periodTypeId') {
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
    $('#storageFieldContract').attr('readonly', false);
    $('#storageFieldContract').attr('disabled', false);
    if (!$(`#${element}`)[0].checkValidity()) {
      $(`#${element}`).addClass('is-invalid');
      if ($(`#${element}`).data('select2')) {
        $($(`#${element}`).data('select2').$container).addClass('is-invalid');
      }
      $(`#${element}Error`).show();
      return true;
    }
    $('#storageFieldContract').attr('readonly', true);
    $('#storageFieldContract').attr('disabled', true);
    return false;
  }

  initFormSubmit() {
    // eslint-disable-next-line consistent-return
    $('#addUpdateStorageFieldForm').submit(() => {
      const { storageRental, contract } = this.state;

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

      const newStorageRental = {
        ...storageRental,
        storageId: parseInt(contract.storageId, 10),
        rentPay: parseFloat(storageRental.rentPay),
        checkId: parseInt(storageRental.checkId, 10),
        storageRentalPaymentTypeId: parseInt(storageRental.storageRentalPaymentTypeId, 10),
        periodTypeId: parseInt(storageRental.periodTypeId, 10),
      };

      $('.storageFieldSubmitbtn').hide();
      $('.storageFieldSubmitBtnDisabled').show();
      AjaxService.ajaxPost('./api/StorageRentalMgrApi', newStorageRental)
        .then((d) => {
          window.Toastr.success('Your payment has been saved', 'Save Successful', { positionClass: 'toast-top-center' });
          this.manageStorageFieldsMgr.init();
          this.init(d.id);
        })
        .catch(() => {
          $('.storageFieldSubmitbtn').show();
          $('.storageFieldSubmitBtnDisabled').hide();
        });
    });
  }
}
