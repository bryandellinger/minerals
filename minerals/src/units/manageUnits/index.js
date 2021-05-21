import initCardHeader from '../initCardHeader';
import initDataTable from './initDataTable';
import 'datatables.net/js/jquery.dataTables';
import 'datatables.net-buttons/js/dataTables.buttons';
import 'datatables.net-buttons/js/buttons.html5';
import 'datatables.net-buttons/js/buttons.flash';
import 'datatables.net-dt/css/jquery.dataTables.css';
import '../../datatablebutton.css';
import AjaxService from '../../services/ajaxService';

const template = require('../../views/units/manageUnits/index.handlebars');

export default class ManageUnitsMgr {
    constructor(options) {
        this.sideBarMgr = options.sideBarMgr;
        this.addUpdateUnitMgr = options.addUpdateUnitMgr;
        this.container = $('#manageUnitsContainer');
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
        AjaxService.ajaxGet('./api/UnitMgrApi/unitsForDataTable')
            .then((units) => {
                const tabledata = units.map(
                    (unit) => (
                        {
                            ...unit,
                            wells: this.getRidOfDuplicates(unit.wells).join(),
                            numOfWells: this.getRidOfDuplicates(unit.wells).join() ?
                                this.getRidOfDuplicates(unit.wells).length : '',
                        }
                    ),
                );
                this.table = initDataTable(tabledata);
                this.initSelectClick();
                $('.spinner2').hide();
            });
    }

    getRidOfDuplicates(wells) {
        const wellArray = wells.split(',');
        const trimmedWellArray = wellArray.map(string => string.trim());
        const uniqueWellArray = Array.from(new Set(trimmedWellArray));
        return uniqueWellArray
    }

    initSelectClick() {
        $('#unitsTable tbody').on('click', 'button', (event) => {
            const data = this.table.row($(event.target).parents('tr')).data();
            this.addUpdateUnitMgr.init(data.id);
            this.sideBarMgr.setVisibleContainer('addUpdateUnitContainer');
        });
    }

}