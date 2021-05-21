export default function (contractSubTypes, contractTypeId, contractSubTypeId) {
  return contractSubTypes.filter(
    (contractSubType) => contractSubType.contractTypeId === contractTypeId,
  )
    .map(
      (item) => (
        { ...item, selected: contractSubTypeId === item.id }
      ),
    );
}
