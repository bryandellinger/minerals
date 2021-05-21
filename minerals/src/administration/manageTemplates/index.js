import initCardHeader from '../initCardHeader';
import initDataTable from './initDataTable';
import AjaxService from '../../services/ajaxService';
import 'datatables.net/js/jquery.dataTables';
import 'datatables.net-buttons/js/dataTables.buttons';
import 'datatables.net-buttons/js/buttons.html5';
import 'datatables.net-buttons/js/buttons.flash';
import 'datatables.net-dt/css/jquery.dataTables.css';
import '../../datatablebutton.css';

const template = require('../../views/administration/manageTemplates/index.handlebars');

export default class ManageTemplatesMgr {
  constructor(options) {
    this.sideBarMgr = options.sideBarMgr;
    this.initSideBarButtonClick();
    this.addUpdateTemplateMgr = options.addUpdateTemplateMgr;
    this.container = $('#manageTemplatesContainer');
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
    AjaxService.ajaxGet('./api/UploadTemplateMgrApi')
      .then((uploadTemplates) => {
        this.table = initDataTable(uploadTemplates);
        this.initSelectClick();
        $('.spinner').hide();
      });
  }

  initSelectClick() {
    $('#uploadTemplatesTable tbody').on('click', 'button', (event) => {
      const data = this.table.row($(event.target).parents('tr')).data();
      this.addUpdateTemplateMgr.init(data.id);
      this.sideBarMgr.setVisibleContainer('addUpdateTemplateContainer');
    });
  }


  initSideBarButtonClick() {
    $('.sidebar').on('click', '.spa-btn', () => {
      this.init(0);
    });
  }
}
