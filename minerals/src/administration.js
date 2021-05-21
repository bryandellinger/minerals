import '../node_modules/toastr/build/toastr.css';
import './style.css';
import '@fortawesome/fontawesome-free/js/fontawesome';
import '@fortawesome/fontawesome-free/js/solid';
import '@fortawesome/fontawesome-free/js/regular';
import '@fortawesome/fontawesome-free/js/brands';
import 'jquery-validation';
import * as Toastr from 'toastr';
import ManageTemplatesMgr from './administration/manageTemplates/index';
import AddUpdateTemplateMgr from './administration/addUpdateTemplate/index';
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
    const sideBarMgr = new SideBarMgr('administration');
    const addUpdateTemplateMgr = new AddUpdateTemplateMgr(sideBarMgr);
    const manageTemplatesMgr = new ManageTemplatesMgr({ addUpdateTemplateMgr, sideBarMgr });
    sideBarMgr.init();
    manageTemplatesMgr.init();
    const templateId = getUrlVars()['templateId'] || 0;
    const uploadPaymentId = getUrlVars()['uploadPaymentId'] || 0;
    addUpdateTemplateMgr.init(parseInt(templateId, 10), parseInt(uploadPaymentId, 10) );
    addUpdateTemplateMgr.addManageTemplatesMgr(manageTemplatesMgr);
    $(document).on('focus', ':input', function () {
        $(this).attr('autocomplete', 'off');
    });

    $("input.select2-input").attr('autocomplete', 'off');

});