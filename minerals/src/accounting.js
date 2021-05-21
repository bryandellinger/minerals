import '../node_modules/toastr/build/toastr.css';
import './style.css';
import '@fortawesome/fontawesome-free/js/fontawesome';
import '@fortawesome/fontawesome-free/js/solid';
import '@fortawesome/fontawesome-free/js/regular';
import '@fortawesome/fontawesome-free/js/brands';
import 'jquery-validation';
import * as Toastr from 'toastr';
import ManageChecksMgr from './accounting/manageChecks/index';
import AddUpdateCheckMgr from './accounting/addUpdateCheck/index';
import ManagePaymentssMgr from './accounting/managePayments/index';
import AddUpdatePaymentMgr from './accounting/addUpdatePayment/index';
import ManageStorageFieldsMgr from './accounting/manageStorageFields/index';
import AddUpdateStorageFieldMgr from './accounting/addUpdateStorageField/index';
import ManageContractRentalsMgr from './accounting/manageContractRentals/index';
import AddUpdateContractRentalMgr from './accounting/addUpdateContractRental/index';
import ManageOtherPaymentsMgr from './accounting/manageOtherPayments/index';
import AddUpdateOtherPaymentMgr from './accounting/addUpdateOtherPayment/index';
import ManageUploadPaymentsMgr from './accounting/manageUploadPayments/index';
import AddUpdateUploadPaymentMgr from './accounting/addUpdateUploadPayment/index';
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
    const sideBarMgr = new SideBarMgr('accounting');
    const addUpdatePaymentMgr = new AddUpdatePaymentMgr(sideBarMgr);
    const addUpdateStorageFieldMgr = new AddUpdateStorageFieldMgr(sideBarMgr);
    const addUpdateContractRentalMgr = new AddUpdateContractRentalMgr(sideBarMgr);
    const addUpdateOtherPaymentMgr = new AddUpdateOtherPaymentMgr(sideBarMgr);
    const addUpdateUploadPaymentMgr = new AddUpdateUploadPaymentMgr(sideBarMgr);
    const addUpdateCheckMgr = new AddUpdateCheckMgr(
        sideBarMgr, addUpdatePaymentMgr, addUpdateStorageFieldMgr, addUpdateContractRentalMgr, addUpdateOtherPaymentMgr);
    const manageChecksMgr = new ManageChecksMgr({ addUpdateCheckMgr, sideBarMgr });
    const managePaymentsMgr = new ManagePaymentssMgr({ addUpdatePaymentMgr, sideBarMgr });
    const manageStorageFieldsMgr = new ManageStorageFieldsMgr({ addUpdateStorageFieldMgr, sideBarMgr });
    const manageContractRentalsMgr = new ManageContractRentalsMgr({ addUpdateContractRentalMgr, sideBarMgr });
    const manageOtherPaymentsMgr = new ManageOtherPaymentsMgr({ addUpdateOtherPaymentMgr, sideBarMgr });
    const manageUploadPaymentsMgr = new ManageUploadPaymentsMgr({ addUpdateUploadPaymentMgr, sideBarMgr });
    sideBarMgr.init();
    manageChecksMgr.init();
    managePaymentsMgr.init();
    manageStorageFieldsMgr.init();
    manageContractRentalsMgr.init();
    manageOtherPaymentsMgr.init();
    manageUploadPaymentsMgr.init();
    const checkId = getUrlVars()['checkId'] || 0;
    const paymentId = getUrlVars()['paymentId'] || 0;
    const storageRentalId = getUrlVars()['storageRentalId'] || 0;
    const contractRentalId = getUrlVars()['contractRentalId'] || 0;
    const otherPaymentId = getUrlVars()['otherPaymentId'] || 0;
    const uploadPaymentId = getUrlVars()['uploadPaymentId'] || 0;
    addUpdateCheckMgr.init(
        parseInt(checkId, 10), parseInt(paymentId, 10), parseInt(storageRentalId, 10),
        parseInt(contractRentalId, 10), parseInt(otherPaymentId, 10), parseInt(uploadPaymentId, 10)
    );
    addUpdatePaymentMgr.init(parseInt(paymentId, 10));
    addUpdateStorageFieldMgr.init(parseInt(storageRentalId, 10));
    addUpdateContractRentalMgr.init(parseInt(contractRentalId, 10));
    addUpdateOtherPaymentMgr.init(parseInt(otherPaymentId, 10));
    addUpdateUploadPaymentMgr.init(parseInt(uploadPaymentId, 10));
    addUpdateCheckMgr.addManageChecksMgr(manageChecksMgr);
    addUpdatePaymentMgr.addManagePaymentsMgr(managePaymentsMgr);
    addUpdateStorageFieldMgr.addManageStorageFieldsMgr(manageStorageFieldsMgr);
    addUpdateContractRentalMgr.addManageContractRentalsMgr(manageContractRentalsMgr);
    addUpdateOtherPaymentMgr.addManageOtherPaymentsMgr(manageOtherPaymentsMgr);
    addUpdateUploadPaymentMgr.addManageUploadPaymentsMgr(manageUploadPaymentsMgr);
    $(document).on('focus', ':input', function () {
        $(this).attr('autocomplete', 'off');
    });

    $("input.select2-input").attr('autocomplete', 'off');

});