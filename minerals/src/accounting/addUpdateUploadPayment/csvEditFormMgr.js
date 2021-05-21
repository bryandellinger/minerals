const template = require('../../views/accounting/addUpdateUploadPayment/csvEditFormModal.handlebars');

export default class csvEditFormMgr {
  constructor(csvPaymentMgr) {
    this.csvPaymentMgr = csvPaymentMgr;
    this.container = 'csvEditFormModalContainer';
  }

  init(data) {
    this.render(data);
  }

  render(csvPayment) {
    this.csvPaymentMgr.addUpdateUploadMgr.setState({ ...this.csvPaymentMgr.addUpdateUploadMgr.state, csvPayment });
      const { wells } = this.csvPaymentMgr.addUpdateUploadMgr.state;
      var d = new Date();
      var yr = d.getFullYear();
      const postYears = Array.from(Array(100).keys(), n => n + yr - 50);
      const postMonths = Array.from(Array(12).keys(), n => n + 1);
    const handlebarData = {
      csvPayment,
      wells: wells.map(
        (well) => (
          {
            ...well,
            selected: well.id === csvPayment.wellId,
          }
        ),
        ),
        postYears: postYears.map(
            (postYear) => (
                {
                    postYear,
                    selected: postYear === csvPayment.postYear,
                }
            ),
        ),
        postMonths: postMonths.map(
            (postMonth) => (
                {
                    postMonth,
                    selected: postMonth === csvPayment.postMonth,
                }
            ),
        ),
    };
    const t = template(handlebarData);
    $(`#${this.container}`).empty().append(t);
    $('#csvWellId').select2({ placeholder: 'Select a well', width: '100%' });
    $('#csvPostYear').select2({ placeholder: 'Select a  production year', width: '100%' });
    this.initCsvWellChange();
    this.initInputChange();
    this.initUpdateFromModalBtnClick();
    document.getElementById('openEditFormModal').click();
  }

  initCsvWellChange() {
    $('#csvWellId').on('select2:select', (e) => {
      const { data } = e.params;
      const { csvPayment } = this.csvPaymentMgr.addUpdateUploadMgr.state;
      csvPayment.apiNum = data.text.replace(/\s+/g, '');
      csvPayment.wellId = parseInt(data.id, 10);
      this.csvPaymentMgr.addUpdateUploadMgr.setState({
        ...this.csvPaymentMgr.addUpdateUploadMgr.state, csvPayment,
      });
    });
  }

  initInputChange() {
    $('.modalInput').change((e) => {
      const { csvPayment } = this.csvPaymentMgr.addUpdateUploadMgr.state;
      const element = $(e.target).attr('name');
      csvPayment[element] = $(e.target).val();
      this.csvPaymentMgr.addUpdateUploadMgr.setState({
        ...this.csvPaymentMgr.addUpdateUploadMgr.state,
        csvPayment: {
          ...csvPayment,
          gasProd: csvPayment.gasProd ? parseFloat(csvPayment.gasProd) : null,
          gasRoyalty: csvPayment.gasRoyalty ? parseFloat(csvPayment.gasRoyalty) : null,
          salesPrice: csvPayment.salesPrice ? parseFloat(csvPayment.salesPrice) : null,
          nri: csvPayment.nri ? parseFloat(csvPayment.nri) : null,
          postYear: csvPayment.postYear ? parseInt(csvPayment.postYear, 10) : null,
          postMonth: csvPayment.postMonth ? parseInt(csvPayment.postMonth, 10) : null
        },
      });
    });
  }

  initUpdateFromModalBtnClick() {
    $('#updateFromModalBtn').click((e) => {
      const { csvPayment, csvPayments } = this.csvPaymentMgr.addUpdateUploadMgr.state;
      // eslint-disable-next-line no-confusing-arrow
      const newCsvPayments = csvPayments.map((x) => x.id === csvPayment.id ? csvPayment : x);
      const newCsvPaymentsWithErrors = newCsvPayments.map(
        (item) => (
          {
            ...item,
            error: item.wellId ? '' : 'Well does not exist',
          }
        ),
      );
      this.csvPaymentMgr.addUpdateUploadMgr.setState({ ...this.csvPaymentMgr.addUpdateUploadMgr.state, csvPayments: newCsvPayments });
      this.csvPaymentMgr.table.clear().draw(false);
      this.csvPaymentMgr.table.rows.add(newCsvPaymentsWithErrors); // Add new data
      this.csvPaymentMgr.table.columns.adjust().draw(false); // Redraw the DataTable
    });
  }
}
