/* eslint-disable no-undef */
import DropZone from 'dropzone';
import initState from './initState';
import initCardHeader from '../initCardHeader';
import elementsToBeValidated from './elementsToBeValidated';
import AjaxService from '../../services/ajaxService';
import formatReadOnlyNumberInputs from '../../services/formatReadOnlyNumberInputs';
import WellTractTableMgr from './wellTractTableMgr';
import DatePickerMgr from './datePickerMgr';
import * as constants from '../../constants';


const template = require('../../views/units/addUpdateUnit/index.handlebars');

export default class AddUpdateUnitMgr {
  constructor(sideBarMgr) {
    this.container = $('#addUpdateUnitContainer');
    this.sideBarMgr = sideBarMgr;
    this.wellTractTableMgr = null;
    this.datePickerMgr = null;
    this.id = 0;
    this.state = null;
    this.manageUnitsMgr = null;
    this.initSideBarButtonClick();
    this.initScrollToBottom();
    this.initScrollToTop();
    this.initTabClick();
  }

  setState(state) {
    this.state = state;
  }

    init(id, wellId, paymentId) {
    this.id = id || 0;
    this.state = initState();
        this.setState({ ...this.state, editmode: !id, wellId: wellId || 0, paymentId: paymentId || 0});
    this.initEditBtnClick();
    this.initCancelChangesClick();
    this.initUnitInputChange();
    this.initMgrs();
    this.getData(id);
  }


  initMgrs() {
    this.wellTractTableMgr = new WellTractTableMgr(this);
    this.datePickerMgr = new DatePickerMgr(this);
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

  initTabClick() {
    this.container.on('click', '.unitTab', (e) => {
        const { wellId, paymentId } = this.state;
        this.init($(e.target).attr('data-unitId'), wellId, paymentId);
    });
  }

  initEditBtnClick() {
    this.container.on('click', '.editUnitBtn', () => {
      this.setState({ ...this.state, editmode: true });
      this.render();
    });
  }

  initCancelChangesClick() {
    this.container.on('click', '#cancelChangesBtn', () => {
        const { wellId, paymentId } = this.state;
        this.init(this.id, wellId, paymentId);
    });
  }

  getData(id) {
    $('.spinner').show();
    AjaxService.ajaxGet(`./api/UnitMgrApi/unitGroupInfoByUnit/${id}`)
      .then((unitGroups) => {
        this.setState(
          { ...this.state, unitGroups },
        );
        this.getUnitData(id);
      });
  }

  getUnitData(id) {
    $.when(
      $.get(`./api/UnitMgrApi/${id}`, (d) => {
        this.setState(
          {
            ...this.state,
            unit: d ? {
              ...d,
              dpuAcresEffectiveDate:
                                d.dpuAcresEffectiveDate ? new Date(d.dpuAcresEffectiveDate) : null,
            } : { id: 0, dpuAcresEffectiveDate: null },
          },
        );
      }),
      $.get('./api/tractMgrApi', (tracts) => {
        this.setState({ ...this.state, tracts });
      }),
      $.get(`./api/TractUnitJunctionMgrApi/junctionsbyunit/${id}`, (d) => {
        this.setState(
          { ...this.state, tractUnitJunctions: d || [] },
        );
      }),
      $.get(`./api/FileMgrApi/filesbyunit/${id}`, (d) => {
        this.setState(
          { ...this.state, files: d || [] },
        );
      }),
      $.get('./api/wellMgrApi', (wells) => {
        this.setState({ ...this.state, wells });
      }),
      $.get('./api/wellStatusApi', (wellStatuses) => {
        this.setState({ ...this.state, wellStatuses });
      }),
    ).then(() => {
      this.render();
      $('.spinner').hide();
    });
  }

  addManageUnitsMgr(manageUnitsMgr) {
    this.manageUnitsMgr = manageUnitsMgr;
  }

  initSideBarButtonClick() {
    $('.sidebar').on('click', '.spa-btn', () => {
      this.init(0);
    });
  }

  render() {
    const {
      editmode,
      unit,
      wellId,
      paymentId,
      unitGroups,
      amendmentInd,
    } = this.state;

    const baseUrl = window.location.href.split('?')[0];
    const wellsUrl = baseUrl.replace('Units', 'Wells');

    const handlebarData = {
      headerInfo: initCardHeader(),
      editmode,
      baseUrl,
      unit,
      wellId,
      paymentId,
      wellsUrl,
      amendmentInd,
      showTabsInd: unitGroups.length > 1,
      unitGroups: unitGroups.map(
        (unitGroup) => (
          {
            ...unitGroup,
            selected: unitGroup.id === unit.id,
          }
        ),
      ),
    };

    const t = template(handlebarData);
    this.container.empty().append(t);

    $('#amendmentIndToggle').bootstrapToggle({
      on: 'Amendment',
      off: 'Correction',
      onstyle: 'secondary',
      offstyle: 'secondary',
    });

    Dropzone.options.uploader = {
      paramName: 'file',
    };

    const self = this;
    $('#uploadDocuments').dropzone({
      url: './File/UploadFile',
      maxFiles: editmode ? 10 : 0,
      init: function init() {
        const { files } = self.state;
        files.forEach((file) => {
          const mockFile = { name: file.fileName, size: file.fileSize };
          this.displayExistingFile(mockFile, `./images/${file.fileIcon}`);
          self.addTitle(mockFile);
        });
        this.on('thumbnail', (f) => {
          f.previewElement.addEventListener('click', () => {
            const file = self.state.files.find((x) => x.fileName === f.name);
            window.location.href = `./File/DownloadFile/${file.id}`;
          });
        });
        this.on('maxfilesexceeded', function (file) {
          this.removeFile(file);
          window.Toastr.warning('please put unit in edit mode before uploading files');
        });
      },
      success: (file, response) => {
        const { files } = this.state;
        files.push(response);
        this.setState({ ...this.state, files });
      },
      addRemoveLinks: editmode,

      removedfile: (file) => {
        const { files } = self.state;
        for (let i = files.length - 1; i >= 0; i--) {
          if (files[i].fileName === file.name) {
            files.splice(i, 1);
          }
        }
        self.setState({ ...this.state, files });
        let _ref;
        return (_ref = file.previewElement) != null ? _ref.parentNode.removeChild(file.previewElement) : void 0;
      },

    });


    this.initAmendmentIndToggleChange();
    this.initFormSubmit();
    this.calculatePrivateAcreage();
    this.datePickerMgr.init('dpuAcresEffectiveDate', 'mm/dd/yy');
    this.wellTractTableMgr.render();
    formatReadOnlyNumberInputs('addUpdateUnitContainer');
  }


  addTitle(file) {
    let preview = document.getElementsByClassName('dz-preview');
    preview = preview[preview.length - 1];
    const imageName = document.createElement('small');
    imageName.innerHTML = file.name;
    preview.insertBefore(imageName, preview.firstChild);
  }


  initAmendmentIndToggleChange() {
    $('#amendmentIndToggle').change((e) => {
      this.setState({ ...this.state, amendmentInd: $(e.target).prop('checked') });
      $('#amendmentName').prop('required', $(e.target).prop('checked'));
    });
  }

  calculatePrivateAcreage() {
    const {
      unit, tractUnitJunctions, wells, wellStatuses,
    } = this.state;
    const producingStatus = wellStatuses.find((x) => x.wellStatusName === constants.WellStatusProducing);
    const nonProducingWells = wells.filter((x) => x.wellStatusId !== producingStatus.id);
    const nonProducingWellIds = nonProducingWells.map((x) => x.id);
    const totalCopAcres = tractUnitJunctions.map((x) => x.copAcres || 0).reduce((a, b) => a + b, 0);
    const dpuAcres = unit.dpuAcres ? parseFloat(unit.dpuAcres) : 0;
    const privateAcres = dpuAcres - totalCopAcres;
    const parts1 = privateAcres.toFixed(6).split('.');
    const parts2 = totalCopAcres.toFixed(6).split('.');
    parts1[0] = parts1[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
    parts2[0] = parts2[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
    $('#privateAcres').val(parts1.join('.'));
    $('#totalCopAcres').val(parts2.join('.'));
    tractUnitJunctions.forEach((element, index) => {
      $('.acreageAttributableToWells').eq(index).val('');
      if (tractUnitJunctions[index].wellIds.length) {
        // eslint-disable-next-line max-len
        const uniqueTractUnitJunctionWellIds = Array.from(new Set(tractUnitJunctions[index].wellIds));
        const uniqueProducingTractUnitJunctionWellIds = uniqueTractUnitJunctionWellIds.filter(
          (el) => !nonProducingWellIds.includes(el),
        );
        const acreageAttributableToWells = uniqueProducingTractUnitJunctionWellIds.length
          ? tractUnitJunctions[index].copAcres / uniqueProducingTractUnitJunctionWellIds.length
          : 0;
        const parts3 = acreageAttributableToWells.toFixed(6).split('.');
        parts3[0] = parts3[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        $('.acreageAttributableToWells').eq(index).val(parts3.join('.'));
      }
    });
  }

  initUnitInputChange() {
    this.container.on('change', '.unitinput', (e) => {
      const { unit } = this.state;
      const element = $(e.target).attr('id');
      unit[element] = $(e.target).val();
      this.setState({ ...this.state, unit });
      if (elementsToBeValidated.find((x) => x.element === element)) {
        this.doesElementHaveError(element);
      }
      this.calculatePrivateAcreage();
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
    $('#addUpdateUnitForm').submit(() => {
      const {
          unit, tractUnitJunctions, wellId, paymentId, amendmentInd, unitGroups, files,
      } = this.state;

      this.setState({ ...this.state, submitInd: true });
      let formHasError = false;

      tractUnitJunctions.forEach((element, i) => {
        const input = $('.copAcres').eq(i);
        const error = $('.copAcresError').eq(i);
        $('.tractIdError').eq(i).hide();
        $($('.tractId').eq(i).data('select2').$container).removeClass('is-invalid');
        if (!element.tractId) {
          formHasError = true;
          $('.tractIdError').eq(i).show();
          $($('.tractId').eq(i).data('select2').$container).addClass('is-invalid');
        }
        if (input[0].checkValidity()) {
          input.removeClass('is-invalid');
          error.hide();
        } else {
          input.addClass('is-invalid');
          error.show();
          formHasError = true;
        }
      });

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
      if (unit.isActiveInd
          && amendmentInd
          && unit.amendmentName
          && unitGroups
          && unitGroups.map((x) => x.amendmentName).includes(unit.amendmentName)
      ) {
        window.Toastr.error('Please create a new amendment name.', `${unit.amendmentName} already exists`);
        return false;
      }

      $('.unitSubmitbtn').hide();
      $('.unitSubmitBtnDisabled').show();

      const newUnit = {
        ...unit,
        amendmentInd,
        files,
        gisAcres: unit.gisAcres ? parseFloat(unit.gisAcres) : null,
        dpuAcres: unit.dpuAcres ? parseFloat(unit.dpuAcres) : null,
        tractUnitJunctions: tractUnitJunctions.map(
          (tractUnitJunction) => (
            {
              id: 0,
              tractId: tractUnitJunction.tractId,
              copAcres: tractUnitJunction.copAcres || 0,
              unitId: 0,
              tractUnitJunctionWellJunctions: tractUnitJunction.wellIds.map(
                (junction) => (
                  {
                    id: 0,
                    tractUnitJunctionId: 0,
                    wellId: junction,
                  }
                ),
              ),
            }
          ),
        ),
      };

      AjaxService.ajaxPost('./api/UnitMgrApi', newUnit)
        .then((d) => {
          window.Toastr.success('Your unit has been saved', 'Save Successful', { positionClass: 'toast-top-center' });
          this.manageUnitsMgr.init();
          this.init(d.id, wellId, paymentId);
        })
        .catch(() => {
          $('.unitSubmitbtn').show();
          $('.unitSubmitBtnDisabled').hide();
        });
    });
  }
}
