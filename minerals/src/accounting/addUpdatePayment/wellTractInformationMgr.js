import AjaxService from '../../services/ajaxService';
import initDataTable from './initDataTable';

const template = require('../../views/accounting/addUpdatePayment/wellTractInformationModalBody.handlebars');

export default class wellTractInformationMgr {
  constructor(addUpdatePaymentMgr) {
    this.addUpdatePaymentMgr = addUpdatePaymentMgr;
    this.table = null;
  }

  render() {
    const { check } = this.addUpdatePaymentMgr.state;
    if (check && check.id) {
      AjaxService.ajaxGet(`./api/WellTractInformationMgrApi/welltractinfosbylessee/${check.lesseeId}`)
        .then((wellTractInformations) => {
          const handlebarData = { check };
          const t = template(handlebarData);
          $('#wellOwnershipTableContainer').empty().append(t);
          if (this.table) {
            this.table.destroy();
          }
          this.table = initDataTable(wellTractInformations);
          this.initSelectClick();
        });
    } else {
      const handlebarData = { check };
      const t = template(handlebarData);
      $('#wellOwnershipTableContainer').empty().append(t);
    }
  }

  initSelectClick() {
    $('#paymentWellTractInfoTable tbody').on('click', 'button', (event) => {
      document.getElementById('modalCloseBtn').click();
      const data = this.table.row($(event.target).parents('tr')).data();
      AjaxService.ajaxGet(`./api/WellTractInformationMgrApi/welltractinfobyid/${data.id}`)
        .then((wellTractInformation) => {
          this.addUpdatePaymentMgr.setState(
            {
              ...this.addUpdatePaymentMgr.state,
              wellTractInformation,
              payment: {
                ...this.addUpdatePaymentMgr.state.payment,
                wellTractInformationId: wellTractInformation.id,
              },
            },
          );
          this.addUpdatePaymentMgr.render();
        });
    });
  }
}
