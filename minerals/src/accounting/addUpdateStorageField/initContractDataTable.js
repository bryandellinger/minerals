/* eslint-disable no-shadow */
export default function initConractDataTable(data) {
  const table = $('#storageFieldsModalTable').DataTable(
    {
      data,
      dom: 'lBfrtip',
      orderCellsTop: true,
      fixedHeader: true,
      pageLength: 5,
      lengthMenu: [[5, 10, 20, -1], [5, 10, 20, 'Show All']],
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
          data: 'storageId', name: 'storageId', autoWidth: true, render: '<button>select</button>',
        },
        { data: 'contractNum', name: 'contractNum', autoWidth: true },
        { data: 'altIdInformation', name: 'altIdInformation', autoWidth: true },
        { data: 'leaseNum', name: 'leaseNum', autoWidth: true },
        { data: 'lesseeName', name: 'lesseeName', autoWidth: true },
        {
          data: 'effectiveDate',
          name: 'effectiveDate',
          type: 'date',
          autoWidth: true,
          render(data) {
            if (data) {
              return data.toLocaleDateString().replace(/\u200E/g, '');
            }
            return '';
          },
        },
        {
          data: 'terminationDate',
          name: 'terminationDate',
          type: 'date',
          autoWidth: true,
          render(data) {
            if (data) {
              return data.toLocaleDateString().replace(/\u200E/g, '');
            }
            return '';
          },
        },
        {
          data: 'expirationDate',
          name: 'expirationDate',
          type: 'date',
          autoWidth: true,
          render(data) {
            if (data) {
              return data.toLocaleDateString().replace(/\u200E/g, '');
            }
            return '';
          },
        },
      ],
    },
  );
  return table;
}
