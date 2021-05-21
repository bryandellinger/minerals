/* eslint-disable max-len */
import AjaxService from '../../services/ajaxService';

export default class checkNumberMgr {
  constructor(AddUpdateStorageFieldMgr) {
    this.addUpdateStorageFieldMgr = AddUpdateStorageFieldMgr;
  }

  init() {
    const { check } = this.addUpdateStorageFieldMgr.state;
    $('#storageRentalCheckNum').select2({
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
      if ($('#storageRentalCheckNum').find(`option[value='${data.id}']`).length) {
        $('storageRentalCheckNum').val(data.id).trigger('change');
      } else {
        // Create a DOM Option and pre-select by default
        const newOption = new Option(data.text, data.id, true, true);
        // Append it to the select
        $('#storageRentalCheckNum').append(newOption).trigger('change');
      }
    }
  }

  initCheckNumChange() {
    $('#storageRentalCheckNum').on('select2:select', (e) => {
      AjaxService.ajaxGet(`./api/CheckMgrApi/${e.params.data.id}`)
        .then((check) => {
          if (check && check.id) {
            const { storageRental } = this.addUpdateStorageFieldMgr.state;
            this.addUpdateStorageFieldMgr.setState({
              ...this.addUpdateStorageFieldMgr.state,
              check: {
                ...check,
                checkDate: check.checkDate ? new Date(check.checkDate).toLocaleDateString() : null,
                receivedDate: check.receivedDate ? new Date(check.receivedDate).toLocaleDateString() : null,
              },
              storageRental: {
                ...storageRental,
                checkId: check.id,
                storageRentalRecvDate: check.receivedDate ? new Date(check.receivedDate) : null,
              },
            });
           /* if (!(payment.id && adjustmentInd)) {
              this.addUpdatePaymentMgr.setState({
                ...this.addUpdatePaymentMgr.state, wellTractInformation: {},
              });
            }*/

            AjaxService.ajaxGet(`./api/LesseeMgrApi/${check.lesseeId}`)
              .then((lessee) => {
                this.addUpdateStorageFieldMgr.setState({
                  ...this.addUpdateStorageFieldMgr.state, lessee,
                });

                this.addUpdateStorageFieldMgr.render();
              });
          }
        });
    });
  }
}
