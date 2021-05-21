import * as constants from '../../constants';
import initOwnershipHistoryDataTable from './initOwnershipHistoryDataTable';

export default class HistoricalOwnershipMgr {
  constructor(addUpdateWellMgr) {
    this.addUpdateWellMgr = addUpdateWellMgr;
    this.historicalOwnershipTable = null;
  }

  init() {
    const {
      historicalInfos, tracts, contracts, lessees,
      contractEventDetailReasonsForChange,
    } = this.addUpdateWellMgr.state;
    if (contractEventDetailReasonsForChange && contractEventDetailReasonsForChange.length) {
      const contractEventDetailReasonForChangeCorrectionId = contractEventDetailReasonsForChange
        .find(
          (x) => x.reason === constants.ContractEventDetailReasonForChangeCorrection,
        ).id;
      const filteredHistoricalInfos = historicalInfos.filter(
        (x) => x.contractEventDetailReasonForChangeId
       !== contractEventDetailReasonForChangeCorrectionId,
      );
      const data = filteredHistoricalInfos.map(
        (item) => (
          {
            ...item,
            changeDate: new Date(item.changeDate),
            reason: contractEventDetailReasonsForChange.find(
              (x) => x.id === item.contractEventDetailReasonForChangeId,
            ).reason,
            tractNum: tracts.find((x) => x.id === item.tractId).tractNum,
            contractNum: contracts.find((x) => x.id === item.contractId) ? contracts.find((x) => x.id === item.contractId).contractNum : '',
            lesseeName: item.lesseeName,
            percentOwnership: item.percentOwnership || '',
            royaltyPercent: item.royaltyPercent || '',
          }
        ),
      );
      this.historicalOwnershipTable = initOwnershipHistoryDataTable(data);
    }
  }
}
