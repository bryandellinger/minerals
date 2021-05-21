import '../node_modules/toastr/build/toastr.css';
import './style.css';
import '@fortawesome/fontawesome-free/js/fontawesome';
import '@fortawesome/fontawesome-free/js/solid';
import '@fortawesome/fontawesome-free/js/regular';
import '@fortawesome/fontawesome-free/js/brands';
import 'jquery-validation';
import * as Toastr from 'toastr';
import ManageContractsMgr from './contracts/manageContracts/index';
import ManageLesseesMgr from './contracts/manageLessees/index';
import ManageTractsMgr from './contracts/manageTracts/index';
import AddUpdateContractMgr from './contracts/addUpdateContract/index'
import SideBarMgr from './sidebar/index';
import getUrlVars from './services/getUrlVars';

require('jquery-ui-bundle/jquery-ui.css');
require('jquery-ui-bundle');

window.Toastr = Toastr;

$(() => {
    $.ajaxSetup({ cache: false });
    const sideBarMgr = new SideBarMgr('contracts');
    const addUpdateContractMgr = new AddUpdateContractMgr(sideBarMgr);
    const manageContractsMgr = new ManageContractsMgr({ addUpdateContractMgr, sideBarMgr });
    const manageLesseesMgr = new ManageLesseesMgr({ sideBarMgr });
    const manageTractsMgr = new ManageTractsMgr({ sideBarMgr });
    sideBarMgr.init();
    const contractId = getUrlVars()['contractId'] || 0;
    const wellId = getUrlVars()['wellId'] || 0;
    const tractNum = getUrlVars()['tractNum'] || '';
    const lesseeId = getUrlVars()['lesseeId'] || 0;
    const storageRentalId = getUrlVars()['storageRentalId'] || 0;
    const contractRentalId = getUrlVars()['contractRentalId'] || 0;
    const suretyId = getUrlVars()['suretyId'] || 0;
    addUpdateContractMgr.init(
        parseInt(contractId, 10), parseInt(wellId, 10), parseInt(storageRentalId, 10), parseInt(contractRentalId, 10), parseInt(suretyId, 10)
    );
    manageContractsMgr.init();
    manageLesseesMgr.init(null, parseInt(lesseeId, 10));
    manageTractsMgr.init(tractNum);
    addUpdateContractMgr.addManageContractsMgr(manageContractsMgr);
    addUpdateContractMgr.addManageTractsMgr(manageTractsMgr);
    $(document).on('focus', ':input', function () {
        $(this).attr('autocomplete', 'off');
    });
    $("input.select2-input").attr('autocomplete', 'off');
});
