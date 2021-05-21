/* eslint-disable no-shadow */
export default function initDataTable(data) {
  const table = $('#paymentsTable').DataTable(
    {
      dom: 'lBfrtip',
      orderCellsTop: true,
      fixedHeader: true,
      buttons: [
        'copyHtml5',
        'excelHtml5',
        'csvHtml5',
        'pdfHtml5',
      ],
      processing: true,
      serverSide: true,
      filter: true,
      ajax: {
        url: './api/RoyaltyPaymentDataTableApi',
        type: 'POST',
        datatype: 'json',
        data(data) {
          data.from = $('#from').val();
          data.to = $('#to').val();
          data.paymentTypeId = parseInt($('#paymentTypeDropDownId').val(), 10);
        },
      },
      columnDefs: [
        {
          targets: [0],
          defaultContent: '<button>select</button>',
          searchable: false,
          orderable: false,
          },
          {
              targets: [11],
              searchable: false,
              orderable: false,
          },
      ],
      columns: [
        {
          data: 'id', name: 'id', autoWidth: true, render: '<button>select</button>',
        },
        { data: 'checkNum', name: 'checkNum', autoWidth: true },
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
              data: 'entryDate',
              name: 'entryDate',
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
        { data: 'paymentTypeName', name: 'paymentTypeName', autoWidth: true },
        { data: 'tractNum', name: 'tractNum', autoWidth: true },
        {
          data: 'postMonth',
          name: 'postMonth',
          autoWidth: true,
          render(data) {
            if (data) {
              const d = new Date(data);
              return `${d.getMonth() + 1}/${d.getFullYear()}`;
            }

            return '';
          },
          },
          { data: 'unitNames', name: 'unitNames', autoWidth: true },
      ],
    },
  );
  return table;
}
