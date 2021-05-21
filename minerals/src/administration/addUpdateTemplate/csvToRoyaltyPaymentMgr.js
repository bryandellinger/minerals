import royaltyMappingColumns from './royaltyMappingColumns';

const template = require('../../views/administration/addUpdateTemplate/csvToRoyaltyPayment.handlebars');

export default class CsvToRoyaltyPaymentMgr {
  constructor(addUpdateTemplateMgr) {
    this.addUpdateTemplateMgr = addUpdateTemplateMgr;
    this.container = 'csvToRoyaltyPaymentContainer';
  }

  init() {
      this.render();
  }

  render() {
    const {
      editmode, uploadTemplate, unmappedHeaders, mappedHeaders,
    } = this.addUpdateTemplateMgr.state;
    const handlebarData = {
      editmode,
      uploadTemplate,
      unmappedHeaders,
      mappedHeaders,
    };
    const t = template(handlebarData);
    $(`#${this.container}`).empty().append(t);
    $('.draggable').draggable({
      addClasses: false,
      revert: 'invalid',
    });

    $('.droppable').droppable({
      accept(elem) {
        return elem.hasClass('draggable');
      },
      drop(event, ui) {
        // $(this).css('background-color', 'lightgray');
        const dropped = ui.draggable;
        const droppedOn = $(this);
        $(dropped).detach().css({ top: 0, left: 0 }).appendTo(droppedOn);
      },
      over(event, ui) {
        // Enable all the .droppable elements
        $('.droppable').droppable('enable');

        // If the droppable element we're hovered over already contains a .draggable element,
        // don't allow another one to be dropped on it
        if ($(this).has('.draggable').length && $(this).hasClass('mappedHeaders')) {
          $(this).droppable('disable');
        }
      },
    });
    this.initCsvToRoyaltyHeaderBtnClick();
  }

  initCsvToRoyaltyHeaderBtnClick() {
    $('#csvToRoyaltyHeaderBtn').click(() => {
      const {
        files,
      } = this.addUpdateTemplateMgr.state;

      if (files && files.length && files[0].id) {
        this.addUpdateTemplateMgr.setState({
          ...this.addUpdateTemplateMgr.state,
          mappedHeaders: royaltyMappingColumns,
          unmappedHeaders: [],
        });
        this.getData(files[0].id);
      } else {
        window.Toastr.warning('Please upload a csv file');
      }
    });
  }

  getData(fileId) {
    $('.spinner').show();
    $.when(
      $.get(`./api/UploadTemplateMgrApi/GetHeaders/${fileId}`, (headers) => {
        this.addUpdateTemplateMgr.setState({
          ...this.addUpdateTemplateMgr.state,
          unmappedHeaders: headers.map(
            (item, index) => (
              {
                header: item,
                index,
              }
            ),
          ),
        });
      }),
    ).then(() => {
      this.render();
      $('.spinner').hide();
    });
  }

  getUnmappedHeaders() {
    const unmappedHeaders = [];
    $('.unmappedHeaders span').each(function () {
      const index = parseInt($(this).attr('data-index'), 10);
      const header = $(this).attr('data-header');
      unmappedHeaders.push({ id: 0, index, header });
    });
    this.addUpdateTemplateMgr.setState({
      ...this.addUpdateTemplateMgr.state, unmappedHeaders,
    });
  }

  getMappedHeaders() {
    const mappedHeaders = [];
    $('.mappedHeaders').each(function () {
      const label = $(this).attr('data-label');
      const attribute = $(this).attr('data-attribute');
      let index = null;
      let header = null;
      $(this).find('span').each(function () {
        index = parseInt($(this).attr('data-index'), 10);
        header = $(this).attr('data-header');
      });
      mappedHeaders.push({
        id: 0, label, attribute, index, header,
      });
    });
    this.addUpdateTemplateMgr.setState({
      ...this.addUpdateTemplateMgr.state, mappedHeaders,
    });
  }
}
