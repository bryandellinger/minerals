import * as constants from '../../constants';

export default function getShowShutInInd(
    contractTypes, contractSubTypes, contract,
) {
    const allowedContractTypeNames = [
        constants.ContractType_LandLease,
        constants.ContractType_Production,
    ];
    const allowedContractTypeIds = contractTypes.filter(
        (x) => allowedContractTypeNames.includes(
            x.contractTypeName,
        ),
    ).map((y) => y.id);

    const allowedContractSubTypeNames = [
        constants.ContractSubType_Coal,
        constants.ContractSubType_DgsLand,
        constants.ContractSubType_OilAndGas,
        constants.ContractSubType_StateForest,
        constants.ContractSubType_StateForestAndPark,
        constants.ContractSubType_StatePark,
        constants.ContractSubType_Streambed,
        constants.ContractSubType_NSDLease,
        constants.ContractSubType_GasStorageField,
    ];

    const allowedContractSubTypeIds = contractSubTypes.filter(
        (x) => allowedContractSubTypeNames.includes(
            x.contractSubTypeName,
        ),
    ).map((y) => y.id);


    return contract.contractTypeId &&
              allowedContractTypeIds.includes(contract.contractTypeId) &&
            (
                !contract.contractSubTypeId ||
                allowedContractSubTypeIds.includes(contract.contractSubTypeId)
            );
}