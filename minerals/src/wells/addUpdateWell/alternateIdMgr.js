import * as constants from '../../constants';

const template = require('../../views/wells/addUpdateWell/alternateId.handlebars');

export default class AlternateIdMgr {
  constructor(addUpdateWellMgr) {
    this.addUpdateWellMgr = addUpdateWellMgr;
    this.container = 'alternateIdContainer';
  }

  render() {
    const {
      alternateIdDropDownLabel, well, editmode,
    } = this.addUpdateWellMgr.state;
    const handlebarData = {
      alternateIdDropDownLabel,
      choices: [
        constants.AlternateIdDropDownLabel_NA,
        constants.AlternateIdDropDownLabel_Internal,
        constants.AlternateIdDropDownLabel_Operator,
      ],
      well,
      alternateIdRequiredInd: alternateIdDropDownLabel !== constants.AlternateIdDropDownLabel_NA,
      editmode,
    };
    const t = template(handlebarData);
    $(`#${this.container}`).empty().append(t);
    this.initAltIdChoiceChange();
  }

  initAltIdChoiceChange() {
    $('.altIdChoice').click((e) => {
      e.preventDefault();
      const { well } = this.addUpdateWellMgr.state;
      well.altId = null;
      well.altIdType = $(e.target).text();
      this.addUpdateWellMgr.setState(
        {
          ...this.addUpdateWellMgr.state, well, alternateIdDropDownLabel: $(e.target).text(),
        },
      );
      this.render();
    });
  }
}
