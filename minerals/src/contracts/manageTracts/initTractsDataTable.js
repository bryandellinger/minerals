/* eslint-disable no-shadow */
export default function initTractsDataTable(data) {
  function commaSeparateNumber(val) {
    if (val) {
      while (/(\d+)(\d{3})/.test(val.toString())) {
        val = val.toString().replace(/(\d+)(\d{3})/, '$1' + ',' + '$2');
      }
      return val;
    }
    return '';
  }
  const table = $('#tractsTable').DataTable(
    {
      data,
      dom: 'lBfrtip',
      buttons: [
        'copyHtml5',
        'excelHtml5',
        'csvHtml5',
        'pdfHtml5',
      ],
      searching: true,
      pageLength: 3,
      lengthMenu: [[3, 10, 20, -1], [3, 10, 20, 'Show All']],
      order: [[2, 'asc']],
      columns: [
        { data: null },
        { data: 'id' },
        { data: 'tractNum' },
        { data: 'acreage' },
        { data: 'reversionDate' },
        { data: 'terminatedDate' },
        { data: 'terminated' },
        { data: 'altTractNum' },

      ],
      columnDefs: [
        {
          targets: 0,
          data: null,
          defaultContent: '<button><i class="fas fa-check"></i> select</button>',
        },
        {
          targets: [1],
          visible: false,
          searchable: false,
        },
        {
          render(data, type, row) { return commaSeparateNumber(data); },
          targets: [3],
        },
        {
          targets: [4, 5],
          type: 'date',
          render(data) { return data ? new Date(data).toLocaleDateString('en-US') : ''; },
        },
        {
          targets: [6],
          render(data) { return data ? 'terminated' : 'active'; },
        },
      ],
    },
  );
  return table;
}
