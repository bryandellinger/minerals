/* eslint-disable max-len */
import AjaxService from '../../services/ajaxService';

export default class checkNumberMgr {
  constructor(addUpdatePaymentMgr) {
    this.addUpdatePaymentMgr = addUpdatePaymentMgr;
  }

  init() {
    const { payment } = this.addUpdatePaymentMgr.state;
    $('#ajaxSelect2').select2({
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
    if (payment && payment.checkId) {
      const data = {
        id: payment.checkid,
        text: payment.checkNum,
      };
      if ($('#ajaxSelect2').find(`option[value='${data.id}']`).length) {
        $('#ajaxSelect2').val(data.id).trigger('change');
      } else {
        // Create a DOM Option and pre-select by default
        const newOption = new Option(data.text, data.id, true, true);
        // Append it to the select
        $('#ajaxSelect2').append(newOption).trigger('change');
      }
    }
  }

  initCheckNumChange() {
    $('#ajaxSelect2').on('select2:select', (e) => {
      const { payment, adjustmentInd } = this.addUpdatePaymentMgr.state;
      AjaxService.ajaxGet(`./api/CheckMgrApi/${e.params.data.id}`)
        .then((check) => {
          if (check && check.id) {
            const { payment } = this.addUpdatePaymentMgr.state;
            this.addUpdatePaymentMgr.setState({
              ...this.addUpdatePaymentMgr.state,
              check: {
                ...check,
                checkDate: check.checkDate ? new Date(check.checkDate).toLocaleDateString() : null,
                receivedDate: check.receivedDate ? new Date(check.receivedDate).toLocaleDateString() : null,
              },
              payment: {
                ...payment,
                checkId: check.id,
                checkNum: check.checkNum,
              },
            });
            if (!(payment.id && adjustmentInd)) {
              this.addUpdatePaymentMgr.setState({
                ...this.addUpdatePaymentMgr.state, wellTractInformation: {},
              });
            }

            AjaxService.ajaxGet(`./api/LesseeMgrApi/${check.lesseeId}`)
              .then((lessee) => {
                this.addUpdatePaymentMgr.setState({
                  ...this.addUpdatePaymentMgr.state, lessee,
                });

                this.addUpdatePaymentMgr.render();
              });
          }
        });
    });
  }
}
