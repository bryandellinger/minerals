/* eslint-disable no-shadow */
export default function initDataTable(data, editmode) {
  const table = $('#csvPaymentsTable').DataTable(
    {
      initComplete() {
        const api = this.api();

        if (!editmode) {
          // Hide Office column
          api.column(0).visible(false);
        }
      },
      data,
      dom: 'lBfrtip',
      orderCellsTop: true,
      fixedHeader: true,
      order: [[0, 'asc']],
      buttons: [
        'copyHtml5',
        'excelHtml5',
        'csvHtml5',
        'pdfHtml5',
      ],
      language: {
        emptyTable: 'No Data found. Please place in edit mode,  upload a payment, and click get payments from csv button.',
      },
      searching: true,
      columnDefs: [
        {
          targets: [0],
          defaultContent: '<button type="button">edit</button>',
          searchable: false,
          orderable: false,
        },
      ],
      columns: [
        {
          data: 'id', name: 'id', autoWidth: true, render: '<button type="button">edit</button>',
        },
        { data: 'apiNum', name: 'apiNum', autoWidth: true },
        {
          data: 'gasProd',
          name: 'gasProd',
          autoWidth: true,
          render: $.fn.dataTable.render.number(',', '.', 2),
        },
        {
          data: 'gasRoyalty',
          name: 'gasRoyalty',
          autoWidth: true,
          render: $.fn.dataTable.render.number(',', '.', 2),
        },
        {
          data: 'salesPrice',
          name: 'salesPrice',
          autoWidth: true,
          render: $.fn.dataTable.render.number(',', '.', 2),
        },
        {
          data: 'nri',
          name: 'nri',
          autoWidth: true,
          render: $.fn.dataTable.render.number(',', '.', 9),
          },
          { data: 'postYear', name: 'postYear', autoWidth: true },
          { data: 'postMonth', name: 'postMonth', autoWidth: true },
        { data: 'error', name: 'error', autoWidth: true },
      ],
    },
  );
  return table;
}
