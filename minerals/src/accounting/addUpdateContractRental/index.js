/* eslint-disable no-nested-ternary */
/* eslint-disable max-len */
/* eslint-disable no-undef */
import initState from './initState';
import initCardHeader from '../initCardHeader';
import formatReadOnlyNumberInputs from '../../services/formatReadOnlyNumberInputs';
import CheckNumberMgr from './checkNumberMgr';
import getRentalPayment from '../../services/getRentalPayment';
import initContractDataTable from './initContractDataTable';
import * as constants from '../../constants';
import elementsToBeValidated from './elementsToBeValidated';
import DatePickerMgr from './datePickerMgr';
import AjaxService from '../../services/ajaxService';
import getShowOverdueInd from './getShowOverdueInd';

const template = require('../../views/accounting/addUpdateContractRental/index.handlebars');

export default class AddUpdateContractRentalMgr {
  constructor(sideBarMgr) {
    this.container = $('#addUpdateContractRentalContainer');
    this.sideBarMgr = sideBarMgr;
    this.initSideBarButtonClick();
    this.initScrollToBottom();
    this.initScrollToTop();
    this.id = 0;
    this.table = null;
    this.checkNumberMgr = null;
    this.datePickerMgr = null;
    this.manageContractRentalsMgr = null;
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
    this.initMgrs();
    this.getData(id);
  }

  initMgrs() {
    this.checkNumberMgr = new CheckNumberMgr(this);
    this.datePickerMgr = new DatePickerMgr(this);
  }

  addManageContractRentalsMgr(manageContractRentalsMgr) {
    this.manageContractRentalsMgr = manageContractRentalsMgr;
  }

  getData(id) {
    $('.spinner').show();
    $.when(
      $.get(`./api/ContractRentalMgrApi/${id}`, (contractRental) => {
        if (contractRental && contractRental.id) {
          this.setState({
            ...this.state,
            contractRental: {
              ...contractRental,
              contractRentalEntryDate: contractRental.contractRentalEntryDate ? new Date(contractRental.contractRentalEntryDate) : null,
            },
          });
        }
      }),
      $.get(`./api/CheckMgrApi/GetByContractRental/${id}`, (check) => {
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
      $.get(`./api/LesseeMgrApi/GetByContractRental/${id}`, (lessee) => {
        if (lessee) {
          this.setState({ ...this.state, lessee });
        }
      }),
      $.get(`./api/ContractMgrApi/GetByContractRental/${id}`, (contract) => {
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
      $.get('./api/MonthMgrApi', (months) => {
        this.setState({
          ...this.state, months,
        });
      }),
      $.get(`./api/ContractRentalPaymentMonthJunctionMgrApi/ContractRentalPaymentMonthsByContractRental/${id}`, (contractRentalPaymentMonths) => {
        this.setState({
          ...this.state, contractRentalPaymentMonths,
        });
      }),
      $.get(`./api/ContractMgrApi/GetForContractRental/${id}`, (contracts) => {
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
      $.get('./api/PeriodTypeMgrApi', (periodTypes) => {
        this.setState({
          ...this.state, periodTypes,
        });
      }),
    ).then(() => {
      const { periodTypes, contractRental } = this.state;
      if (!contractRental.periodTypeId) {
        contractRental.periodTypeId = periodTypes.find((x) => x.periodTypeName === constants.PeriodTypeAnnual).id;
        this.setState({
          ...this.state, contractRental,
        });
        if (!contractRental.contractPaymentPeriodYear) {
          const d = new Date();
          contractRental.contractPaymentPeriodYear = d.getFullYear();
          this.setState({
            ...this.state, contractRental,
          });
        }
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
      editmode, contractRental, check, lessee, contract, contracts,
      contractRentalPaymentMonths, months, periodTypes,
    } = this.state;

    getRentalPayment({
      acreage: contract.acreage,
      contractId: contract.contractId,
      effectiveDate: contract.effectiveDate,
      secondThroughFourthYearDelayRental: contract.secondThroughFourthYearDelayRental || 0,
      fifthYearOnwardDelayRental: contract.fifthYearOnwardDelayRental || 0,
    })
      .then((rentalPayment) => {
        if (!contract.contractRentalPaymentOverride) {
          contract.contractRentalPayment = rentalPayment.rentalPaymentDue;
          this.setState({
            ...this.state, contract,
          });
        }
        const d = new Date();
        const fullYear = d.getFullYear();
        const start = fullYear - 50;
        const end = fullYear + 50;
        const paymentPeriodYears = Array(end - start + 1).fill().map((_, idx) => start + idx);
        const handlebarData = {
          headerInfo: initCardHeader(),
          showExpirationInd: contract && contract.expirationDate && contract.expirationDate < new Date(),
          showTerminationInd: contract && contract.terminationDate && contract.terminationDate < new Date(),
          rentalPayment,
          editmode,
          contractRental,
          check,
          lessee,
          showLesseeNameFromCheckInd: lessee && check && lessee.lesseeName !== check.lesseeName,
          contract: {
            ...contract,
            terminationDate: contract.terminationDate ? contract.terminationDate.toLocaleDateString() : null,
            effectiveDate: contract.effectiveDate ? contract.effectiveDate.toLocaleDateString() : null,
            expirationDate: contract.expirationDate ? contract.expirationDate.toLocaleDateString() : null,
          },
          contractRentalPaymentMonths: months.map((month) => (
            {
              ...month,
              selected: contractRentalPaymentMonths.map((x) => x.id).includes(month.id),
            }
          )),
          periodTypes: periodTypes.map((periodType) => (
            {
              ...periodType,
              selected: periodType.id === parseInt(contractRental.periodTypeId, 10),
            }
          )),
          paymentPeriodYears: paymentPeriodYears.map((paymentPeriodYear) => (
            {
              id: paymentPeriodYear,
              paymentPeriodYearName: paymentPeriodYear,
              selected: paymentPeriodYear === parseInt(contractRental.contractPaymentPeriodYear, 10),
            }
          )),
        };
        const t = template(
          {
            ...handlebarData,
            showOverdueInd: getShowOverdueInd(
              handlebarData.periodTypes.find((x) => x.selected),
              handlebarData.contractRental,
              handlebarData.contractRentalPaymentMonths.filter((x) => x.selected),
            ),
          },
        );
        this.container.empty().append(t);
        if (this.table) {
          this.table.destroy();
        }
        this.table = initContractDataTable(contracts);
        this.checkNumberMgr.init();
        this.checkNumberMgr.initCheckNumChange();
        this.initSelectClick();
        $('#contractRentalPaymentsDue').select2({ placeholder: 'N/A', width: '100%' });
        this.initContractRentalInputChange();
        this.datePickerMgr.init('contractRentalEntryDate', 'mm/dd/yy');
        this.initheldByProductionToggle();
        this.initFormSubmit();
        formatReadOnlyNumberInputs('addUpdateContractRentalContainer');
      });
  }

  initheldByProductionToggle() {
    $('#heldByProductionToggle').bootstrapToggle({
      on: 'Yes',
      off: 'No',
      onstyle: 'secondary',
      offstyle: 'secondary',
    });
    $('#heldByProductionToggle').change((e) => {
      this.setState({ ...this.state, contractRental: { ...this.state.contractRental, heldByProduction: $(e.target).prop('checked') } });
      this.render();
    });
  }

  initContractRentalInputChange() {
    $('.contractRentalInput').change((e) => {
      const { contractRental } = this.state;
      const element = $(e.target).attr('name');
      contractRental[element] = $(e.target).val();
      this.setState({ ...this.state, contractRental });
      if (elementsToBeValidated.find((x) => x.element === element)) {
        this.doesElementHaveError(element);
      }
        if (element === 'periodTypeId' || element === 'contractPaymentPeriodYear') {
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
    $('#contractRentalContract').attr('readonly', false);
    $('#contractRentalContract').attr('disabled', false);
    if (!$(`#${element}`)[0].checkValidity()) {
      $(`#${element}`).addClass('is-invalid');
      if ($(`#${element}`).data('select2')) {
        $($(`#${element}`).data('select2').$container).addClass('is-invalid');
      }
      $(`#${element}Error`).show();
      return true;
    }
    $('#contractRentalContract').attr('readonly', true);
    $('#contractRentalContract').attr('disabled', true);
    return false;
  }

  initSelectClick() {
    $('#contractRentalsModalTable tbody').on('click', 'button', (event) => {
      $('.spinner').show();
      document.getElementById('contractRentalTableModalCloseBtn').click();
      const contract = this.table.row($(event.target).parents('tr')).data();
      this.setState({ ...this.state, contract });
      $.when(
        $.get(`./api/ContractRentalPaymentMonthJunctionMgrApi/ContractRentalPaymentMonthsByContract/${contract.contractId}`, (contractRentalPaymentMonths) => {
          this.setState({
            ...this.state, contractRentalPaymentMonths,
          });
        }),
      ).then(() => {
        this.render();
        $('.spinner').hide();
      });
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

  initAddNewContractRentalButtonClick() {
    this.container.on('click', '.addNewContractRental', () => {
      this.init(0);
    });
  }

  initEditBtnClick() {
    this.container.on('click', '.editContractRentalBtn', () => {
      this.setState({ ...this.state, editmode: true });
      this.render();
    });
  }

  initCancelChangesClick() {
    this.container.on('click', '#cancelContractRentalChangesBtn', () => {
      this.init(this.id);
    });
  }

  initFormSubmit() {
    // eslint-disable-next-line consistent-return
    $('#addUpdateContractRentalForm').submit(() => {
      const { contractRental, contract } = this.state;

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

      const newContractRental = {
        ...contractRental,
        contractId: parseInt(contract.contractId, 10),
        contractRentPay: parseFloat(contractRental.contractRentPay),
        checkId: parseInt(contractRental.checkId, 10),
        periodTypeId: parseInt(contractRental.periodTypeId, 10),
      };

      $('.contractRentalSubmitbtn').hide();
      $('.contractRentalSubmitBtnDisabled').show();
      AjaxService.ajaxPost('./api/ContractRentalMgrApi', newContractRental)
        .then((d) => {
          window.Toastr.success('Your payment has been saved', 'Save Successful', { positionClass: 'toast-top-center' });
          this.manageContractRentalsMgr.init();
          this.init(d.id);
        })
        .catch(() => {
          $('.contractRentalSubmitbtn').show();
          $('.contractRentalSubmitBtnDisabled').hide();
        });
    });
  }
}
