import initCardHeader from '../initCardHeader';
import initDataTable from './initDataTable';
import 'datatables.net/js/jquery.dataTables';
import 'datatables.net-buttons/js/dataTables.buttons';
import 'datatables.net-buttons/js/buttons.html5';
import 'datatables.net-buttons/js/buttons.flash';
import 'datatables.net-dt/css/jquery.dataTables.css';
import '../../datatablebutton.css';
import 'disableautofill/src/jquery.disableAutoFill';
import AjaxService from '../../services/ajaxService';


const template = require('../../views/wells/manageWells/index.handlebars');

export default class ManageWellsMgr {
  constructor(options) {
    this.sideBarMgr = options.sideBarMgr;
    this.addUpdateWellMgr = options.addUpdateWellMgr;
    this.container = $('#manageWellsContainer');
    this.tablecontainer = 'tablecontainer';
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
    $('.spinner2').show();
    AjaxService.ajaxGet('./api/WellMgrApi/wellsForDataTable')
      .then((wells) => {   
        this.table = initDataTable(wells);
        this.initSelectClick();
        $('.spinner2').hide();
      });
  }

  initSelectClick() {
    $('#wellsTable tbody').on('click', 'button', (event) => {
      const data = this.table.row($(event.target).parents('tr')).data();
      this.addUpdateWellMgr.init(data.wellId);
      this.sideBarMgr.setVisibleContainer('addUpdateWellContainer');
    });
  }
}
