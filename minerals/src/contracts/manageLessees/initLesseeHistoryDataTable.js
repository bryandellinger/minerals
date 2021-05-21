/* eslint-disable no-shadow */
export default function initLesseeDataTable(data) {
  const table = $('#lesseeHistoryTable').DataTable(
    {
      searching: false,
      paging: false,
      info: false,
      data,
      columns: [
        { data: 'lesseeName' },
        { data: 'tableDate' },
      ],
      columnDefs: [
        {
          targets: [1],
          type: 'date',
          render(data) { return new Date(data).toLocaleDateString('en-US'); },
        },
      ],
    },
  );
  return table;
}
