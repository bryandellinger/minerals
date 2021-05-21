/* eslint-disable no-shadow */
export default function initOwnershipHistoryDataTable(data) {
  const table = $('#historicalOwnershipTable').DataTable(
    {
      data,
      dom: 'lBfrtip',
      buttons: [
        'copyHtml5',
        'excelHtml5',
        'csvHtml5',
        'pdfHtml5',
      ],
          columns: [
         { data: 'id' },
         { data: 'link' },
        { data: 'wellNum' },
        { data: 'apiNum' },
        { data: 'tractNum' },
        { data: 'contractNum' },
        { data: 'lesseeName' },
        { data: 'percentOwnership' },
        { data: 'autoUpdatedAllowedInd' },
        ],
          columnDefs: [
              {
                  targets: [0],
                  visible: false,
                  searchable: false,
              },
              {
                  targets: [1],
                  searchable: false,
                  orderable: false,
              },
          ],
    },
  );
  return table;
}
