/* eslint-disable no-shadow */
export default function initDataTable(data) {
  const table = $('#suretiesTable').DataTable(
    {
      data,
      dom: 'lBfrtip',
      orderCellsTop: true,
      fixedHeader: true,
      order: [[0, 'desc']],
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
        { data: 'suretyTypeName', name: 'suretyTypeName', autoWidth: true },
        { data: 'suretyNum', name: 'suretyNum', autoWidth: true },
          { data: 'bondCategoryName', name: 'bondCategoryName', autoWidth: true },
          { data: 'suretyStatus', name: 'suretyStatus', autoWidth: true },
        {
          data: 'initialSuretyValue',
          name: 'initialSuretyValue',
          autoWidth: true,
          render: $.fn.dataTable.render.number(',', '.', 2),
        },
        {
          data: 'currentSuretyValue',
          name: 'currentSuretyValue',
          autoWidth: true,
          render: $.fn.dataTable.render.number(',', '.', 2),
        },
        {
          data: 'issueDate',
          name: 'issueDate',
          type: 'date',
          autoWidth: true,
          render(data) {
            if (data) {
              const d = new Date(data);
              return d.toLocaleDateString().replace(/\u200E/g, '');
            }
            return '';
          },
        },
        {
            data: 'releasedDate',
           name: 'releasedDate',
          type: 'date',
          autoWidth: true,
          render(data) {
            if (data) {
              const d = new Date(data);
              return d.toLocaleDateString().replace(/\u200E/g, '');
            }
            return '';
          },
        },
        { data: 'lesseeName', name: 'lesseeName', autoWidth: true },
          { data: 'insurer', name: 'insurer', autoWidth: true },
          { data: 'contractNum', name: 'contractNum', autoWidth: true },
       { data: 'wells', name: 'wells', autoWidth: true },
      ],
    },
  );
  return table;
}
