import initCardHeader from '../initCardHeader';
import initDataTable from './initDataTable';
import AjaxService from '../../services/ajaxService';
import 'datatables.net/js/jquery.dataTables';
import 'datatables.net-buttons/js/dataTables.buttons';
import 'datatables.net-buttons/js/buttons.html5';
import 'datatables.net-buttons/js/buttons.flash';
import 'datatables.net-dt/css/jquery.dataTables.css';
import '../../datatablebutton.css';

const template = require('../../views/accounting/manageUploadPayments/index.handlebars');

export default class ManageUploadPaymentsMgr {
  constructor(options) {
    this.sideBarMgr = options.sideBarMgr;
    this.initSideBarButtonClick();
    this.addUpdateUploadPaymentMgr = options.addUpdateUploadPaymentMgr;
    this.container = $('#manageUploadPaymentsContainer');
    this.table = null;
  }

    init() {
    if (this.table) {
      this.table.destroy();
    }
    this.table = null;
    const t = template({ headerInfo: initCardHeader() });
    this.container.empty().append(t);
    this.getData();
  }

  getData() {
    $('.spinner').show();
      AjaxService.ajaxGet('./api/UploadPaymentMgrApi')
          .then((uploadPayments) => {
              this.table = initDataTable(uploadPayments.map(
                  (uploadPayment) => (
            {
               ...uploadPayment,
              uploadPaymentEntryDate: uploadPayment.uploadPaymentEntryDate ? new Date(uploadPayment.uploadPaymentEntryDate) : null,
            }
          ),
        ));
        this.initSelectClick();
        $('.spinner').hide();
      });
  }

  initSelectClick() {
      $('#uploadPaymentsTable tbody').on('click', 'button', (event) => {
      const data = this.table.row($(event.target).parents('tr')).data();
      this.addUpdateUploadPaymentMgr.init(data.id);
      this.sideBarMgr.setVisibleContainer('addUpdateUploadPaymentContainer');
    });
  }


  initSideBarButtonClick() {
    $('.sidebar').on('click', '.spa-btn', () => {
      this.init(0);
    });
  }
}
