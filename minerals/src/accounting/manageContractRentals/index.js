import initCardHeader from '../initCardHeader';
import initDataTable from './initDataTable';
import AjaxService from '../../services/ajaxService';
import 'datatables.net/js/jquery.dataTables';
import 'datatables.net-buttons/js/dataTables.buttons';
import 'datatables.net-buttons/js/buttons.html5';
import 'datatables.net-buttons/js/buttons.flash';
import 'datatables.net-dt/css/jquery.dataTables.css';
import '../../datatablebutton.css';

const template = require('../../views/accounting/manageContractRentals/index.handlebars');

export default class ManageContracxtRentalsMgr {
  constructor(options) {
    this.sideBarMgr = options.sideBarMgr;
    this.initSideBarButtonClick();
    this.addUpdateContractRentalMgr = options.addUpdateContractRentalMgr;
    this.container = $('#manageContractRentalsContainer');
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
    AjaxService.ajaxGet('./api/ContractRentalMgrApi')
      .then((contractRentals) => {
        this.table = initDataTable(contractRentals.map(
          (contractRental) => (
            {
              ...contractRental,
                    contractRentalEntryDate: contractRental.contractRentalEntryDate ? new Date(contractRental.contractRentalEntryDate) : null,
                    effectiveDate: contractRental.effectiveDate ? new Date(contractRental.effectiveDate) : null,
                    checkDate: contractRental.checkDate ? new Date(contractRental.checkDate) : null,
            }
          ),
        ));
        this.initSelectClick();
        $('.spinner').hide();
      });
  }

  initSelectClick() {
    $('#contractRentalsTable tbody').on('click', 'button', (event) => {
      const data = this.table.row($(event.target).parents('tr')).data();
      this.addUpdateContractRentalMgr.init(data.id);
      this.sideBarMgr.setVisibleContainer('addUpdateContractRentalContainer');
    });
  }


  initSideBarButtonClick() {
    $('.sidebar').on('click', '.spa-btn', () => {
      this.init(0);
    });
  }
}
