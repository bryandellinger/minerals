import initCardHeader from '../initCardHeader';
import initDataTable from './initDataTable';
import * as constants from '../../constants';
import 'datatables.net/js/jquery.dataTables';
import 'datatables.net-buttons/js/dataTables.buttons';
import 'datatables.net-buttons/js/buttons.html5';
import 'datatables.net-buttons/js/buttons.flash';
import 'datatables.net-dt/css/jquery.dataTables.css';
import '../../datatablebutton.css';
import AjaxService from '../../services/ajaxService';

const template = require('../../views/accounting/managePayments/index.handlebars');

export default class ManagePaymentsMgr {
  constructor(options) {
    this.sideBarMgr = options.sideBarMgr;
    this.initSideBarButtonClick();
    this.addUpdatePaymentMgr = options.addUpdatePaymentMgr;
    this.container = $('#managePaymentsContainer');
    this.tablecontainer = 'tablecontainer';
    this.table = null;
  }

  init() {
      AjaxService.ajaxGet('./api/PaymentTypeMgrApi')
          .then((paymentTypes) => {
              if (this.table) {
                  this.table.destroy();
              }
              this.table = null;
              const t = template({
                  headerInfo: initCardHeader(),
                  paymentTypes: paymentTypes.map(
                      (payment) => (
                          {
                              ...payment,
                              selected: payment.paymentTypeName === constants.PaymentTypeOilAndGas,
                          }
                      ),
                  ),
              });
              this.container.empty().append(t);

              $('.paymentDatepicker').datepicker({
                  dateFormat: 'mm/dd/yy',
                  changeMonth: true,
                  changeYear: true,
                  yearRange: '-100:+100',
                  onSelect: () => { this.table.draw(); },
                  beforeShow: (elem, dp) => {
                      $(dp.dpDiv).removeClass('hide-day-calender');
                  },
                  onChangeMonthYear: function (year, month, inst) {
                      var format = inst.settings.dateFormat
                      var selectedDate = new Date(inst.selectedYear, inst.selectedMonth, inst.selectedDay)
                      var date = $.datepicker.formatDate('mm/dd/yy', selectedDate);
                      $(this).datepicker("setDate", date);
                  },
            });
            


          $('#from').datepicker('setDate', new Date(new Date().setFullYear(new Date().getFullYear() - 1)));
          $('#to').datepicker('setDate', new Date(new Date().setFullYear(new Date().getFullYear() + 1)));
        this.table = initDataTable();
        $('#toBtn').click(() => { $('#to').datepicker('show'); });
        $('#fromBtn').click(() => { $('#from').datepicker('show'); });
        $('#fromClearBtn').click(() => { $('#from').datepicker('setDate', null); this.table.draw(); });
        $('#toClearBtn').click(() => { $('#to').datepicker('setDate', null); this.table.draw(); });
        $('#from').keydown((e) => {
          if (e.keyCode === 8) {
            e.preventDefault();
            $('#from').datepicker('setDate', null);
            this.table.draw();
          }
        });
        $('#to').keydown((e) => {
          if (e.keyCode === 8) {
            e.preventDefault();
            $('#to').datepicker('setDate', null);
            this.table.draw();
          }
        });
        this.initSelectClick();
        $('#paymentTypeDropDownId').change(() => {
          this.table.draw();
        });
      });
  }

  initSelectClick() {
    $('#paymentsTable tbody').on('click', 'button', (event) => {
      const data = this.table.row($(event.target).parents('tr')).data();
      this.addUpdatePaymentMgr.init(data.id);
      this.sideBarMgr.setVisibleContainer('addUpdatePaymentContainer');
    });
  }

  initSideBarButtonClick() {
    $('.sidebar').on('click', '.spa-btn', () => {
      this.init(0);
    });
  }
}
