import * as constants from '../../constants';

export default function getShowProducerPriceIndexInd(
    contractTypes, contractSubTypes, contract,
) {
    const allowedContractTypeNames = [
        constants.ContractType_LandLease,
        constants.ContractType_SUA,
        constants.ContractType_Production,
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