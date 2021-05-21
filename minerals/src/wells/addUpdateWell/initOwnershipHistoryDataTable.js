/* eslint-disable no-shadow */
export default function initOwnershipHistoryDataTable(data) {
  const table = $('#historicalOwnershipTable').DataTable(
    {
      data,
      dom: 'lBfrtip',
      order: [[1, 'desc'], [0, 'asc']],
      buttons: [
        'copyHtml5',
        'excelHtml5',
        'csvHtml5',
        'pdfHtml5',
      ],
      columns: [
        { data: 'id' },
        { data: 'changeDate' },
        { data: 'reason' },
        { data: 'tractNum' },
        { data: 'contractNum' },
        { data: 'lesseeName' },
        { data: 'percentOwnership' },
        { data: 'royaltyPercent' },
      ],
      columnDefs: [
        {
          targets: [0],
          visible: false,
          searchable: false,
        },
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
