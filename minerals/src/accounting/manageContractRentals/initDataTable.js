/* eslint-disable no-shadow */
export default function initDataTable(data) {
  const table = $('#contractRentalsTable').DataTable(
    {
      data,
      dom: 'lBfrtip',
      orderCellsTop: true,
      fixedHeader: true,
      order: [[4, 'desc'], [0,'desc']],
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
          { data: 'contractNum', name: 'contractNum', autoWidth: true },
          { data: 'tractNum', name: 'tractNum', autoWidth: true }, 
          {
              data: 'contractRentPay',
              name: 'contractRentPay',
              autoWidth: true,
              render: $.fn.dataTable.render.number(',', '.', 2),
          },
          {
              data: 'contractRentalEntryDate',
              name: 'contractRentalEntryDate',
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
              data: 'checkDate',
              name: 'checkDate',
              type: 'date',
              autoWidth: true,
              render(data) {
                  if (data) {
                      return data.toLocaleDateString().replace(/\u200E/g, '');
                  }
                  return '';
              },
          },
          { data: 'lesseeName', name: 'lesseeName', autoWidth: true },
          { data: 'contractPaymentPeriodYear', name: 'contractPaymentPeriodYear', autoWidth: true },
          { data: 'periodTypeName', name: 'periodTypeName', autoWidth: true },
          { data: 'heldByProduction', name: 'heldByProduction', autoWidth: true },
      ],
      },
  );
  return table;
}
