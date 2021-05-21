/* eslint-disable max-len */
export default class monthPickerMgr {
  constructor(addUpdatePaymentMgr) {
    this.addUpdatePaymentMgr = addUpdatePaymentMgr;
  }

  init() {
    const self = this;
    $('#prodMonth').datepicker({
      changeMonth: true,
      changeYear: true,
      showButtonPanel: true,
      dateFormat: 'm yy',
      yearRange: '-100:+100',
      onChangeMonthYear: () => {
        setTimeout(() => {
          $('.ui-datepicker-month > option').each(function () {
            this.text = (parseInt(this.value, 10) + 1);
          });
        }, 0);
      },
      onClose(dateText, inst) {
        $('#prodMonthError').hide();
        $('#prodMonth').removeClass('is-invalid');
        $(this).datepicker('setDate', new Date(inst.selectedYear, inst.selectedMonth, 1));
        const {
          payment,
        } = self.addUpdatePaymentMgr.state;
        payment.postYear = inst.selectedYear;
        payment.postMonth = inst.selectedMonth + 1;
        self.addUpdatePaymentMgr.setState({
          ...self.addUpdatePaymentMgr.state, payment,
        });
        self.addUpdatePaymentMgr.render();
      },
      beforeShow: (elem, dp) => {
        $(dp.dpDiv).addClass('hide-day-calender');
        setTimeout(() => {
          $('.ui-datepicker-month > option').each(function () {
            this.text = (parseInt(this.value, 10) + 1);
          });
        }, 0);
      },
    });
    $('#prodMonthBtn').click(() => { $('#prodMonth').datepicker('show'); });
    $('#prodMonthClearBtn').click(() => {
      $('#prodMonth').datepicker('setDate', null);
      self.addUpdatePaymentMgr.setState({
        ...self.addUpdatePaymentMgr.state, payment: { ...self.addUpdatePaymentMgr.state.payment, postYear: null, postMonth: null },
      });
      self.addUpdatePaymentMgr.render();
    });
    $('#prodMonth').keydown((e) => {
      const {
        editmode,
      } = this.addUpdatePaymentMgr.state;
      if (e.keyCode === 8 && editmode) {
        e.preventDefault();
        $('#prodMonth').datepicker('setDate', null);
        self.addUpdatePaymentMgr.setState({
          ...self.addUpdatePaymentMgr.state, payment: { ...self.addUpdatePaymentMgr.state.payment, postYear: null, postMonth: null },
        });
        self.addUpdatePaymentMgr.render();
      }
    });
    if (this.addUpdatePaymentMgr.state.payment.postMonth && this.addUpdatePaymentMgr.state.payment.postYear) {
      $('#prodMonth').datepicker(
        'setDate', new Date(this.addUpdatePaymentMgr.state.payment.postYear, this.addUpdatePaymentMgr.state.payment.postMonth - 1, 1),
      );
    }
  }
}
