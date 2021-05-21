import * as constants from '../../constants';

export default function getShowCheckSubmissionPeriodInd(
  contractTypes, contractSubTypes, contract,
) {
  const allowedContractSubTypeNames = [
    constants.ContractSubType_Streambed,
    constants.ContractSubType_DgsLand,
    constants.ContractSubType_StateForest,
    constants.ContractSubType_StateForestAndPark,
    constants.ContractSubType_StatePark,
    constants.ContractSubType_NSDLease,
    ];

    const allowedContractTypeNames = [
        constants.ContractType_Production,
    ];

  const allowedContractSubtypeIds = contractSubTypes.filter(
    (x) => allowedContractSubTypeNames.includes(
      x.contractSubTypeName,
    ),
    ).map((y) => y.id);

    const allowedContractTypeIds = contractTypes.filter(
        (x) => allowedContractTypeNames.includes(
            x.contractTypeName,
        ),
    ).map((y) => y.id);

    const allowedSubTypes = contract.contractSubTypeId && allowedContractSubtypeIds.includes(
    contract.contractSubTypeId,
    );

    const allowedTypes = contract.contractTypeId && allowedContractTypeIds.includes(
        contract.contractTypeId,
    );

    return allowedSubTypes || allowedTypes;
}
