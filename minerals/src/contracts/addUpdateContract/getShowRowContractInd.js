import * as constants from '../../constants';

export default function getShowRowContractInd(contractTypes, contractSubTypes, contract) {
  const allowedContractSubTypeNames = [
    constants.ContractSubType_Coal,
    constants.ContractSubType_DgsLand,
    constants.ContractSubType_GasStorageField,
    constants.ContractSubType_OilAndGas,
    constants.ContractSubType_StateForest,
    constants.ContractSubType_StateForestAndPark,
    constants.ContractSubType_StatePark,
    constants.ContractSubType_NSDLease,
  ];
  const allowedContractSubtypeIds = contractSubTypes.filter(
    (x) => allowedContractSubTypeNames.includes(
      x.contractSubTypeName,
    ),
  ).map((y) => y.id);

  return contract.contractSubTypeId && allowedContractSubtypeIds.includes(
    contract.contractSubTypeId,
  );
}
