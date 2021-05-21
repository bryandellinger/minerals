/* eslint-disable consistent-return */
import initState from './initState';
import ApiNumMgr from './apiNumMgr';
import AlternateIdMgr from './alternateIdMgr';
import DatePickerMgr from './datePickerMgr';
import GetDataMgr from './getDataMgr';
import RenderMgr from './renderMgr';
import WellTractTableMgr from './wellTractTableMgr';
import HistoricalOwnershipMgr from './historicalOwnershipMgr';
import ValidateAndSubmitMgr from './validateAndSubmitMgr';
import NRIInformationMgr from './nriInformationMgr';
import TestLogMgr from './testLogMgr';
import elementsToBeValidated from './elementsToBeValidated';


require('select2');
require('select2/dist/css/select2.css');
require('bootstrap4-toggle');
require('bootstrap4-toggle/css/bootstrap4-toggle.min.css');

export default class AddUpdateWellMgr {
  constructor(sideBarMgr) {
    this.container = $('#addUpdateWellContainer');
    this.sideBarMgr = sideBarMgr;
    this.id = 0;
    this.state = null;
    this.apiNumMgr = null;
    this.alternateIdMgr = null;
    this.datePickerMgr = null;
    this.initSideBarButtonClick();
    this.manageWellsMgr = null;
    this.getDataMgr = null;
    this.renderMgr = null;
    this.wellTractTableMgr = null;
    this.historicalOwnershipMgr = null;
    this.validateAndSubmitMgr = null;
    this.testLogMgr = null;
    this.nriInformationMgr = null;
    this.initScrollToBottom();
    this.initScrollToTop();
    this.suretyTable = null;
  }

  setState(state) {
    this.state = state;
  }

    init(id, contractId, unitId, paymentId, suretyId) {
    this.id = id || 0;
    this.state = initState();
    this.initCancelChangesClick();
    this.initAddNewWellClick();
    this.setState({
        ...this.state, editmode: !id, contractId: contractId || 0, unitId: unitId || 0, paymentId: paymentId || 0, suretyId: suretyId || 0,
    });
    this.initMgrs();
        if (this.suretyTable) {
            this.suretyTable.destroy();
        }
    this.suretyTable= null;
    this.getDatamgr.getData(id);
    this.initEditBtnClick();
        this.initWellInputChange();

    this.renderMgr.render();
  }

  initMgrs() {
    this.apiNumMgr = new ApiNumMgr(this);
    this.alternateIdMgr = new AlternateIdMgr(this);
    this.datePickerMgr = new DatePickerMgr(this);
    this.getDatamgr = new GetDataMgr(this);
    this.testLogMgr = new TestLogMgr(this);
    this.renderMgr = new RenderMgr(this);
    this.validateAndSubmitMgr = new ValidateAndSubmitMgr(this);
    this.wellTractTableMgr = new WellTractTableMgr(this);
    this.historicalOwnershipMgr = new HistoricalOwnershipMgr(this);
    this.nriInformationMgr = new NRIInformationMgr(this);
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

  initprivatePropertyIndToggleChange() {
    $('#privatePropertyIndToggle').change((e) => {
      const { well } = this.state;
      well.privatePropertyInd = $(e.target).prop('checked');
      this.setState({ ...this.state, well });
    });
  }

  initautoUpdatedAllowedIndToggleChange() {
    $('#autoUpdatedAllowedInd').change((e) => {
      const { well } = this.state;
      well.autoUpdatedAllowedInd = $(e.target).prop('checked');
      this.setState({ ...this.state, well });
    });
  }


  initEditBtnClick() {
    this.container.on('click', '.editWellBtn', () => {
      this.setState({ ...this.state, editmode: true });
      this.renderMgr.render();
    });
  }

  initAddNewWellClick() {
    this.container.on('click', '.addNewWell', () => {
      this.init(0);
    });
  }

  initWellInputChange() {
    $('body').on('change', '.wellinput', (e) => {
      const { well } = this.state;
      const element = $(e.target).attr('id');
      well[element] = $(e.target).val();
      this.setState({ ...this.state, well });
      if (elementsToBeValidated.find((x) => x.element === element)) {
        this.doesElementHaveError(element);
      }
    });
  }

  initElevationTypeToggleChange() {
    $('#elevationTypeToggle').change((e) => {
      const { well } = this.state;
      well.belowGroundInd = $(e.target).prop('checked');
      this.setState({ ...this.state, well });
    });
  }

  initSideBarButtonClick() {
    $('.sidebar').on('click', '.spa-btn', () => {
      this.init(0);
    });
  }

  initCancelChangesClick() {
    this.container.on('click', '#cancelChangesBtn', () => {
      const { contractId, unitId, paymentId, suretyId, } = this.state;
      this.init(this.id, contractId, unitId, paymentId, suretyId);
    });
  }

  addManageWellsMgr(manageWellsMgr) {
    this.manageWellsMgr = manageWellsMgr;
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
}
