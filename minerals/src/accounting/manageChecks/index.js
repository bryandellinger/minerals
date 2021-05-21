import initCardHeader from '../initCardHeader';
import initDataTable from './initDataTable';
import 'datatables.net/js/jquery.dataTables';
import 'datatables.net-buttons/js/dataTables.buttons';
import 'datatables.net-buttons/js/buttons.html5';
import 'datatables.net-buttons/js/buttons.flash';
import 'datatables.net-dt/css/jquery.dataTables.css';
import '../../datatablebutton.css';
import AjaxService from '../../services/ajaxService';

const template = require('../../views/accounting/manageChecks/index.handlebars');

export default class ManageChecksMgr {
    constructor(options) {
        this.sideBarMgr = options.sideBarMgr;
        this.initSideBarButtonClick();
        this.addUpdateCheckMgr = options.addUpdateCheckMgr;
        this.container = $('#manageChecksContainer');
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
        this.table = initDataTable();
        this.initSelectClick();
    }

    initSelectClick() {
        $('#checksTable tbody').on('click', 'button', (event) => {
            const data = this.table.row($(event.target).parents('tr')).data();
            this.addUpdateCheckMgr.init(data.id);
            this.sideBarMgr.setVisibleContainer('addUpdateCheckContainer');
        });
    }



    initSideBarButtonClick() {
        $('.sidebar').on('click', '.spa-btn', () => {
            this.init(0);
        });
    }

}