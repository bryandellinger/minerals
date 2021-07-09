export default function addDataTableColumnFilter(datatableName, table, indexesToIgnore, width, ) {
    debugger;
    $(`#${datatableName} thead tr`).clone(true).appendTo(`#${datatableName} thead`);
    $(`#${datatableName} thead tr:eq(1) th`).each(function (i) {
        $(this).removeClass('sorting');
        $(this).removeClass('sorting_desc');
        $(this).removeClass('sorting_asc');
        var title = $(this).text();
        if (indexesToIgnore && indexesToIgnore.includes(i)) {
            $(this).html(``);
        } else {
            if (width) {
                $(this).html(`<input type="text" style="max-width: ${width}px;" />`);
            }
            else {
                $(this).html('<input type="text" />');
            }
        }


        $('input', this).on('keyup change', function () {
            if (table.column(i).search() !== this.value) {
                table
                    .column(i)
                    .search(this.value)
                    .draw();
            }
        });
    });
}
