import '../node_modules/toastr/build/toastr.css';
import './style.css';
import '@fortawesome/fontawesome-free/js/fontawesome';
import '@fortawesome/fontawesome-free/js/solid';
import '@fortawesome/fontawesome-free/js/regular';
import '@fortawesome/fontawesome-free/js/brands';
import 'jquery-validation';
import * as Toastr from 'toastr';
import ManageWellsMgr from './wells/manageWells/index';
import AddUpdateWellMgr from './wells/addUpdateWell/index'
import SideBarMgr from './sidebar/index';
import getUrlVars from './services/getUrlVars';

require('jquery-ui-bundle/jquery-ui.css');
require('jquery-ui-bundle');

window.Toastr = Toastr;

$(() => {
    $.ajaxSetup({ cache: false });
    const sideBarMgr = new SideBarMgr('wells');
    const addUpdateWellMgr = new AddUpdateWellMgr(sideBarMgr);
    const manageWellsMgr = new ManageWellsMgr({ addUpdateWellMgr, sideBarMgr });
    sideBarMgr.init();
    manageWellsMgr.init();
    const wellId = getUrlVars()['wellId'] || 0;
    const contractId = getUrlVars()['contractId'] || 0;
    const unitId = getUrlVars()['unitId'] || 0;
    const paymentId = getUrlVars()['paymentId'] || 0;
    const suretyId = getUrlVars()['suretyId'] || 0;
    addUpdateWellMgr.init(parseInt(wellId, 10), parseInt(contractId, 10), parseInt(unitId, 10), parseInt(paymentId, 10), parseInt(suretyId, 10));
    addUpdateWellMgr.addManageWellsMgr(manageWellsMgr);

    $(document).on('focus', ':input', function () {
        $(this).attr('autocomplete', 'off');
    });

    $("input.select2-input").attr('autocomplete', 'off');

});
