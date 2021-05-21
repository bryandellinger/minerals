/* eslint-disable no-shadow */
export default function initDataTable(data) {
 

    const table = $('#checksTable').DataTable(
        {
            "dom": 'lBfrtip',
            "orderCellsTop": true,
            "fixedHeader": true,
            "buttons": [
                'copyHtml5',
                'excelHtml5',
                'csvHtml5',
                'pdfHtml5',
            ],
            "processing": true,
            "serverSide": true,
            "filter": true,
            "ajax": {
                "url": "./api/CheckDataTableApi",
                "type": "POST",
                "datatype": "json"
            },
            "columnDefs": [
            {
                    "targets": [0],
                    "data": null,
                    "defaultContent": '<button>select</button>',
                    "searchable": false,
                    "orderable": false
            },
            {
                "targets": [1],
                "visible": false,
                "searchable": false
            },
            {
              "targets": [4],
               "searchable": false
            },
            {
              "targets": [5],
              "searchable": false
            },
            {
             "targets": [6],
             "searchable": false
            },
            ],
            "columns": [
                { "data": "id", "name": "id", "autoWidth": true, "render": '<button>select</button>' },
                { "data": "id", "name": "id", "autoWidth": true },
                { "data": "checkNum", "name": "checkNum", "autoWidth": true },
                { "data": "lesseeName", "name": "lesseeName", "autoWidth": true },
                {
                    "data": "totalAmount", "name": "totalAmount", "autoWidth": true,
                    "render": $.fn.dataTable.render.number(',', '.', 2)
                },
                {
                    "data": "receivedDate", "name": "receivedDate", "autoWidth": true,
                    "render": function (data) {
                        if (data) {
                            var d = new Date(data);
                            return d.toLocaleDateString().replace(/\u200E/g, "");
                        } else {
                            return '';
                        }                     
                    }                 
                },
                {
                    "data": "checkDate", "name": "checkDate", "autoWidth": true,
                    "render": function (data) {
                        if (data) {
                            var d = new Date(data);
                            return d.toLocaleDateString().replace(/\u200E/g, "");
                        } else {
                            return '';
                        }
                    }    
                },
            ]
        }
    );
    return table;
}
