/* eslint-disable no-shadow */
export default function initDataTable(data) {
  const table = $('#uploadPaymentsTable').DataTable(
    {
      data,
      dom: 'lBfrtip',
      orderCellsTop: true,
      fixedHeader: true,
      order: [ [4,'desc']],
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
          { data: 'fileName', name: 'fileName', autoWidth: true },
          { data: 'templateName', name: 'templateName', autoWidth: true },
          { data: 'lesseeName', name: 'lesseeName', autoWidth: true },
          {
              data: 'uploadPaymentEntryDate',
              name: 'uploadPaymentEntryDate',
              type: 'date',
              autoWidth: true,
              render(data) {
                  if (data) {
                      return data.toLocaleDateString().replace(/\u200E/g, '');
                  }
                  return '';
              },
          },
          { data: 'checkNum', name: 'checkNum', autoWidth: true },
          {
              data: 'totalAmount',
              name: 'totalAmont',
              autoWidth: true,
              render: $.fn.dataTable.render.number(',', '.', 2),
          },
      ],
      },
  );
  return table;
}
