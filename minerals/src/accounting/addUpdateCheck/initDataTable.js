/* eslint-disable no-shadow */
export default function initDataTable(data) {
  const table = $('#checkPaymentsTable').DataTable(
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
      order: [[5, 'desc']],
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
        { data: 'lesseeName', name: 'lesseeName', autoWidth: true },
        { data: 'wellNum', name: 'wellNum', autoWidth: true },
        { data: 'apiNum', name: 'apiNum', autoWidth: true },
        {
          data: 'gasRoyalty',
          name: 'gasRoyalty',
          autoWidth: true,
          render: $.fn.dataTable.render.number(',', '.', 2),
        },
        {
          data: 'checkDate',
          name: 'checkDate',
          autoWidth: true,
          render(data) {
            if (data) {
              const d = new Date(data);
                return d.toLocaleDateString().replace(/\u200E/g, "");
            }
            return '';
          },
          },
          { data: 'paymentTypeName', name: 'paymentTypeName', autoWidth: true }, 
          { data: 'tractNum', name: 'tractNum', autoWidth: true }, 
          {
              data: 'postMonthYear',
              name: 'postMonthYear',
              autoWidth: true,
              render(data) {
                  if (data) {
                      return `${data.getMonth() + 1}/${data.getFullYear()}`;
                  }
                  else {
                      return '';
                  }
              },
          },
      ],
    },
  );
  return table;
}
