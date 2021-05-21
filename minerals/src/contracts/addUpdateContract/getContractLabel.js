import * as constants from '../../constants';

export default function getContractLabel(
  contractTypes, contractSubTypes, contract,
) {
  let returnValue = constants.ContractLabel_Tract;
  const contractTypeId = contract && contract.contractTypeId ? contract.contractTypeId : 0;
  const contractSubTypeId = contract && contract.contractSubTypeId ? contract.contractSubTypeId : 0;
  const contractType = contractTypes.find((x) => x.id === contractTypeId);
  const contractSubType = contractSubTypes.find((x) => x.id === contractSubTypeId);
  const contractTypeName = contractType && contractType.contractTypeName ? contractType.contractTypeName : '';
  const contractSubTypeName = contractSubType && contractSubType.contractSubTypeName ? contractSubType.contractSubTypeName : '';
  const isSequence = contractSubType ? contractSubType.mapTypeOverride === 'Z' : contractTypes.find((x) => x.id === contract.contractTypeId && x.mapType === 'Z');
  if (isSequence) {
    returnValue = constants.ContractLabel_Sequence;
    if (contractTypeName === constants.ContractType_Prospecting) {
      returnValue = constants.ContractLabel_Area;
    }
    if (contractSubTypeName === constants.ContractSubType_GasStorageField) {
      returnValue = constants.ContractLabel_FieldNumber;
    }
  }
  return returnValue;
}
