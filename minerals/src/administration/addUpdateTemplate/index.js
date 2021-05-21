/* eslint-disable no-nested-ternary */
/* eslint-disable max-len */
/* eslint-disable no-undef */
import DropZone from 'dropzone';
import DropzoneMgr from './dropzoneMgr';
import CsvToRoyaltyPaymentMgr from './csvToRoyaltyPaymentMgr';
import initState from './initState';
import initCardHeader from '../initCardHeader';
import formatReadOnlyNumberInputs from '../../services/formatReadOnlyNumberInputs';
import AjaxService from '../../services/ajaxService';
import elementsToBeValidated from './elementsToBeValidated';
import royaltyMappingColumns from './royaltyMappingColumns';

const template = require('../../views/administration/addUpdateTemplate/index.handlebars');

export default class AddUpdateTemplateMgr {
  constructor(sideBarMgr) {
    this.container = $('#addUpdateTemplateContainer');
    this.sideBarMgr = sideBarMgr;
    this.dropzoneMgr = null;
    this.csvToRoyaltyPaymentMgr = null;
    this.initSideBarButtonClick();
    this.initScrollToBottom();
    this.initScrollToTop();
    this.id = 0;
    this.uploadPaymentId = 0;
    this.manageTemplatesMgr = null;
  }

  setState(state) {
    this.state = state;
  }

    init(id, uploadPaymentId) {
    this.id = id || 0;
    this.uploadPaymentId = uploadPaymentId || 0;
    this.state = initState();
    this.setState({ ...this.state, editmode: !id });
    this.initEditBtnClick();
    this.initCancelChangesClick();
    this.initAddNewTemplateBtnClick();
    this.initMgrs();
    this.getData(id);
  }

  initMgrs() {
    this.dropzoneMgr = new DropzoneMgr(this);
    this.csvToRoyaltyPaymentMgr = new CsvToRoyaltyPaymentMgr(this);
  }

  addManageTemplatesMgr(manageTemplatesMgr) {
    this.manageTemplatesMgr = manageTemplatesMgr;
  }

  getData(id) {
    $('.spinner').show();
    $.when(
      $.get(`./api/UploadTemplateMgrApi/${id}`, (uploadTemplate) => {
        if (uploadTemplate && uploadTemplate.id) {
          this.setState({
            ...this.state,
            uploadTemplate: {
              ...uploadTemplate,
            },
          });
        }
      }),
      $.get(`./api/UploadTemplateMgrApi/GetMappedHeaders/${id}`, (mappedHeaders) => {
        if (mappedHeaders && mappedHeaders.length) {
          this.setState({
            ...this.state,
            mappedHeaders,
          });
        } else {
          this.setState({
            ...this.state,
            mappedHeaders: royaltyMappingColumns,
          });
        }
      }),
      $.get(`./api/UploadTemplateMgrApi/GetUnmappedHeaders/${id}`, (unmappedHeaders) => {
        if (unmappedHeaders && unmappedHeaders.length) {
          this.setState(
            { ...this.state, unmappedHeaders },
          );
        }
      }),
      $.get(`./api/FileMgrApi/filesbytemplate/${id}`, (d) => {
        this.setState(
          { ...this.state, files: d || [] },
        );
      }),
      $.get('./api/LesseeMgrApi', (lessees) => {
        this.setState({ ...this.state, lessees });
      }),
    ).then(() => {
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

  initTemplateInputChange() {
    $('.templateInput').change((e) => {
      const { uploadTemplate } = this.state;
      const element = $(e.target).attr('name');
      uploadTemplate[element] = $(e.target).val();
      this.setState({ ...this.state, uploadTemplate });
      if (elementsToBeValidated.find((x) => x.element === element)) {
        this.doesElementHaveError(element);
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
      editmode, uploadTemplate, lessees,
    } = this.state;
    const handlebarData = {
      headerInfo: initCardHeader(),
        editmode,
      uploadPaymentId: this.uploadPaymentId,
      uploadTemplate,
      lessees: lessees.map(
        (lessee) => (
          {
            ...lessee,
            selected: lessee.id === uploadTemplate.lesseeId,
          }
        ),
      ),
    };
    const t = template(handlebarData);
    this.container.empty().append(t);
    $('#lesseeId').select2({ placeholder: 'Select a company', width: '100%' });
    this.initTemplateInputChange();
    this.dropzoneMgr.init();
    this.csvToRoyaltyPaymentMgr.init();
    this.initFormSubmit();
    formatReadOnlyNumberInputs('addUpdateOtherPaymentContainer');
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

  initAddNewOtherTemplateButtonClick() {
    this.container.on('click', '.addNewTemplate', () => {
      this.init(0, this.uploadPaymentId);
    });
  }

  initEditBtnClick() {
    this.container.on('click', '.editTemplateBtn', () => {
      this.setState({ ...this.state, editmode: true });
      this.render();
    });
  }

  initCancelChangesClick() {
    this.container.on('click', '#cancelTemplateChangesBtn', () => {
      this.init(this.id, this.uploadPaymentId);
    });
  }

  initAddNewTemplateBtnClick() {
    this.container.on('click', '.addNewTemplateBtn', () => {
      this.init(0);
    });
  }

  initFormSubmit() {
    // eslint-disable-next-line consistent-return
    $('#addUpdateTemplateForm').submit(() => {
      this.csvToRoyaltyPaymentMgr.getUnmappedHeaders();
      this.csvToRoyaltyPaymentMgr.getMappedHeaders();
      const {
        uploadTemplate, files, mappedHeaders, unmappedHeaders,
      } = this.state;

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

      const newUploadTemplate = {
        ...uploadTemplate,
        lesseeId: parseInt(uploadTemplate.lesseeId, 10),
        files,
        uploadTemplateMappedHeaders: mappedHeaders,
        uploadTemplateUnmappedHeaders: unmappedHeaders,
      };

      $('.templateSubmitbtn').hide();
      $('.templateSubmitBtnDisabled').show();

      AjaxService.ajaxPost('./api/UploadTemplateMgrApi', newUploadTemplate)
        .then((d) => {
          window.Toastr.success('Your template has been saved', 'Save Successful', { positionClass: 'toast-top-center' });
          this.manageTemplatesMgr.init();
          this.init(d.id, this.uploadPaymentId);
        })
        .catch(() => {
          $('.templateSubmitbtn').show();
          $('.templateSubmitBtnDisabled').hide();
        });
    });
  }
}
