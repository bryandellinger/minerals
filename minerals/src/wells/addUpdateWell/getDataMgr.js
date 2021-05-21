import * as constants from '../../constants';

export default class GetDataMgr {
  constructor(AddUpdateWellMgr) {
    this.addUpdateWellMgr = AddUpdateWellMgr;
  }

  getData() {
    let apiStatecodesFromApi = null;
    $('.spinner').show();
    $.when(
      $.get(`./api/WellMgrApi/${this.addUpdateWellMgr.id}`, (d) => {
        if (d) {
          const well = {
            ...d,
            id: this.addUpdateWellMgr.id,
            spudDate: d.spudDate ? new Date(d.spudDate) : null,
            initialProductionDate:
             d.initialProductionDate ? new Date(d.initialProductionDate) : null,
            bofAppDate: d.bofAppDate ? new Date(d.bofAppDate) : null,
            plugDate: d.plugDate ? new Date(d.plugDate) : null,
            shutInDate: d.shutInDate ? new Date(d.shutInDate) : null,
            completionDate: d.completionDate ? new Date(d.completionDate) : null,
          };
          this.addUpdateWellMgr.setState(
            { ...this.addUpdateWellMgr.state, well, alternateIdDropDownLabel: well.altIdType },
          );
        }
      }),
      $.get('./api/ContractMgrApi', (contracts) => {
        this.addUpdateWellMgr.setState(
          { ...this.addUpdateWellMgr.state, contracts },
        );
      }),
      $.get(`./api/WellTractInformationMgrApi/welltractinfobywell/${this.addUpdateWellMgr.id}`, (wellTractInfos) => {
        this.addUpdateWellMgr.setState(
          {
            ...this.addUpdateWellMgr.state,
            wellTractInfos: wellTractInfos.filter((x) => x.activeInd) || [],
            historicalInfos: wellTractInfos.filter((x) => !x.activeInd) || [],
            wellTractInfosExistInd: wellTractInfos && wellTractInfos.length,
          },
        );
      }),
      $.get(`./api/WellLogTestTypeMgrApi/digitallogsbywell/${this.addUpdateWellMgr.id}`, (d) => {
        this.addUpdateWellMgr.setState(
          { ...this.addUpdateWellMgr.state, digitalLogs: d || [] },
        );
      }),
      $.get(`./api/WellLogTestTypeMgrApi/digitalimagelogsbywell/${this.addUpdateWellMgr.id}`, (d) => {
        this.addUpdateWellMgr.setState(
          { ...this.addUpdateWellMgr.state, digitalImageLogs: d || [] },
        );
      }),
      $.get(`./api/TractUnitJunctionWellJunctionMgrApi/unitsbywell/${this.addUpdateWellMgr.id}`, (d) => {
        this.addUpdateWellMgr.setState(
          { ...this.addUpdateWellMgr.state, units: d || [] },
        );
      }),
      $.get(`./api/WellLogTestTypeMgrApi/hardcopylogsbywell/${this.addUpdateWellMgr.id}`, (d) => {
        this.addUpdateWellMgr.setState(
          { ...this.addUpdateWellMgr.state, hardCopyLogs: d || [] },
        );
      }),
      $.get(`./api/WellBoreShareMgrApi/wellboresharessbywell/${this.addUpdateWellMgr.id}`, (d) => {
        this.addUpdateWellMgr.setState(
          { ...this.addUpdateWellMgr.state, wellboreShares: d || [] },
        );
      }),
        $.get(`./api/SuretyMgrApi/GetSuretiesByWell/${this.addUpdateWellMgr.id}`, (d) => {
            this.addUpdateWellMgr.setState(
                { ...this.addUpdateWellMgr.state, sureties: d || [] },
            );
        }),
      $.get('./api/tractMgrApi', (d) => {
        const tracts = d.filter((x) => !x.terminated);
        this.addUpdateWellMgr.setState({ ...this.addUpdateWellMgr.state, tracts });
      }),
      $.get('./api/padMgrApi', (pads) => {
        this.addUpdateWellMgr.setState({ ...this.addUpdateWellMgr.state, pads });
      }),
      $.get('./api/WellLogTestTypeMgrApi', (wellLogTestTypes) => {
        this.addUpdateWellMgr.setState({ ...this.addUpdateWellMgr.state, wellLogTestTypes });
      }),
      $.get('./api/WellStatusApi', (wellStatuses) => {
        this.addUpdateWellMgr.setState({ ...this.addUpdateWellMgr.state, wellStatuses });
      }),
      $.get('./api/FormationMgrApi', (formations) => {
        this.addUpdateWellMgr.setState(
          {
            ...this.addUpdateWellMgr.state,
            producingFormations: formations,
            deepestFormations: formations,
          },
        );
      }),
      $.get('./api/WellTypeMgrApi', (wellTypes) => {
        this.addUpdateWellMgr.setState({ ...this.addUpdateWellMgr.state, wellTypes });
      }),
      $.get('./api/ApiCodeMgrApi/statecodes', (d) => {
        apiStatecodesFromApi = d;
      }),
      $.get('./api/LesseeMgrApi', (lessees) => {
        this.addUpdateWellMgr.setState({ ...this.addUpdateWellMgr.state, lessees });
      }),
      $.get('./api/ContractEventDetailReasonMgrApi', (contractEventDetailReasonsForChange) => {
        this.addUpdateWellMgr.setState({
          ...this.addUpdateWellMgr.state, contractEventDetailReasonsForChange,
        });
      }),
      $.get('./api/townshipMgrApi', (d) => {
        function compare(a, b) {
          // Use toUpperCase() to ignore character casing
          const nameA = a.municipality.toUpperCase();
          const nameB = b.municipality.toUpperCase();

          let comparison = 0;
          if (nameA > nameB) {
            comparison = 1;
          } else if (nameA < nameB) {
            comparison = -1;
          }
          return comparison;
        }
        const sortedtownships = d.sort(compare);
        const townships = sortedtownships.map(
          (township) => (
            {
              ...township,
              name: `${township.municipality} (${township.county})`,
            }
          ),
        );
        this.addUpdateWellMgr.setState({
          ...this.addUpdateWellMgr.state, townships,
        });
      }),
    ).then(() => {
      const { well } = this.addUpdateWellMgr.state;
      let selectedApiStateCode = constants.ApiStateCode_Pennsylvania;
      if (well.apiNum) {
        selectedApiStateCode = parseInt(well.apiNum.split('-')[0], 10);
        this.addUpdateWellMgr.setState({
          ...this.addUpdateWellMgr.state,
          apiCountyCode: well.apiNum.split('-').length > 1 && parseInt(well.apiNum.split('-')[1], 10),
          apiUniqueIdentifier: well.apiNum.split('-').length > 2 && well.apiNum.split('-')[2],
          directionalSideTrackCode: (well.apiNum.split('-').length > 3 && well.apiNum.split('-')[3]) || '00',
          eventSequenceCode: (well.apiNum.split('-').length > 4 && well.apiNum.split('-')[4]) || '00',
        });
      }
      const apiStates = apiStatecodesFromApi.map(
        (apiState) => (
          {
            ...apiState,
            text: `${apiState.stateCode.toString().padStart(2, '0')}(${apiState.stateName})`,
            selected: apiState.stateCode === selectedApiStateCode,
          }
        ),
      );
      this.addUpdateWellMgr.setState(
        { ...this.addUpdateWellMgr.state, apiStates, apistateCode: selectedApiStateCode },
      );
        $('.spinner').hide();
     
        this.addUpdateWellMgr.renderMgr.render();
    });
  }
}
