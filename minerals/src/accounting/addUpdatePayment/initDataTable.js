/* eslint-disable no-shadow */
export default function initDataTable(data) {
  const table = $('#paymentWellTractInfoTable').DataTable(
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
          data: 'id', name: 'id', autoWidth: true, render: '<button>select</button>',
        },
        { data: 'wellNum', name: 'wellNum', autoWidth: true },
        { data: 'apiNum', name: 'apiNum', autoWidth: true },
        { data: 'tractNum', name: 'tractNum', autoWidth: true },
        { data: 'padName', name: 'padName', autoWidth: true },
        { data: 'contractNum', name: 'contractNum', autoWidth: true },
        { data: 'percentOwnership', name: 'percentOwnership', autoWidth: true },
        { data: 'royaltyPercent', name: 'royaltyPercent', autoWidth: true },
        { data: 'unitNames', name: 'unitNames', autoWidth: true },
      ],
    },
  );
  return table;
}
