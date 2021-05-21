export default function updateCheckSubmissionPeriods(
  checkSubmissionPeriod, checkSubmissionPeriods,
) {
  return checkSubmissionPeriods.map(
    (x) => (
      {
        ...x,
        checked: x.value === checkSubmissionPeriod,
      }
    ),
  );
}
