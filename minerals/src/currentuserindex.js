import '@babel/polyfill';
import '../node_modules/toastr/build/toastr.css';
import './style.css';
import '@fortawesome/fontawesome-free/js/fontawesome';
import '@fortawesome/fontawesome-free/js/solid';
import '@fortawesome/fontawesome-free/js/regular';
import '@fortawesome/fontawesome-free/js/brands';
import 'jquery-validation';
import * as Toastr from 'toastr';
import CurrentUserMgr from './currentUserMgr/index';

window.Toastr = Toastr;

$(() => {
  $.ajaxSetup({ cache: false });
  const currentUserMgr = new CurrentUserMgr();
  currentUserMgr.init();
});
