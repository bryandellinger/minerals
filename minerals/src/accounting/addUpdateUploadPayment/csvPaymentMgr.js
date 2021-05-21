import AjaxService from '../../services/ajaxService';
import initDataTable from './initDataTable';
import CsvEditFormMgr from './csvEditFormMgr';

const template = require('../../views/accounting/addUpdateUploadPayment/csvPayment.handlebars');

export default class csvPaymentMgr {
  constructor(addUpdateUploadMgr) {
    this.addUpdateUploadMgr = addUpdateUploadMgr;
    this.container = 'csvPaymentContainer';
    this.table = null;
    this.csvEditFormMgr = null;
  }

  init() {
    const {
      csvPayments, editmode,
    } = this.addUpdateUploadMgr.state;
    if (this.table) {
      this.table.destroy();
    }
    this.table = null;
    this.csvEditFormMgr = new CsvEditFormMgr(this);
    this.render();

    this.table = initDataTable(csvPayments.map(
      (item) => (
        {
          ...item,
          error: item.wellId ? '' : 'Well does not exist',
        }
      ),
    ), editmode);
    this.initEditClick();
  }

  render() {
    const {
      editmode, files,
    } = this.addUpdateUploadMgr.state;
    const handlebarData = {
      editmode,
      files,
    };
    const t = template(handlebarData);
    $(`#${this.container}`).empty().append(t);
    this.initCreatePaymentBtnClick();
  }

  initCreatePaymentBtnClick() {
    $('#createPaymentBtn').click(() => {
      const {
        files, uploadPayment, editmode,
      } = this.addUpdateUploadMgr.state;

      const fileId = parseInt(files[0].id, 10);
      const uploadTemplateId = parseInt(uploadPayment.uploadTemplateId, 10);

      $('.spinner').show();
      AjaxService.ajaxGet(`./api/UploadPaymentMgrApi/CreateCSVPayments/${fileId}/${uploadTemplateId}`)
        .then((csvPayments) => {
          if (this.table) {
            this.table.destroy();
          }
          this.table = null;
          this.addUpdateUploadMgr.setState({ ...this.addUpdateUploadMgr.state, csvPayments });
          const newCsvPaymentsWithErrors = this.addUpdateUploadMgr.state.csvPayments.map(
            (item) => (
              {
                ...item,
                error: item.wellId ? '' : 'Well does not exist',
              }
            ),
          );
          this.table = initDataTable(newCsvPaymentsWithErrors, editmode);
          this.initEditClick();
          $('.spinner').hide();
        })
        .catch(() => {
          $('.spinner').hide();
        });
    });
  }

  initEditClick() {
    $('#csvPaymentsTable tbody').on('click', 'button', (event) => {
      const data = this.table.row($(event.target).parents('tr')).data();
      this.csvEditFormMgr.init(data);
    });
  }
}
