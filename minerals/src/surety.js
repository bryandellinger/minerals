import '../node_modules/toastr/build/toastr.css';
import './style.css';
import '@fortawesome/fontawesome-free/js/fontawesome';
import '@fortawesome/fontawesome-free/js/solid';
import '@fortawesome/fontawesome-free/js/regular';
import '@fortawesome/fontawesome-free/js/brands';
import 'jquery-validation';
import * as Toastr from 'toastr';
import ManageSuretiesMgr from './surety/manageSureties/index';
import AddUpdateSuretyMgr from './surety/addUpdateSurety/index';
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
    const sideBarMgr = new SideBarMgr('surety');
    const addUpdateSuretyMgr = new AddUpdateSuretyMgr(sideBarMgr);
    const manageSuretiesMgr = new ManageSuretiesMgr({ addUpdateSuretyMgr, sideBarMgr });
    sideBarMgr.init();
    manageSuretiesMgr.init();
    const suretyId = getUrlVars()['suretyId'] || 0;
    const wellId = getUrlVars()['wellId'] || 0;
    const contractId = getUrlVars()['contractId'] || 0;
    addUpdateSuretyMgr.init(parseInt(suretyId, 10), parseInt(wellId, 10), parseInt(contractId, 10));
    addUpdateSuretyMgr.addManageSuretiesMgr(manageSuretiesMgr);
    $(document).on('focus', ':input', function () {
        $(this).attr('autocomplete', 'off');
    });

    $("input.select2-input").attr('autocomplete', 'off');

});