/* eslint-disable max-len */
import AjaxService from '../../services/ajaxService';

export default class checkNumberMgr {
  constructor(AddUpdateOtherPaymentMgr) {
      this.addUpdateOtherPaymentMgr = AddUpdateOtherPaymentMgr;
  }

  init() {
      const { check } = this.addUpdateOtherPaymentMgr.state;
      $('#otherPaymentCheckNum').select2({
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
        if ($('#otherPaymentCheckNum').find(`option[value='${data.id}']`).length) {
            $('#otherPaymentCheckNum').val(data.id).trigger('change');
      } else {
        // Create a DOM Option and pre-select by default
        const newOption = new Option(data.text, data.id, true, true);
        // Append it to the select
            $('#otherPaymentCheckNum').append(newOption).trigger('change');
      }
    }
  }

  initCheckNumChange() {
      $('#otherPaymentCheckNum').on('select2:select', (e) => {
      AjaxService.ajaxGet(`./api/CheckMgrApi/${e.params.data.id}`)
        .then((check) => {
          if (check && check.id) {
              const { otherPayment } = this.addUpdateOtherPaymentMgr.state;
              this.addUpdateOtherPaymentMgr.setState({
                  ...this.addUpdateOtherPaymentMgr.state,
              check: {
                ...check,
                checkDate: check.checkDate ? new Date(check.checkDate).toLocaleDateString() : null,
                receivedDate: check.receivedDate ? new Date(check.receivedDate).toLocaleDateString() : null,
              },
               otherPayment: {
                   ...otherPayment,
                checkId: check.id,
              },
            });

            AjaxService.ajaxGet(`./api/LesseeMgrApi/${check.lesseeId}`)
              .then((lessee) => {
                  this.addUpdateOtherPaymentMgr.setState({
                      ...this.addUpdateOtherPaymentMgr.state, lessee,
                  });
                  this.addUpdateOtherPaymentMgr.render();
              });
          }
        });
    });
  }
}
