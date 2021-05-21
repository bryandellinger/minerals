/* eslint-disable no-shadow */
export default function initLesseeDataTable(data) {
    function commaSeparateNumber(x) {
        if (!x) {
            return '';
        }
        var parts = x.toString().split(".");
        parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        return parts.join(".");
    }

    const table = $('#unitsTable').DataTable(
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
      order: [[2, 'asc']],
      columns: [
        { data: null },
          { data: 'id' },
          { data: 'unitName' },
          { data: 'dpuAcres' },
          { data: 'copAcres' },
          { data: 'tractNum' },
          { data: 'wells' },
          { data: 'numOfWells' },
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
          {
              render(data, type, row) { return commaSeparateNumber(data); },
              targets: [3,4],
          },
      ],
    },
  );
  return table;
}
