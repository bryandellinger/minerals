import AjaxService from '../../services/ajaxService';
import elementsToBeValidated from './elementsToBeValidated';

const template = require('../../views/wells/addUpdateWell/apiNum.handlebars');

export default class ApiNumMgr {
  constructor(addUpdateWellMgr) {
    this.addUpdateWellMgr = addUpdateWellMgr;
    this.container = 'apiNumContainer';
  }

  getCountyCodes() {
    $('.spinner').show();
    const {
      apiCountyCode,
    } = this.addUpdateWellMgr.state;

    AjaxService.ajaxGet(`./api/ApiCodeMgrApi/countycodes/${this.addUpdateWellMgr.state.apiStateCode}`)
      .then((d) => {
        const apiCounties = d.map(
          (apiCounty) => (
            {
              ...apiCounty,
              text: `${apiCounty.countyCode.toString().padStart(3, '0')}(${apiCounty.countyName})`,
              selected: apiCounty.countyCode === apiCountyCode,
            }
          ),
        );
        this.addUpdateWellMgr.setState({
          ...this.addUpdateWellMgr.state, apiCounties,
        });
        $('.spinner').hide();
        this.render();
      });
  }

  render() {
    const {
      apiStates, apiCounties, apiUniqueIdentifier, directionalSideTrackCode, eventSequenceCode,
    } = this.addUpdateWellMgr.state;
    const handlebarData = {
      apiStates,
      apiCounties,
      apiUniqueIdentifier,
      directionalSideTrackCode,
      eventSequenceCode,
      derivedApiNum: this.getDerivedApiNum(),
    };
    const t = template(handlebarData);
    $(`#${this.container}`).empty().append(t);
    $('#apiStateCode').select2({ width: '100%' });
    $('#apiStateCounty').select2({ width: '100%', placeholder: 'Select a county' });
    this.initApiStateSelectChange();
    this.initApiCountySelectChange();
    this.initApiNumInputChange();
  }

  initApiStateSelectChange() {
    $('#apiStateCode').change((e) => {
      const data = $(e.target).select2('data');
      const { apiStates } = this.addUpdateWellMgr.state;
      const newApiStates = apiStates.map(
        (apiState) => (
          {
            ...apiState,
            text: `${apiState.stateCode.toString().padStart(2, '0')}(${apiState.stateName})`,
            selected: apiState.stateCode === parseInt(data[0].id, 10),
          }
        ),
      );
      this.addUpdateWellMgr.setState({
        ...this.addUpdateWellMgr.state,
        apiStateCode: parseInt(data[0].id, 10),
        apiCountyCode: 0,
        apiStates: newApiStates,
      });
      this.getCountyCodes();
    });
  }

  initApiCountySelectChange() {
    $('#apiStateCounty').change((e) => {
      const data = $(e.target).select2('data');
      this.addUpdateWellMgr.setState({
        ...this.addUpdateWellMgr.state, apiCountyCode: parseInt(data[0].id, 10),
      });
      this.displayDerivedApiNum();
    });
  }

  getDerivedApiNum() {
    const {
      apiStateCode, apiCountyCode, apiUniqueIdentifier, directionalSideTrackCode, eventSequenceCode,
    } = this.addUpdateWellMgr.state;
    return `${apiStateCode.toString().padStart(2, '0')}-${apiCountyCode.toString().padStart(3, '0') || 'xxx'}-${apiUniqueIdentifier || 'xxxxx'}-${directionalSideTrackCode || '00'}-${eventSequenceCode || '00'}`;
  }

  displayDerivedApiNum() {
    $('#derivedApiNum').val(this.getDerivedApiNum());
  }


  initApiNumInputChange() {
    $('.apiNumInput').change((e) => {
      const element = $(e.target).attr('id');
      this.addUpdateWellMgr.state[element] = $(e.target).val();
      if (elementsToBeValidated.find((x) => x.element === element)) {
        this.addUpdateWellMgr.doesElementHaveError(element);
      }
      this.displayDerivedApiNum();
    });
  }
}
