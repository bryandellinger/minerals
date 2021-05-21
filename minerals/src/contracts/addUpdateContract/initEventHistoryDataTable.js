/* eslint-disable no-shadow */
export default function initEventHistoryDataTable(data) {
  const table = $('#historicalEventsTable').DataTable(
    {
      data,
      order: [[2, 'desc'], [4, 'asc']],
      dom: 'lBfrtip',
      buttons: [
        'copyHtml5',
        'excelHtml5',
        'csvHtml5',
        'pdfHtml5',
      ],
      columns: [
        { data: 'id' },
        { data: null },
        { data: 'effectiveDate' },
        { data: 'lesseeName' },
        { data: 'reason' },
        { data: 'acres' },
        { data: 'shareOfLeasePercentage' },
        { data: 'royaltyPercent' },
        { data: 'currentTotalAcres' },
        { data: 'previousTotalAcres' },
      ],
      columnDefs: [
        {
          targets: [0],
          visible: false,
          searchable: false,
        },
        {
          targets: [2],
          type: 'date',
          render(data) { return new Date(data).toLocaleDateString('en-US'); },
        },
        {
          targets: [1],
          class: 'details-control',
          data: null,
          searchable: false,
          orderable: false,
          defaultContent: '<i class="fas fa-info-circle"></i>',
        },
      ],
    },
  );
  return table;
}
