import * as constants from '../../constants';

export default function getShowSuretyInd(
    contractTypes, contractSubTypes, contract,
) {
    const allowedContractTypeNames = [
        constants.ContractType_LandLease,
        constants.ContractType_SUA,
        constants.ContractType_Production,
        constants.ContractType_Seismic,
    ];
    const allowedContractTypeIds = contractTypes.filter(
        (x) => allowedContractTypeNames.includes(
            x.contractTypeName,
        ),
    ).map((y) => y.id);
    return contract.contractTypeId && allowedContractTypeIds.includes(
        contract.contractTypeId,
    );
}