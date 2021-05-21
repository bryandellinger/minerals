import initCardHeader from '../initCardHeader';
import initDataTable from './initDataTable';
import AjaxService from '../../services/ajaxService';
import 'datatables.net/js/jquery.dataTables';
import 'datatables.net-buttons/js/dataTables.buttons';
import 'datatables.net-buttons/js/buttons.html5';
import 'datatables.net-buttons/js/buttons.flash';
import 'datatables.net-dt/css/jquery.dataTables.css';
import '../../datatablebutton.css';

const template = require('../../views/accounting/manageOtherPayments/index.handlebars');

export default class ManageOtherPaymentsMgr {
  constructor(options) {
    this.sideBarMgr = options.sideBarMgr;
    this.initSideBarButtonClick();
   this.addUpdateOtherPaymentMgr = options.addUpdateOtherPaymentMgr;
    this.container = $('#manageOtherPaymentsContainer');
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
      AjaxService.ajaxGet('./api/OtherPaymentMgrApi')
          .then((otherPayments) => {
              this.table = initDataTable(otherPayments.map(
                  (otherPayment) => (
            {
                     ...otherPayment,
                          otherRentalEntryDate: otherPayment.otherRentalEntryDate ? new Date(otherPayment.otherRentalEntryDate) : null,
                          receivedDate: otherPayment.receivedDate ? new Date(otherPayment.receivedDate) : null,
                          checkDate: otherPayment.checkDate ? new Date(otherPayment.checkDate) : null,
            }
          ),
        ));
        this.initSelectClick();
        $('.spinner').hide();
      });
  }

  initSelectClick() {
      $('#otherPaymentsTable tbody').on('click', 'button', (event) => {
      const data = this.table.row($(event.target).parents('tr')).data();
      this.addUpdateOtherPaymentMgr.init(data.id);
      this.sideBarMgr.setVisibleContainer('addUpdateOtherPaymentContainer');
    });
  }


  initSideBarButtonClick() {
    $('.sidebar').on('click', '.spa-btn', () => {
      this.init(0);
    });
  }
}
