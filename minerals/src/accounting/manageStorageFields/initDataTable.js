/* eslint-disable no-shadow */
export default function initDataTable(data) {
  const table = $('#storageFieldsTable').DataTable(
    {
      data,
      dom: 'lBfrtip',
      orderCellsTop: true,
      fixedHeader: true,
      order: [[5, 'desc'], [0, 'desc']],
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
        { data: 'altIdInformation', name: 'altIdInformation', autoWidth: true },
        { data: 'company', name: 'company', autoWidth: true },
        {
          data: 'rentPay',
          name: 'rentPay',
          autoWidth: true,
          render: $.fn.dataTable.render.number(',', '.', 2),
        },
        {
            data: 'storageRentalEntryDate',
            name: 'storageRentalEntryDate',
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
              data: 'receivedDate',
              name: 'receivedDate',
              type: 'date',
              autoWidth: true,
              render(data) {
                  if (data) {
                      return data.toLocaleDateString().replace(/\u200E/g, '');
                  }
                  return '';
              },
          },
          { data: 'leaseNum', name: 'leaseNum', autoWidth: true },
          { data: 'lesseeName', name: 'lesseeName', autoWidth: true },
          { data: 'paymentPeriodYear', name: 'paymentPeriodYear', autoWidth: true },
          { data: 'periodTypeName', name: 'periodTypeName', autoWidth: true },
          { data: 'storageRentalPaymentTypeName', name: 'StorageRentalPaymentTypeName', autoWidth: true }
      ],
    },
  );
  return table;
}
