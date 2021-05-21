import * as constants from '../../constants';

const template = require('../../views/contracts/addUpdateContract/performanceSurety.handlebars');

export default class PluggingSuretyMgr {
  constructor(AddUpdateContractMgr) {
    this.addUpdateContractMgr = AddUpdateContractMgr;
    this.container = 'pluggingSuretyDetailContainer';
  }

  init() {
    this.initCreatePluggingDetailLinkClick();
    this.initPluggingSuretyDetailInputChange();
    this.initAddRow();
    this.intDeleteRow();
    this.initmeasurementTypeToggle();
  }

  initmeasurementTypeToggle() {
    $(`#${this.container}`).on('click', '#measurementTypeToggle', (e) => {
      const {
        pluggingSuretyDetails,
      } = this.addUpdateContractMgr.state;
      const pluggingSuretyDetailsMeasurementType = $(e.target).prop('checked') ? constants.MeasurementType_TMD : constants.MeasurementType_TVD;
      const newPluggingSuretyDetails = pluggingSuretyDetails.map(
        (pluggingSuretyDetail) => (
          {
            ...pluggingSuretyDetail,
            measurementType: pluggingSuretyDetailsMeasurementType,
          }
        ),
      );
      this.addUpdateContractMgr.setState(
        {
          ...this.addUpdateContractMgr.state,
          pluggingSuretyDetail: newPluggingSuretyDetails,
          pluggingSuretyDetailsMeasurementType,
        },
      );
    });
  }

  initPluggingSuretyDetailInputChange() {
    $(`#${this.container}`).on('change', '.pluggingSuretyDetailInput', (e) => {
      const index = parseInt($(e.target).data('index'), 10);
      const element = $(e.target).data('element');
      const type = $(e.target).data('type');
      const { pluggingSuretyDetails } = this.addUpdateContractMgr.state;
      pluggingSuretyDetails[index][element] = type === 'int' ? parseInt($(e.target).val(), 10) : parseFloat($(e.target).val());
      this.addUpdateContractMgr.setState(
        { ...this.addUpdateContractMgr.state, pluggingSuretyDetails },
      );
    });
  }

  initCreatePluggingDetailLinkClick() {
    $(`#${this.container}`).on('click', '#createPluggingDetailLink', (e) => {
      e.preventDefault();
      this.insertAt(0);
    });
  }

  initAddRow() {
    $(`#${this.container}`).on('click', '.addRecord', (e) => {
      this.insertAt(parseInt($(e.target).data('index'), 10));
    });
  }

  intDeleteRow() {
    $(`#${this.container}`).on('click', '.removeRecord', (e) => {
      const { pluggingSuretyDetails } = this.addUpdateContractMgr.state;
      const index = parseInt($(e.target).data('index'), 10);
      // eslint-disable-next-line no-unused-vars
      const removed = pluggingSuretyDetails.splice(index, 1);
      this.addUpdateContractMgr.setState(
        {
          ...this.addUpdateContractMgr.state,
          pluggingSuretyDetails,
        },
      );
      this.render();
    });
  }


  insertAt(index) {
    const {
      pluggingSuretyDetails,
      pluggingSuretyDetailsMeasurementType,
    } = this.addUpdateContractMgr.state;
    pluggingSuretyDetails.splice(index + 1, 0, {
      id: 0,
      measurementType: pluggingSuretyDetailsMeasurementType,
      fromDepth: null,
      toDepth: null,
      minimumBondAmount: null,
    });
    this.addUpdateContractMgr.setState(
      { ...this.addUpdateContractMgr.state, pluggingSuretyDetails },
    );
    this.render();
    }

    reorderSuretyTable(indexArray) {
        const { pluggingSuretyDetails } = this.addUpdateContractMgr.state;
        const reorderedPluggingSuretyDetails = [];
        indexArray.forEach(index => reorderedPluggingSuretyDetails.push(pluggingSuretyDetails[index]));
        this.addUpdateContractMgr.setState(
            { ...this.addUpdateContractMgr.state, pluggingSuretyDetails: reorderedPluggingSuretyDetails },
        );
        this.render();
    }


  render() {
    const {
      pluggingSuretyDetails,
      pluggingSuretyDetailsMeasurementType,
      editmode,
    } = this.addUpdateContractMgr.state;
    const handlebarData = {
      pluggingSuretyDetails,
      measurementTypeToggleIsChecked:
       pluggingSuretyDetailsMeasurementType === constants.MeasurementType_TMD,
      editmode,
    };
    const t = template(handlebarData);
    $(`#${this.container}`).empty().append(t);
    $('#measurementTypeToggle').bootstrapToggle({
      on: constants.MeasurementType_TMD,
      off: constants.MeasurementType_TVD,
      onstyle: 'secondary',
      offstyle: 'secondary',
      size: 'xs',
    });
      if (editmode && $('#pluggingSuretyTableTbody').find('tr').length && $('#pluggingSuretyTableTbody').find('tr').length > 1)  {
          $('#pluggingSuretyTableTbody').sortable({
              axis: 'y',
              update:  () => {
                  this.reorderSuretyTable($('#pluggingSuretyTableTbody').sortable("toArray").map((x) => parseInt(x.split("_").pop(),10)));
              }
          });
          $('#pluggingSuretyTableTbody').css('cursor', 'grab');
      }
  }
}
