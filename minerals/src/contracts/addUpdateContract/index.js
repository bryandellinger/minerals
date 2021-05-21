/* eslint-disable no-restricted-syntax */
/* eslint-disable no-shadow */
/* eslint-disable no-unused-vars */
// eslint-disable-next-line import/no-extraneous-dependencies
import getFilteredContractSubTypes from './getFilteredContractSubtypes';
import initState from './initState';
import GetDataMgr from './getDataMgr';
import RenderMgr from './renderMgr';
import ValidateandSubmitMgr from './validateAndSubmit';
import MultiSelectMgr from './multiSelectMgr';
import getShowAltIdInformationInd from './getShowAltIdInformationInd';
import PaymentRequirementMgr from './paymentRequirementMgr';
import DatePickerMgr from './datePickerMgr';
import PluggingSuretyMgr from './pluggingSuretyMgr';
import TotalBonusAmountMgr from './totalBonusAmountMgr';
import ContractEventDetailsMgr from './contractEventDetailsMgr';
import HistoricalOwnershipMgr from './historicalOwnershipMgr';

require('select2');
require('select2/dist/css/select2.css');
require('jquery-ui-bundle/jquery-ui.css');
require('jquery-ui-bundle');
require('bootstrap4-toggle');
require('bootstrap4-toggle/css/bootstrap4-toggle.min.css');

export default class AddUpdateContractMgr {
  constructor(sideBarMgr) {
    this.container = $('#addUpdateContractContainer');
    this.sideBarMgr = sideBarMgr;
    this.id = 0;
    this.state = initState();
    this.initSideBarButtonClick();
    this.initChangeContractType();
    this.initChangeContractSubType();
    this.initChangeTerminationReason();
    this.initEditBtn();
    this.initBackBtn();
    this.initCancelBtn();
    this.initUpdateBtn();
    this.initAddNewBtn();
    this.initContractNumBlur();
    this.initTractNumberChange();
    this.initSequenceChange();
    this.initInPoolAcreageChange();
    this.initBufferAcreageChange();
    this.initNSDAcreageAppliesToAllIndChange();
    this.initNSDAcreageChange();
    this.initAcreageChange();
    this.initAltIdCategoriesChange();
    this.initAltIdInformationChange();
    this.initContractNumberOverrideCheckbox();
    this.initRowContractOnEnter();
    this.initRowContractChange();
    this.initDeleteRowContract();
    this.removeEfromNumberInputs();
    this.initScrollToBottom();
    this.initScrollToTop();
    this.getDataMgr = null;
    this.renderMgr = null;
    this.manageContractsMgr = null;
    this.manageTractsMgr = null;
    this.validateandSubmitMgr = null;
    this.multiSelectMgr = null;
    this.paymentRequirementMgr = null;
    this.datePickerMgr = null;
    this.pluggingSuretyMgr = null;
    this.totalBonusAmountMgr = null;
    this.contractEventDetailsMgr = null;
    this.historicalOwnershipMgr = null;
    this.suretyTable = null;
    this.initMgrs();
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

  removeEfromNumberInputs() {
    const invalidChars = ['-', 'e', '+', 'E'];
    this.container.on('keydown', "input[type='number']", (e) => {
      if (invalidChars.includes(e.key)) {
        e.preventDefault();
      }
      // for ie
      if (window.navigator.userAgent.match(/MSIE|Trident/) !== null) {
        const allowedcharcodes = [9, 13, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 190, 8,
          96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 110, 67, 86];
        const charCode = e.which || e.keycode;
        if (!(allowedcharcodes.includes(charCode))
        ) {
          e.preventDefault();
        }
      }
    });
  }

  addManageContractsMgr(manageContractsMgr) {
    this.manageContractsMgr = manageContractsMgr;
  }

  addManageTractsMgr(manageTractsMgr) {
    this.manageTractsMgr = manageTractsMgr;
  }

  initMgrs() {
    this.pluggingSuretyMgr = new PluggingSuretyMgr(this);
    this.renderMgr = new RenderMgr(this);
    this.getDataMgr = new GetDataMgr(this);
    this.validateandSubmitMgr = new ValidateandSubmitMgr(this);
    this.multiSelectMgr = new MultiSelectMgr(this);
    this.paymentRequirementMgr = new PaymentRequirementMgr(this);
    this.paymentRequirementMgr.init();
    this.datePickerMgr = new DatePickerMgr(this);
    this.totalBonusAmountMgr = new TotalBonusAmountMgr(this);
    this.contractEventDetailsMgr = new ContractEventDetailsMgr(this);
    this.historicalOwnershipMgr = new HistoricalOwnershipMgr(this);
  }

  setState(state) {
    this.state = state;
  }

    init(id, wellId, storageRentalId, contractRentalId, suretyId) {
    this.state = initState();
    if (id) {
      this.id = id;
      this.setState({
          ...this.state, updateInd: true, insertInd: false, wellId: wellId || 0,
          storageRentalId: storageRentalId || 0, contractRentalId: contractRentalId || 0, suretyId: suretyId || 0
      });
    } else {
      this.id = 0;
      this.setState({
          ...this.state, editmode: true, updateInd: false, insertInd: true, wellId: wellId || 0,
          storageRentalId: storageRentalId || 0, contractRentalId: contractRentalId || 0, suretyId: suretyId || 0
      });
        }
    this.suretyTable = null;
    this.getDataMgr.getData();
  }

  initRowContractOnEnter() {
    this.container.on('keydown', '#rowContract', (e) => {
      if (e.keyCode === 13) {
        e.preventDefault();
        e.stopPropagation();
        const { rowContracts } = this.state;
        rowContracts.push($(e.target).val());
        $(e.target).val('');
        this.setState({ ...this.state, rowContracts, elementToFocus: 'rowContract' });
        this.render();
      }
    });
  }

  initContractRentalPaymentOverrideChange() {
    this.container.on('change', '#contractRentalPaymentOverride', (e) => {
      this.setState(
        {
          ...this.state,
          contract: {
            ...this.state.contract,
            contractRentalPaymentOverride: $(e.target).is(':checked'),
          },
        },
      );
      this.render();
    });
  }

  initRowContractChange() {
    this.container.on('change', '#rowContract', (e) => {
      const { rowContracts } = this.state;
      rowContracts.push($(e.target).val());
      this.setState({ ...this.state, rowContracts, elementToFocus: 'minimumRoyalty ' });
      this.render();
    });
    }

    initAdditionalContractInformationChange() {
        debugger;
        $('#additionalContractInformation').change((e) => {
            debugger;
            const { contract } = this.state;
            contract.additionalContractInformationId = $(e.target).val() ? parseInt($(e.target).val(), 10) : null;
            this.setState({ ...this.state, contract});
        });
    }


  initDeleteRowContract() {
    this.container.on('click', '.delete-row-contract', (e) => {
      const { rowContracts } = this.state;
      const rowContract = $(e.target).data('rowcontract');
      const newRowContracts = rowContracts.filter((e) => e !== rowContract);
      this.setState({ ...this.state, rowContracts: newRowContracts, elementToFocus: 'rowContract' });
      this.render();
    });
  }

  initContractNumBlur() {
    this.container.on('blur', '#contractNum', (e) => {
      const { contract } = this.state;
      contract.contractNum = $(e.target).val();
      this.setState({ ...this.state, contract, elementToFocus: 'contractSubmitbtn' });
      this.render();
    });
  }

  initCancelBtn() {
      const { wellId, storageRentalId, contractRentalId, suretyId } = this.state;
    this.container.on('click', '#cancelChangesBtn', (e) => { this.init(this.id, wellId, storageRentalId, contractRentalId, suretyId); });
  }

  initInPoolAcreageChange() {
    this.container.on('change', '#inPoolAcreage', () => {
      const { agreement } = this.state;
      agreement.inPoolAcreage = parseFloat($('#inPoolAcreage').val());
      this.setState({ ...this.state, agreement });
      const acreage = (agreement.inPoolAcreage || 0) + (agreement.bufferAcreage || 0);
      $('#acreage').val(acreage.toFixed(4)).trigger('change');
    });
  }

  initLeaseNumChange() {
    this.container.on('change', '#leaseNum', () => {
      const { storage } = this.state;
      storage.leaseNum = $('#leaseNum').val();
      this.setState({ ...this.state, storage });
    });
  }

  initBufferAcreageChange() {
    this.container.on('change', '#bufferAcreage', () => {
      const { agreement } = this.state;
      agreement.bufferAcreage = parseFloat($('#bufferAcreage').val());
      this.setState({ ...this.state, agreement });
      const acreage = (agreement.inPoolAcreage || 0) + (agreement.bufferAcreage || 0);
      $('#acreage').val(acreage.toFixed(4)).trigger('change');
    });
    }

    initNSDAcreageChange() {
        this.container.on('change', '#nsdAcreage', () => {
            const { agreement } = this.state;
            agreement.nsdAcreage = parseFloat($('#nsdAcreage').val());
            this.setState({ ...this.state, agreement });
        });
    }

    initNSDAcreageAppliesToAllIndChange() {
        this.container.on('change', '#nsdAcreageAppliesToAllInd', (e) => {
            const { agreement } = this.state;
            agreement.nsdAcreageAppliesToAllInd = $(e.target).is(':checked'),
            this.setState({ ...this.state, agreement });       
        });
    }

  initAcreageChange() {
    this.container.on('change', '#acreage', () => {
      const { agreement } = this.state;
      const oldacreage = parseFloat(agreement.acreage || 0);
      const newacreage = parseFloat($('#acreage').val() || 0);
      agreement.acreage = $('#acreage').val();
      this.setState({ ...this.state, agreement });
      this.doesElementHaveError('acreage', false);
      if (newacreage > oldacreage) {
        this.totalBonusAmountMgr.calculatetotalBonusAmount();
      }
      this.render();
    });
  }

  doesElementHaveError(element, showOnlyIfFormSubmitted, showOnlyIfNoOverride) {
    return this.validateandSubmitMgr.doesElementHaveError(
      element, showOnlyIfFormSubmitted, showOnlyIfNoOverride,
    );
  }

  initTractNumberChange() {
    this.container.on('change', '#tractNumber', (e) => {
      const { terminatedTracts } = this.state;
      this.setState({ ...this.state, tractNum: $(e.target).val(), elementToFocus: 'districtid' });
      if (terminatedTracts.find((x) => x.tractNum === $(e.target).val())) {
        window.Toastr.warning('Are you sure you want to choose a terminated tract?', 'Warning Terminated Tract', { positionClass: 'toast-top-center' });
      }
      this.render();
    });
  }

  initAltIdCategoriesChange() {
    this.container.on('change', '#altIdCategories', () => {
      const { agreement, altIdCategories } = this.state;
      agreement.altIdCategoryId = parseInt($('#altIdCategories').val(), 10);
      const showAltIdInformationInd = getShowAltIdInformationInd(
        agreement, altIdCategories,
      );
      if (!showAltIdInformationInd) {
        agreement.altIdInformation = null;
      }
      this.setState(
        {
          ...this.state,
          agreement,
          elementToFocus: showAltIdInformationInd ? 'altIdInformation' : 'effectiveDate',
        },
      );
      this.render();
    });
  }


  initAltIdInformationChange() {
    this.container.on('input', '#altIdInformation', () => {
      const { agreement } = this.state;
      agreement.altIdInformation = $('#altIdInformation').val();
      this.setState({ ...this.state, agreement });
      this.doesElementHaveError('altIdInformation', true, false);
    });
  }

  initSequenceChange() {
    this.container.on('change', '#sequence', (e) => {
      const { contract } = this.state;
      contract.sequence = $(e.target).val();
      this.setState({ ...this.state, contract, elementToFocus: 'districtid' });
      this.render();
    });
  }

  initContractNumberOverrideCheckbox() {
    this.container.on('change', '#contractNumOverrideIn', (e) => {
      this.setState({ ...this.state, contractNumOverrideIn: $(e.target).is(':checked'), elementToFocus: $(e.target).is(':checked') ? 'contractNum' : 'effectiveDate' });
      this.render();
    });
  }

  initAddNewBtn() {
    this.container.on('click', '#addNewContract', () => {
      this.init(0);
      $('.spa-btn:first').switchClass('btn-primary', 'btn-light');
      $('.spa-btn:nth-child(3)').switchClass('btn-light', 'btn-primary');
    });
  }

  initUpdateBtn() {
    this.container.on('click', '#editContractBtn, #editContractBtn2, #editContractBtn3', () => {
      this.setState({ ...this.state, editmode: true });
      this.render();
    });
  }


  initEditBtn() {
    this.container.on('click', '#editContractBtn', () => {
      this.setState({ ...this.state, editmode: true });
      this.render();
    });
  }

  initBackBtn() {
    this.container.on('click', '#backToManageContractsBtn', () => {
      this.sideBarMgr.setVisibleContainer('manageContractsContainer');
    });
  }


  initSideBarButtonClick() {
    $('.sidebar').on('click', '.spa-btn', () => {
      this.init();
    });
  }

  initChangeContractType() {
    this.container.on('change', '#contractType', (event) => {
      const { contract, contractSubTypes } = this.state;
      contract.contractTypeId = parseInt($(event.target).val(), 10);
      contract.contractSubTypeId = null;
      const tractOrSequence = this.isSequence() ? 'sequence' : 'tractNumber';
      const elementToFocus = getFilteredContractSubTypes(
        contractSubTypes, contract.contractTypeId, contract.contractSubTypeId,
      ).length ? 'contractSubType' : tractOrSequence;
      this.setState({
        ...this.state,
        contract,
        elementToFocus,
      });
      this.render();
    });
  }

  initChangeContractSubType() {
    this.container.on('change', '#contractSubType', (event) => {
      const { contract } = this.state;
      contract.contractSubTypeId = parseInt($(event.target).val(), 10);
      this.setState({ ...this.state, contract, elementToFocus: this.isSequence() ? 'sequence' : 'tractNumber' });
      this.render();
    });
  }

  initChangeTerminationReason() {
    this.container.on('change', '#terminationReason', (event) => {
      const { agreement } = this.state;
      agreement.terminationReason = $(event.target).val();
      this.setState({ ...this.state, agreement });
      this.doesElementHaveError('terminationReason', true, false);
    });
  }

  isSequence() {
    const { contract, contractTypes, contractSubTypes } = this.state;
    const contractSubType = contractSubTypes.find((x) => x.id === contract.contractSubTypeId);
    return contractSubType ? contractSubType.mapTypeOverride === 'Z' : contractTypes.find((x) => x.id === contract.contractTypeId && x.mapType === 'Z');
  }

  setDistricts() {
    this.multiSelectMgr.setDistricts();
  }


  initTractSelect() {
    $('#tractid').on('select2:select', (e) => {
      const { contract } = this.state;
      contract.tractId = parseInt(e.params.data.id, 10);
      this.setState({ ...this.state, contract, elementToFocus: this.isSequence() ? 'sequence' : 'districtid' });
      this.render();
    });
  }

  initDistrictClose() {
    this.multiSelectMgr.initDistrictClose();
  }

  initDistrictChange() {
    this.multiSelectMgr.initDistrictChange();
  }

  initWellPaymentsDueChange() {
    this.multiSelectMgr.initWellPaymentsDueChange();
  }

  initBaseRentalPaymentsDueChange() {
    this.multiSelectMgr.initBaseRentalPaymentsDueChange();
  }

  initRentalPaymentsDueChange() {
    this.multiSelectMgr.initRentalPaymentsDueChange();
  }

  initContractRentalPaymentsDueChange() {
    this.multiSelectMgr.initContractRentalPaymentsDueChange();
  }

  initTownshipChange() {
    this.multiSelectMgr.initTownshipChange();
  }

  initAssociatedContractChange() {
    this.multiSelectMgr.initAssociatedContractChange();
  }

  initAssociatedTractChange() {
    this.multiSelectMgr.initAssociatedTractChange();
  }

  initFormSubmit() {
    this.validateandSubmitMgr.initFormSubmit();
  }

  render() {
    this.renderMgr.render();
  }
}
