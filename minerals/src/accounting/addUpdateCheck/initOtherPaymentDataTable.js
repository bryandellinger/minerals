/* eslint-disable no-shadow */
export default function initDataTable(data) {
    const table = $('#checkOtherPaymentsTable').DataTable(
        {
            data,
            dom: 'lBfrtip',
            orderCellsTop: true,
            fixedHeader: true,
            order: [[3, 'desc'], [0, 'desc']],
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
                {
                    data: 'otherPaymentType', name: 'otherPaymentType', autoWidth: true
                },
                {
                    data: 'otherRentPay',
                    name: 'otherRentPay',
                    autoWidth: true,
                    render: $.fn.dataTable.render.number(',', '.', 2),
                },
                {
                    data: 'otherRentalEntryDate',
                    name: 'otherRentalEntryDate',
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
                { data: 'otherRentalNotes', name: 'otherRentalNotes', autoWidth: true },
            ],
        },
    );
    return table;
}
