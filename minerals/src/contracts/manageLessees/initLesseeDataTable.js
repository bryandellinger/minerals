/* eslint-disable no-shadow */
export default function initLesseeDataTable(data) {
  const table = $('#lesseeTable').DataTable(
    {
      data,
      dom: 'lBfrtip',
      buttons: [
        'copyHtml5',
        'excelHtml5',
        'csvHtml5',
        'pdfHtml5',
      ],
      searching: true,
      pageLength: 3,
      lengthMenu: [[3, 10, 20, -1], [3, 10, 20, 'Show All']],
      order: [[2, 'asc']],
      columns: [
        { data: null },
        { data: 'id' },
        { data: 'lesseeName' },
      ],
      columnDefs: [
        {
          targets: 0,
          data: null,
          defaultContent: '<button><i class="fas fa-check"></i> select</button>',
        },
        {
          targets: [1],
          visible: false,
          searchable: false,
        },
      ],
    },
  );
  return table;
}
