/* eslint-disable no-shadow */
export default function initDataTable(data) {
  const table = $('#adjustmentsTable').DataTable(
    {
      data,
      dom: 'lBfrtip',
      orderCellsTop: true,
      fixedHeader: true,
      pageLength: 3,
      lengthMenu: [[3, 10, 20, -1], [3, 10, 20, 'Show All']],
      buttons: [
        'copyHtml5',
        'excelHtml5',
        'csvHtml5',
        'pdfHtml5',
      ],
      searching: true,
      order: [[1, 'desc'], [0, 'desc']],
      columnDefs: [
        {
          targets: [0],
          defaultContent: '<button>select</button>',
          searchable: false,
        },
        { orderable: false, targets: [0, 1, 2, 3, 4, 5, 6, 7] },
      ],
      columns: [
        {
          data: 'id', name: 'id', autoWidth: true, render: '<button>select</button>',
        },
        {
          data: 'entryDate',
          name: 'entryDate',
          autoWidth: true,
          render(data) {
            if (data) {
              const d = new Date(data);
              return d.toLocaleDateString().replace(/\u200E/g, '');
            }
            return '';
          },
        },
        { data: 'checkNum', name: 'checkNum', autoWidth: true },
        {
          data: 'volumeWithChange',
          name: 'volumeWithChange',
          autoWidth: true,
        },
        {
          data: 'paymentWithChange',
          name: 'paymentWithChange',
          autoWidth: true,
        },
        {
          data: 'nriWithChange',
          name: 'nriWithChange',
          autoWidth: true,
        },
        {
          data: 'salesPriceWithChange',
          name: 'salesPriceWithChange',
          autoWidth: true,
        },
        {
          data: 'legacyDataInd',
          name: 'legacyDataInd',
          autoWidth: true,
          render(data) {
            if (data) {
              return 'yes';
            }
            return 'no';
          },
        },
      ],
    },
  );
  return table;
}
