import initCardHeader from '../initCardHeader';
import AjaxService from '../../services/ajaxService';
import initState from './initState';
import DatePickerMgr from './datePickerMgr';
import initTractsDataTable from './initTractsDataTable';
import formatReadOnlyNumberInputs from '../../services/formatReadOnlyNumberInputs';
import elementsToBeValidated from './elementsToBeValidated';

const template = require('../../views/contracts/manageTracts/index.handlebars');
const tractsTableTemplate = require('../../views/contracts/manageTracts/tractsTable.handlebars');
const tractFormTemplate = require('../../views/contracts/manageTracts/tractForm.handlebars');
const addTractButtonTemplate = require('../../views/contracts/manageTracts/addTractButton.handlebars');

export default class ManageTractsMgr {
  constructor(options) {
    this.sideBarMgr = options.sideBarMgr;
    this.container = $('#manageTractsContainer');
    this.tractstablecontainer = 'tractstablecontainer';
    this.tractformcontainer = 'tractformcontainer';
    this.tractstable = null;
  }

    init(tractNum) {
      this.state = initState();
        this.setState({ ...this.state, tractNum: tractNum || '', addCloseButton: tractNum });
    this.initMgrs();
    if (this.tractsTable) {
      this.tractstable.destroy();
      this.tractstable = null;
    }
    this.getDistrictData();
    this.render();
  }

  initMgrs() {
    this.datePickerMgr = new DatePickerMgr(this);
  }

  setState(state) {
    this.state = state;
  }

  render() {
    const t = template({ headerInfo: initCardHeader() });
    this.container.empty().append(t);
    this.getTractsData();
    this.initCancelTractChangesBtn();
  }

  initCancelTractChangesBtn() {
    $('#cancelTractChangesBtn').click(() => {
      this.getTractData(this.state.tractId);
    });
  }

  renderTractForm() {
    const {
      tract, editmode, districts, districtTractJunctions,
    } = this.state;
    const handleBarData = {
      tract,
      editmode,
      districts: districts.map(
        (district) => (
          {
            ...district,
            selected: districtTractJunctions.map((x) => x.districtId).includes(district.id),
          }
        ),
      ),
    };
    const t = tractFormTemplate(handleBarData);
    $(`#${this.tractformcontainer}`).empty().append(t);
    $('#administrativeIndToggle, #terminatedIndToggle').bootstrapToggle({
      on: 'Yes',
      off: 'No',
      onstyle: 'secondary',
      offstyle: 'secondary',
    });
    this.datePickerMgr.init('terminatedDate', 'mm/dd/yy');
    this.datePickerMgr.init('reversionDate', 'mm/dd/yy');
    $('#districttracts').select2({ placeholder: 'Select all that apply', width: '100%' });
    this.initDistrictTractsSelect();
    this.initEditTractBtnClick();
    this.initAdministrativeIndToggleChange();
    this.initTerminatedIndToggleChange();
    this.initAcreageChange();
    this.initAltTractNumChange();
    this.initTractNumChange();
    this.initFormSubmit();
    formatReadOnlyNumberInputs(this.tractformcontainer);
    $('html, body').animate({ scrollTop: $(document).height() }, 'slow');
  }

  initDistrictTractsSelect() {
    $('#districttracts').on('change', () => {
      const { tract } = this.state;
      const districtTractJunctions = $('#districttracts').select2('data').map(
        (x) => (
          {
            id: 0,
            tractId: tract.id,
            districtId: parseInt(x.id, 10),
          }
        ),
      );
      this.setState(
        { ...this.state, districtTractJunctions },
      );
    });
  }


  initEditTractBtnClick() {
    $('.editTractBtn').click(() => {
      this.setState({ ...this.state, editmode: true });
      this.renderTractForm();
    });
  }

  initAcreageChange() {
    $('#tractAcreage').change((e) => {
      const { tract } = this.state;
      tract.acreage = parseFloat($(e.target).val());
      this.setState({ ...this.state, tract });
      this.doesElementHaveError('tractAcreage');
    });
    }

    initAltTractNumChange() {
        $('#altTractNum').change((e) => {
        const { tract } = this.state;
        tract.altTractNum =$(e.target).val();
        this.setState({ ...this.state, tract });
        });
    }

  initTractNumChange() {
    $('#tractTractNum').change((e) => {
      const { tract } = this.state;
      tract.tractNum = $(e.target).val();
      this.setState({ ...this.state, tract });
      this.doesElementHaveError('tractTractNum');
    });
  }

  initAdministrativeIndToggleChange() {
    $('#administrativeIndToggle').change((e) => {
      const { tract } = this.state;
      tract.administrative = $(e.target).prop('checked');
      this.setState({ ...this.state, tract });
    });
  }

  initTerminatedIndToggleChange() {
    $('#terminatedIndToggle').change((e) => {
      const { tract } = this.state;
      tract.terminated = $(e.target).prop('checked');
      this.setState({ ...this.state, tract });
    });
  }


  getTractsData() {
    $('.spinner').show();
    AjaxService.ajaxGet('./api/TractMgrApi')
      .then((tracts) => {
        this.setState({ ...this.state, tracts });
        this.renderTractsTable();
        $('.spinner').hide();
      });
  }

  getDistrictData() {
    AjaxService.ajaxGet('./api/DistrictMgrApi')
      .then((d) => {
        function compare(a, b) {
          // Use toUpperCase() to ignore character casing
          const nameA = a.districtId;
          const nameB = b.districtId;

          let comparison = 0;
          if (nameA > nameB) {
            comparison = 1;
          } else if (nameA < nameB) {
            comparison = -1;
          }
          return comparison;
        }
        const sorteddistricts = d.sort(compare);
        const districts = sorteddistricts.map(
          (district) => (
            {
              ...district,
              name: `(${district.districtId.toString().padStart(2, '0')}) ${district.name}`,
            }
          ),
        );
        this.setState({
          ...this.state, districts,
        });
      });
  }


  getTractData(id) {
    $('.spinner').show();
    AjaxService.ajaxGet(`./api/TractMgrApi/${id}`)
      .then((d) => {
        if (d) {
          this.setState({
            ...this.state,
            editmode: false,
            tractId: d.id,
            submitInd: false,
            tract: {
              ...d,
              terminatedDate: d.terminatedDate ? new Date(d.terminatedDate) : null,
              reversionDate: d.reversionDate ? new Date(d.reversionDate) : null,
            },
          });
          AjaxService.ajaxGet(`./api/DistrictTractJunctionMgrApi/junctionsbytract/${id}`)
            .then((districtTractJunctions) => {
              this.setState({
                ...this.state,
                districtTractJunctions: districtTractJunctions || [],
              });
              this.renderTractForm();
              $('.spinner').hide();
            });
        } else {
          this.setState({
            ...this.state,
            editmode: true,
            tractId: 0,
            submitInd: false,
            districtTractJunctions: [],
            tract: { id: 0, terminatedDate: null, reversionDate: null },
          });
          this.renderTractForm();
          $('.spinner').hide();
        }
      });
  }

  renderTractsTable() {
      const { tracts, tractNum } = this.state;
    const tabledata = tracts.map(
      (tract) => (
        {
          ...tract,
          tractNum: tract.administrative ? `${tract.tractNum}(admin)` : tract.tractNum,
        }
      ),
    );
    const tractstabletemplate = tractsTableTemplate();
    $(`#${this.tractstablecontainer}`).empty().append(tractstabletemplate);
    this.tractstable = initTractsDataTable(tabledata);
    this.addTractButton();
    this.initAddTractBtnClick();
      this.initSelectClick();

      if (tractNum) {
          $('#tractsTable').DataTable().search(tractNum).draw();
          this.setState({ ...this.state, tractNum: '' });
          }
  }

  initSelectClick() {
    $('#tractsTable tbody').on('click', 'button', (event) => {
      const data = this.tractstable.row($(event.target).parents('tr')).data();
      this.getTractData(data.id);
    });
  }

  // eslint-disable-next-line class-methods-use-this
    addTractButton() {
    const { addCloseButton } = this.state;
    const isIE = window.navigator.userAgent.match(/MSIE|Trident/) !== null;
    const t = addTractButtonTemplate({ isIE, addCloseButton });
    $('#tractsTable_wrapper .dt-buttons').prepend(t);
    this.initBackBtnClick();
    }

    initBackBtnClick() {
        $('#backBtn').click(() => {
            window.close('', '_parent', '');
        });
    }

  initAddTractBtnClick() {
    $('#addTractBtn').click(() => {
      this.getTractData(0);
    });
  }

  doesElementHaveError(element) {
    const { submitInd, tracts, tract } = this.state;
    const { showOnlyIfFormSubmitted } = elementsToBeValidated
      .find((x) => x.element === element).showOnlyIfFormSubmitted;
    if (!$(`#${element}`).length) {
      return false;
    }
    $(`#${element}Error`).hide();
    $('#duplicateTractError').hide();
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
    if (!tract.id && tract.tractNum && tracts.find((x) => x.tractNum === tract.tractNum)) {
      $('#tractTractNum').addClass('is-invalid');
      $('#duplicateTractError').show();
      return true;
    }
    return false;
  }

  initFormSubmit() {
    // eslint-disable-next-line consistent-return
    $('#addUpdateTractForm').submit(() => {
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
      $('#tractSubmitbtn').hide();
      $('#tractSubmitBtnDisabled').show();
      const { tract, districtTractJunctions, addCloseButton } = this.state;
      const newTract = {
        ...tract,
        districtTracts: districtTractJunctions,
      };
      AjaxService.ajaxPost('./api/TractMgrApi', newTract)
        .then((d) => {
          window.Toastr.success('Your tract has been saved', 'Save Successful', { positionClass: 'toast-top-center' });
          this.tractstable.destroy();
          this.tractstable = null;
          this.getTractsData();
          this.getTractData(d.id);
          this.setState({ ...this.state, addCloseButton});
        })
        .catch(() => {
          $('#tractSubmitbtn').show();
          $('#tractSubmitbtn').hide();
        });
    });
  }
}
