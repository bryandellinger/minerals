/* eslint-disable no-shadow */
export default function initSuretyDataTable(data) {
    const table = $('#contractSuretyTable').DataTable(
    {
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
            searching: true,
            columns: [
                {
                    data: 'suretyNum', name: 'suretyNum', autoWidth: true,
                    render: function (data, type, row, meta) {
                        if (type === 'display') {
                            data = '<a href="./Surety?container=addUpdateSuretyContainer&suretyId=' + row.id +'&contractId=' + row.contractId +'"><i class="fas fa-link"></i>' + data + '</a>';
                        }
                        return data;
                    }
                },
        {
          data: 'initialSuretyValue',
          name: 'initialSuretyValue',
          autoWidth: true,
          render: $.fn.dataTable.render.number(',', '.', 2),
        },
        {
          data: 'currentSuretyValue',
          name: 'currentSuretyValue',
          autoWidth: true,
          render: $.fn.dataTable.render.number(',', '.', 2),
        },
        {
          data: 'issueDate',
          name: 'issueDate',
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
            data: 'releasedDate',
           name: 'releasedDate',
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
        { data: 'suretyStatus', name: 'suretyStatus', autoWidth: true },
      ],
    },
  );
  return table;
}
