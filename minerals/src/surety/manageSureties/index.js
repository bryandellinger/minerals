import initCardHeader from '../initCardHeader';
import initDataTable from './initDataTable';
import AjaxService from '../../services/ajaxService';
import 'datatables.net/js/jquery.dataTables';
import 'datatables.net-buttons/js/dataTables.buttons';
import 'datatables.net-buttons/js/buttons.html5';
import 'datatables.net-buttons/js/buttons.flash';
import 'datatables.net-dt/css/jquery.dataTables.css';
import '../../datatablebutton.css';
import * as constants from '../../constants';

const template = require('../../views/surety/manageSureties/index.handlebars');

export default class ManageSuretiesMgr {
  constructor(options) {
    this.sideBarMgr = options.sideBarMgr;
    this.initSideBarButtonClick();
    this.addUpdateSuretyMgr = options.addUpdateSuretyMgr;
    this.container = $('#manageSuretiesContainer');
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
    AjaxService.ajaxGet('./api/SuretyMgrApi')
      .then((sureties) => {
        this.table = initDataTable(sureties.map(
          (surety) => (
            {
              ...surety,
              issueDate: surety.issueDate ? new Date(surety.issueDate) : null,
              releasedDate: surety.releasedDate ? new Date(surety.releasedDate) : null,
              wells: surety.bondCategoryName === constants.BondCategoryPlugging ?  surety.wells : '',
              contractNum: surety.bondCategoryName === constants.BondCategoryPerformance ? surety.contractNum: ''
            }
          ),
        ));
        this.initSelectClick();
        $('.spinner').hide();
      });
  }

  initSelectClick() {
    $('#suretiesTable tbody').on('click', 'button', (event) => {
      const data = this.table.row($(event.target).parents('tr')).data();
      this.addUpdateSuretyMgr.init(data.id);
      this.sideBarMgr.setVisibleContainer('addUpdateSuretyContainer');
    });
  }


  initSideBarButtonClick() {
    $('.sidebar').on('click', '.spa-btn', () => {
      this.init(0);
    });
  }
}
