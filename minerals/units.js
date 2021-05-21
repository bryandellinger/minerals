import '../node_modules/toastr/build/toastr.css';
import './style.css';
import '@fortawesome/fontawesome-free/js/fontawesome';
import '@fortawesome/fontawesome-free/js/solid';
import '@fortawesome/fontawesome-free/js/regular';
import '@fortawesome/fontawesome-free/js/brands';
import 'jquery-validation';
import * as Toastr from 'toastr';
import ManageUnitsMgr from './units/manageUnits/index';
import AddUpdateUnitMgr from './units/addUpdateUnit/index';
import SideBarMgr from './sidebar/index';
import getUrlVars from './services/getUrlVars';
import 'dropzone/dist/dropzone.css';

require('jquery-ui-bundle/jquery-ui.css');
require('jquery-ui-bundle');
require('select2');
require('select2/dist/css/select2.css');
require('bootstrap4-toggle');
require('bootstrap4-toggle/css/bootstrap4-toggle.min.css');
window.Dropzone = require('dropzone');

window.Toastr = Toastr;

$(() => {
    $.ajaxSetup({ cache: false });
    const sideBarMgr = new SideBarMgr('units');
    const addUpdateUnitMgr = new AddUpdateUnitMgr(sideBarMgr);
    const manageUnitsMgr = new ManageUnitsMgr({ addUpdateUnitMgr , sideBarMgr });
    sideBarMgr.init();
    manageUnitsMgr.init();
    const unitId = getUrlVars()['unitId'] || 0;
    const wellId = getUrlVars()['wellId'] || 0;
    const paymentId = getUrlVars()['paymentId'] || 0;
    addUpdateUnitMgr.init(parseInt(unitId, 10), parseInt(wellId, 10), parseInt(paymentId, 10));
    addUpdateUnitMgr.addManageUnitsMgr(manageUnitsMgr);
    $(document).on('focus', ':input', function () {
        $(this).attr('autocomplete', 'off');
    });

    $("input.select2-input").attr('autocomplete', 'off');

});