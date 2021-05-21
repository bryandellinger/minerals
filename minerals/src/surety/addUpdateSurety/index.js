/* eslint-disable no-nested-ternary */
/* eslint-disable max-len */
/* eslint-disable no-undef */
import initState from './initState';
import initCardHeader from '../initCardHeader';
import formatReadOnlyNumberInputs from '../../services/formatReadOnlyNumberInputs';
import AjaxService from '../../services/ajaxService';
import elementsToBeValidated from './elementsToBeValidated';
import * as constants from '../../constants';
import DatePickerMgr from './datePickerMgr';
import SuretyRiderMgr from './suretyRiderMgr';
import SuretyWellMgr from './suretyWellMgr';

const template = require('../../views/surety/addUpdateSurety/index.handlebars');

export default class AddUpdateSuretyMgr {
  constructor(sideBarMgr) {
    this.container = $('#addUpdateSuretyContainer');
    this.sideBarMgr = sideBarMgr;
    this.initSideBarButtonClick();
    this.initScrollToBottom();
    this.initScrollToTop();
    this.id = 0;
    this.wellId = 0;
    this.contractId = 0;
    this.manageSuretiesMgr = null;
    this.datePickerMgr = null;
    this.suretyRiderMgr = null;
    this.suretyWellMgr = null;
  }

  setState(state) {
    this.state = state;
  }

    init(id, wellId, contractId) {
        debugger;
   this.id = id || 0;
   this.wellId = wellId || 0;
   this.contractId = contractId || 0;
    this.state = initState();
    this.setState({ ...this.state, editmode: !id });
    this.initEditBtnClick();
    this.initCancelChangesClick();
    this.initAddNewSuretyBtnClick();
    this.initMgrs();
    this.getData(id);
  }

  initMgrs() {
    this.datePickerMgr = new DatePickerMgr(this);
    this.suretyRiderMgr = new SuretyRiderMgr(this);
    this.suretyWellMgr = new SuretyWellMgr(this);
  }

  addManageSuretiesMgr(manageSuretiesMgr) {
    this.manageSuretiesMgr = manageSuretiesMgr;
  }

  getData(id) {
    $('.spinner').show();
    $.when(
      $.get(`./api/SuretyMgrApi/${id}`, (surety) => {
        if (surety && surety.id) {
          this.setState({
            ...this.state,
            surety: {
              ...surety,
              issueDate: surety.issueDate
                ? new Date(surety.issueDate) : null,
            },
          });
        }
      }),
      $.get('./api/SuretyTypesMgrApi', (suretyTypes) => {
        this.setState({ ...this.state, suretyTypes });
      }),
      $.get('./api/BondCategoryMgrApi', (bondCategories) => {
        this.setState({ ...this.state, bondCategories });
      }),
      $.get('./api/LesseeMgrApi', (lessees) => {
        this.setState({ ...this.state, lessees });
      }),
      $.get('./api/SuretyMgrApi/GetInsurers', (insurers) => {
        this.setState({ ...this.state, insurers });
      }),
      $.get('./api/SuretyMgrApi/GetRiderReasons', (riderReasons) => {
        this.setState({ ...this.state, riderReasons });
      }),
      $.get(`./api/SuretyMgrApi/GetSuretyRidersBySurety/${id}`, (suretyRiders) => {
        if (suretyRiders && suretyRiders.length) {
          this.setState({
            ...this.state,
            suretyRiders: suretyRiders.map((suretyRider) => (
              {
                ...suretyRider,
                effectiveDate: suretyRider.effectiveDate
                  ? new Date(suretyRider.effectiveDate) : null,
              }
            )),
          });
        }
      }),
      $.get(`./api/SuretyMgrApi/GetSuretyWellsBySurety/${id}`, (suretyWells) => {
        if (suretyWells && suretyWells.length) {
          this.setState({ ...this.state, suretyWells });
        }
      }),
      $.get('./api/WellMgrApi', (wells) => {
        this.setState({ ...this.state, wells });
      }),
      $.get('./api/ContractMgrApi', (contracts) => {
        this.setState({ ...this.state, contracts });
      }),
    ).then(() => {
      const {
        suretyTypes, surety, bondCategories,
      } = this.state;
      if (!surety.suretyTypeId) {
        surety.suretyTypeId = suretyTypes.find((x) => x.suretyTypeName === constants.SuretyTypeBond).id;
        this.setState({
          ...this.state, surety,
        });
      }
      if (!surety.bondCategoryId) {
        surety.bondCategoryId = bondCategories.find((x) => x.bondCategoryName === constants.BondCategoryPlugging).id;
        this.setState({
          ...this.state, surety,
        });
      }
      this.render();
      $('.spinner').hide();
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

  initSuretyInputChange() {
    $('.suretyInput').change((e) => {
      const { surety } = this.state;
      const element = $(e.target).attr('name');
      surety[element] = $(e.target).val();
      this.setState({ ...this.state, surety });
      if (elementsToBeValidated.find((x) => x.element === element)) {
        this.doesElementHaveError(element);
      }
      if (element === 'suretyStatus' || element === 'bondCategoryId') {
        this.render();
      }
    });
  }

  initSideBarButtonClick() {
    $('.sidebar').on('click', '.spa-btn', () => {
      this.init(0);
    });
  }

  render() {
    const {
      editmode, surety, suretyTypes, bondCategories, lessees,
      suretyStatuses, contracts,
    } = this.state;
    const handlebarData = {
        headerInfo: initCardHeader(),
        wellId: this.wellId,
        contractId: this.contractId,
      editmode,
      surety,
      showReleasedIn: surety.suretyStatus === constants.SuretyStatusReleased,
      pluggingIn: bondCategories.find((x) => x.id === parseInt(surety.bondCategoryId, 10) && x.bondCategoryName === constants.BondCategoryPlugging),
      suretyTypes: suretyTypes.map((suretyType) => (
        {
          ...suretyType,
          selected: suretyType.id === parseInt(surety.suretyTypeId, 10),
        }
      )),
      bondCategories: bondCategories.map((bondCategory) => (
        {
          ...bondCategory,
          selected: bondCategory.id === parseInt(surety.bondCategoryId, 10),
        }
      )),
      lessees: lessees.map((lessee) => (
        {
          ...lessee,
          selected: lessee.id === parseInt(surety.lesseeId, 10),
        }
      )),
      contracts: contracts.map((contract) => (
        {
          ...contract,
          selected: contract.id === parseInt(surety.contractId, 10),
        }
      )),
      suretyStatuses: suretyStatuses.map((suretyStatus) => (
        {
          suretyStatus,
          selected: surety.suretyStatus === suretyStatus,
        }
      )),
    };
    const t = template(handlebarData);
    this.container.empty().append(t);
    $('#lesseeId').select2({ placeholder: 'Select a company', width: '100%' });
    $('#contractId').select2({ placeholder: 'Select a contract', width: '100%' });
    this.datePickerMgr.init('issueDate', 'mm/dd/yy');
    $('#claimedIndToggle').bootstrapToggle({
      on: 'Yes',
      off: 'No',
      onstyle: 'secondary',
      offstyle: 'secondary',
    });
    this.suretyRiderMgr.render();
    this.suretyWellMgr.render();
    this.initClaimedIndToggleChange();
    this.initInsurerAutoComplete();
    this.initSuretyInputChange();
    this.initFormSubmit();
    formatReadOnlyNumberInputs('addUpdateSuretyContainer');
  }

  initClaimedIndToggleChange() {
    $('#claimedIndToggle').change((e) => {
      const { surety } = this.state;
      surety.claimedInd = $(e.target).prop('checked');
      this.setState({ ...this.state, surety });
    });
  }

  initInsurerAutoComplete() {
    const { insurers } = this.state;
    $('#insurer')
      .autocomplete(
        {
          source: insurers,
          minLength: 0,
          select: (event, ui) => {
            this.setState({
              ...this.state,
              surety: {
                ...this.state.surety,
                insurer: ui.item.label,
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

  initAddNewSuretyBtnClick() {
    this.container.on('click', '.addNewSuretyBtn', () => {
      this.init(0);
    });
  }

  initEditBtnClick() {
    this.container.on('click', '.editSuretyBtn', () => {
      this.setState({ ...this.state, editmode: true });
      this.render();
    });
  }

  initCancelChangesClick() {
    this.container.on('click', '#cancelSuretyChangesBtn', () => {
      this.init(this.id, this.wellId, this.contractId);
    });
  }


  initFormSubmit() {
    // eslint-disable-next-line consistent-return
    $('#addUpdateSuretyForm').submit(() => {
      const {
        surety, suretyRiders, suretyWells,
      } = this.state;

      this.setState({ ...this.state, submitInd: true });
      let formHasError = false;

      if (suretyWells.length !== suretyWells.filter((x) => x.wellId).length) {
        window.Toastr.error('Please select a well');
        return false;
      }

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

      const newSurety = {
        ...surety,
        suretyTypeId: parseInt(surety.suretyTypeId, 10),
        bondCategoryId: parseInt(surety.bondCategoryId, 10),
        lesseeId: parseInt(surety.lesseeId, 10),
        contractId: surety.contractId ? parseInt(surety.contractId, 10) : null,
        initialSuretyValue: surety.initialSuretyValue ? parseFloat(surety.initialSuretyValue) : null,
        currentSuretyValue: surety.currentSuretyValue ? parseFloat(surety.currentSuretyValue) : null,
        releasedSuretyValue: surety.releasedSuretyValue ? parseFloat(surety.releasedSuretyValue) : null,
        claimedInd: surety.claimedInd,
        suretyRiders,
        suretyWells,
      };

      $('.suretySubmitbtn').hide();
      $('.suretySubmitBtnDisabled').show();

      AjaxService.ajaxPost('./api/SuretyMgrApi', newSurety)
        .then((d) => {
          window.Toastr.success('Your surety has been saved', 'Save Successful', { positionClass: 'toast-top-center' });
          this.manageSuretiesMgr.init();
          this.init(d.id, this.wellId, this.contractId);
        })
        .catch(() => {
          $('.suretySubmitbtn').show();
          $('.suretySubmitBtnDisabled').hide();
        });
    });
  }
}
