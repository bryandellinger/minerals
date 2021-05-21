/* eslint-disable no-shadow */
export default function initLesseeDataTable(data) {
    const table = $('#wellsTable').DataTable(
    {
      data,
            dom: 'lBfrtip',
            orderCellsTop: true,
            fixedHeader: true,
      buttons: [
        'copyHtml5',
        'excelHtml5',
        'csvHtml5',
        'pdfHtml5',
      ],
      searching: true,
      order: [[2, 'asc']],
      columns: [
        { data: null },
          { data: 'wellId' },
          { data: 'wellNum' },
          { data: 'apiNum' },
          { data: 'tractNum' },
          { data: 'lesseeName' },
          { data: 'status' },
          { data: 'altId' },
      ],
      columnDefs: [
        {
          targets: 0,
          data: null,
          defaultContent: '<button>select</button>',
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
