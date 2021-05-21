import * as constants from '../../constants';

export default function getShowStorageFeeInd(
  contractTypes, contractSubTypes, contract,
) {
  const allowedContractSubTypeNames = [
    constants.ContractSubType_GasStorageField,
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
