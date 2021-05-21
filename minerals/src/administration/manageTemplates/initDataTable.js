/* eslint-disable no-shadow */
export default function initDataTable(data) {
  const table = $('#uploadTemplatesTable').DataTable(
    {
      data,
      dom: 'lBfrtip',
      orderCellsTop: true,
      fixedHeader: true,
      order: [[1, 'asc']],
      buttons: [
        'copyHtml5',
        'excelHtml5',
        'csvHtml5',
        'pdfHtml5',
      ],
      searching: true,
      columnDefs: [
        {
          targets: [0],
          defaultContent: '<button>select</button>',
          searchable: false,
          orderable: false,
        },
      ],
      columns: [
        {
          data: 'id', name: 'id', autoWidth: true, render: '<button>select</button>',
        },
        { data: 'templateName', name: 'templateName', autoWidth: true },
        { data: 'lesseeName', name: 'lesseeName', autoWidth: true },
        { data: 'templateNotes', name: 'templateNotes', autoWidth: true },
      ],
    },
  );
  return table;
}
