import initCardHeader from '../initCardHeader';
import initDataTable from './initDataTable';
import AjaxService from '../../services/ajaxService';
import 'datatables.net/js/jquery.dataTables';
import 'datatables.net-buttons/js/dataTables.buttons';
import 'datatables.net-buttons/js/buttons.html5';
import 'datatables.net-buttons/js/buttons.flash';
import 'datatables.net-dt/css/jquery.dataTables.css';
import '../../datatablebutton.css';

const template = require('../../views/accounting/manageStorageFields/index.handlebars');

export default class ManageStorageFieldsMgr {
  constructor(options) {
    this.sideBarMgr = options.sideBarMgr;
    this.initSideBarButtonClick();
    this.addUpdateStorageFieldMgr = options.addUpdateStorageFieldMgr;
    this.container = $('#manageStorageFieldsContainer');
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
    AjaxService.ajaxGet('./api/StorageRentalMgrApi')
      .then((storageRentals) => {
        this.table = initDataTable(storageRentals.map(
          (storageRental) => (
            {
              ...storageRental,
                    storageRentalEntryDate: storageRental.storageRentalEntryDate ? new Date(storageRental.storageRentalEntryDate) : null,
                    effectiveDate: storageRental.effectiveDate ? new Date(storageRental.effectiveDate) : null,
                    receivedDate: storageRental.receivedDate ? new Date(storageRental.receivedDate) : null,
            }
          ),
        ));
        this.initSelectClick();
        $('.spinner').hide();
      });
  }

  initSelectClick() {
    $('#storageFieldsTable tbody').on('click', 'button', (event) => {
      const data = this.table.row($(event.target).parents('tr')).data();
      this.addUpdateStorageFieldMgr.init(data.id);
      this.sideBarMgr.setVisibleContainer('addUpdateStorageFieldContainer');
    });
  }


  initSideBarButtonClick() {
    $('.sidebar').on('click', '.spa-btn', () => {
      this.init(0);
    });
  }
}
