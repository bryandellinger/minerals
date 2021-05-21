/* eslint-disable max-len */
import AjaxService from '../../services/ajaxService';
import elementsToBeValidated from './elementsToBeValidated';

export default class checkNumberMgr {
  constructor(AddUpdateUploadPaymentMgr) {
    this.addUpdateUploadPaymentMgr = AddUpdateUploadPaymentMgr;
  }

  init() {
    const { check } = this.addUpdateUploadPaymentMgr.state;
    $('#uploadPaymentCheckNum').select2({
      placeholder: 'Select a Check Number',
      minimumInputLength: 2,
      ajax: {
        url: './api/CheckMgrApi/GetSelect2Data',
        contentType: 'application/json; charset=utf-8',
        data(params) {
          const query = {
            id: params.term,
          };
          return query;
        },
        processResults: (results) => ({
          results,
        }),
      },
    });
    if (check && check.id) {
      const data = {
        id: check.id,
        text: check.checkNum,
      };
      if ($('#uploadPaymentCheckNum').find(`option[value='${data.id}']`).length) {
        $('#uploadPaymentCheckNum').val(data.id).trigger('change');
      } else {
        // Create a DOM Option and pre-select by default
        const newOption = new Option(data.text, data.id, true, true);
        // Append it to the select
        $('#uploadPaymentCheckNum').append(newOption).trigger('change');
      }
    }
  }

  initCheckNumChange() {
    $('#uploadPaymentCheckNum').on('select2:select', (e) => {
      this.processCheckNumChange(e.params.data.id);
    });
  }

  processCheckNumChange(checkId) {
    $('.spinner').show();
    AjaxService.ajaxGet(`./api/CheckMgrApi/${checkId}`)
      .then((check) => {
        if (check && check.id) {
          const { uploadPayment } = this.addUpdateUploadPaymentMgr.state;
          this.addUpdateUploadPaymentMgr.setState({
            ...this.addUpdateUploadPaymentMgr.state,
            check: {
              ...check,
              checkDate: check.checkDate ? new Date(check.checkDate).toLocaleDateString() : null,
              receivedDate: check.receivedDate ? new Date(check.receivedDate).toLocaleDateString() : null,
            },
            uploadPayment: {
              ...uploadPayment,
              checkId: check.id,
            },
          });

          AjaxService.ajaxGet(`./api/LesseeMgrApi/${check.lesseeId}`)
            .then((lessee) => {
              this.addUpdateUploadPaymentMgr.setState({
                ...this.addUpdateUploadPaymentMgr.state, lessee,
              });
              this.addUpdateUploadPaymentMgr.render();
              $('.spinner').hide();
            });
        }
      });
  }

  getCheckFromCSV() {
    const { checkNoFromCSV } = this.addUpdateUploadPaymentMgr.state;
    AjaxService.ajaxGet(`./api/CheckMgrApi/GetCheckByCheckNum/${checkNoFromCSV}`)
      .then((check) => {
        if (check && check.id) {
          this.processCheckNumChange(check.id);
        } else {
          this.addUpdateUploadPaymentMgr.setState({
            ...this.addUpdateUploadPaymentMgr.state,
            showGenerateCheckInd: true,
          });
          this.addUpdateUploadPaymentMgr.render();
        }
      });
  }

  initCreateCheckBtnClick() {
    $('#createCheckBtn').click(() => {
      const {
        checkNoFromCSV, files, uploadPayment, checkDate,
      } = this.addUpdateUploadPaymentMgr.state;

      this.addUpdateUploadPaymentMgr.setState({ ...this.addUpdateUploadPaymentMgr.state, submitInd: true });
      let formHasError = false;

      // eslint-disable-next-line no-restricted-syntax
      for (const item of elementsToBeValidated) {
        if (this.addUpdateUploadPaymentMgr.doesElementHaveError(item.element)) {
          formHasError = true;
        }
      }

      if (formHasError) {
        window.Toastr.error('Please fix errors and resubmit');
        return false;
      }

      if (!(files.length)) {
        window.Toastr.error('Please upload the csv payment file');
        return false;
      }

      const data = {
        checkDate,
        checkNum: checkNoFromCSV,
        fileId: parseInt(files[0].id, 10),
        uploadTemplateId: parseInt(uploadPayment.uploadTemplateId, 10),
      };

      $('.uploadPaymentSubmitbtn').hide();
      $('.uploadPaymentSubmitBtnDisabled').show();
      $('.spinner').show();
      AjaxService.ajaxPost('./api/CreateCheckMgrApi', data)
        .then((d) => {
          window.Toastr.success(`Check ${checkNoFromCSV} had been created`, 'Check Created', { positionClass: 'toast-top-center' });
          this.addUpdateUploadPaymentMgr.setState(
            {
              ...this.addUpdateUploadPaymentMgr.state,
              uploadPayment: {
                ...this.addUpdateUploadPaymentMgr.state.uploadPayment,
                checkId: d.id,
              },
            },
          );
          this.addUpdateUploadPaymentMgr.submitForm();
        })
        .catch(() => {
          $('.uploadPaymentSubmitbtn').show();
          $('.uploadPaymentSubmitBtnDisabled').hide();
          $('.spinner').hide();
        });
    });
  }
}
