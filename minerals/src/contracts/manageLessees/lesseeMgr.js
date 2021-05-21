import AjaxService from '../../services/ajaxService';
import initLesseeHistoryDataTable from './initLesseeHistoryDataTable';
import initState from './initState';

const template = require('../../views/contracts/manageLessees/addUpdateLessee.handlebars');
const addLesseeButtonTemplate = require('../../views/contracts/manageLessees/addLesseeButton.handlebars');
const historyFormTemplate = require('../../views/contracts/manageLessees/historyForm.handlebars');

export default class LesseeMgr {
  constructor(manageLeseesMgr) {
    this.manageLeseesMgr = manageLeseesMgr;
    this.container = 'editAddLesseeContainer';
    this.historyFormContainer = 'historyFormContainer';
    this.lesseeHistoryTable = null;
  }

  // eslint-disable-next-line class-methods-use-this
    addLesseeButton() {
        const { addCloseButton } = this.manageLeseesMgr.state;
      const isIE = window.navigator.userAgent.match(/MSIE|Trident/) !== null;
        const t = addLesseeButtonTemplate({ isIE, addCloseButton });
        $('#lesseeTable_wrapper .dt-buttons').prepend(t);
        this.initBackBtnClick();

  }

    initBackBtnClick() {
        $('#lesseeBackBtn').click(() => {
            window.close('', '_parent', '');
        });
    }

  initAddLesseeClick() {
    $('#addLesseeBtn').click(() => {
      $(`#${this.manageLeseesMgr.lesseetablecontainer}`).empty();
      $(`#${this.manageLeseesMgr.lesseetabcontainer}`).empty();
      this.manageLeseesMgr.state = initState();
      this.manageLeseesMgr.setState({ ...this.manageLeseesMgr.state, lessee: { id: 0 } });
      this.render();
    });
  }

  render() {
    const t = template();
    $(`#${this.container}`).empty().append(t);
    this.initCancelBtnClick();
    this.initFormSubmit();
  }

  renderHistoryForm() {
    const { lessee, historyFormEditMode, legalNameChangeInd } = this.manageLeseesMgr.state;
    const data = { lessee, legalNameChangeInd, editmode: historyFormEditMode };
    const t = historyFormTemplate(data);
    $(`#${this.historyFormContainer}`).empty().append(t);
    if (this.lesseeHistoryTable) {
      this.lesseeHistoryTable.destroy();
    }
    this.getLesseeHistoryData();
    this.initCloseBtnClick();
    this.initEditBtnClick();
    this.initLegalNameChangeIndClick();
    this.initFormSubmit();
  }

  getLesseeHistoryData() {
    const { lessee } = this.manageLeseesMgr.state;
    $('.spinner').show();
    AjaxService.ajaxGet(`./api/LesseeHistoryMgrApi/lesseehistorybylessee/${lessee.id}`)
      .then((d) => {
        const tableData = d.map(
          (item) => (
            {
              ...item,
              tableDate: item.createDate ? new Date(item.createDate) : null,
            }
          ),
        );
        this.lesseeHistoryTable = initLesseeHistoryDataTable(tableData);
        $('.spinner').hide();
      });
  }

  initLegalNameChangeIndClick() {
    $('#legalNameChangeInd').change((e) => {
      this.manageLeseesMgr.setState(
        {
          ...this.manageLeseesMgr.state,
          legalNameChangeInd: $(e.target).prop('checked'),
        },
      );
    });
  }

  initEditBtnClick() {
    $('#editHistoryForm').click(() => {
      this.manageLeseesMgr.setState({ ...this.manageLeseesMgr.state, historyFormEditMode: true });
      this.renderHistoryForm();
    });
  }


  initCancelBtnClick() {
    $('#cancelAddLesseeBtn').click(() => {
      this.manageLeseesMgr.init();
    });
  }

  initCloseBtnClick() {
    $('#closeHistoryForm, #cancelHistoryForm').click(() => {
      const { lessee } = this.manageLeseesMgr.state;
      this.manageLeseesMgr.getLesseeContactData(lessee);
    });
  }

  // eslint-disable-next-line class-methods-use-this
  initFormSubmit() {
    // eslint-disable-next-line consistent-return
    $('#addNewLesseeForm, #updateLesseeForm').submit(() => {
      if (!$('#lesseeName')[0].checkValidity()) {
        $('#lesseeName').addClass('is-invalid');
        $('#lesseeNameError').show();
        return false;
      }
      $('.spinner').show();
        const { lessee, legalNameChangeInd, addCloseButton } = this.manageLeseesMgr.state;
      if (lessee && lessee.id) {
        const putData = {
          id: lessee.id,
          lesseeName: $('#lesseeName').val(),
          addHistoryInd: legalNameChangeInd,
          logicalDeleteIn: false,
        };
          AjaxService.ajaxPut(`./api/LesseeMgrApi/${lessee.id}`, putData)
              .then((data) => {
                  window.Toastr.success(`Lessee ${$('#lesseeName').val()} has been updated`, 'Update Successful', { positionClass: 'toast-top-center' });
                  $('.spinner').hide();
                  this.lesseeHistoryTable = null;
                  this.manageLeseesMgr.init(data);
                  this.manageLeseesMgr.setState({ ...this.manageLeseesMgr.state, addCloseButton });
          })
          .catch(() => {
            $('.spinner').hide();
          });
      } else {
        AjaxService.ajaxPost('./api/LesseeMgrApi', { id: 0, lesseeName: $('#lesseeName').val(), logicalDeleteIn: false })
          .then((data) => {
            window.Toastr.success(`Lessee ${$('#lesseeName').val()} has been added`, 'Save Successful', { positionClass: 'toast-top-center' });
            $('.spinner').hide();
            this.lesseeHistoryTable = null;
            this.manageLeseesMgr.init(data);
            this.manageLeseesMgr.setState({ ...this.manageLeseesMgr.state, addCloseButton });
          })
          .catch(() => {
            $('.spinner').hide();
          });
      }
    });
  }
}
