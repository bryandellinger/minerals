export default function getShowAltIdInformationInd(agreement, altIdCategories) {
  return agreement && agreement.altIdCategoryId
  && altIdCategories.find(
    (x) => x.id === agreement.altIdCategoryId,
  ).altIdName !== 'Not Applicable';
}
