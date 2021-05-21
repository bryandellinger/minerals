/* eslint-disable no-shadow */
export default function initDataTable(data) {
  const table = $('#mytable').DataTable(
    {
      scrollX: true,
      data,
      dom: 'lBfrtip',
      buttons: [
        'copyHtml5',
        'excelHtml5',
        'csvHtml5',
        'pdfHtml5',
          ],
      order: [[1, 'desc']],
      searching: false,
      columns: [
        { data: null },
        { data: 'id' },
        { data: 'contractNum' },
        { data: 'contractTypeName' },
        { data: 'contractSubTypeName' },
        { data: 'tractNum' },
        { data: 'name' },
      ],
      columnDefs: [
        {
          targets: 0,
          data: null,
          defaultContent: '<button>select</button>',
        },
        {
          targets: [1],
          visible: false,
          searchable: false,
        },
        /* {
          targets: [6],
          type: 'date',
          render(data) { return new Date(data).toLocaleDateString('en-US'); },
        }, */
      ],
    },
  );
  return table;
}
