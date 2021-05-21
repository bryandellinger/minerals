/* eslint-disable no-shadow */
export default function initContactDataTable(data) {
  const table = $('#contactTable').DataTable(
    {
      searching: false,
      paging: false,
      info: false,
      data,
      order: [[2, 'asc']],
      columns: [
        { data: null },
        { data: 'id' },
        {
          name: 'Name', data(row, type, set) { return `${row.lastName}, ${row.firstName}`; },
        },
        { data: 'title' },
        {
          name: 'Address',
          data(row, type, set) {
            return `<a href="${row.mapUrl}" style="color: #0000EE;" target="_blank">${row.fullAddress}</a>`;
          },
        },
        {
          name: 'Phone',
          data(row, type, set) {
            return `${row.phone1 ? `<i class="fas fa-phone"></i>&nbsp;${row.phone1}` : ''}
                                ${row.phone2 ? `</br><i class="fas fa-phone"></i>&nbsp;${row.phone2}` : ''}
                                ${row.fax ? `</br><i class="fas fa-fax"></i>&nbsp;${row.fax}` : ''}`;
          },
        },
        {
          name: 'Email',
          data(row, type, set) {
            return `${row.email ? `<a href="mailto:${row.email}" style="color: #0000EE;">${row.email}</a>` : ''}`;
          },
        },

      ],
      columnDefs: [
        {
          targets: 0,
          data: null,
          defaultContent: '<button class="edit"><i class="fas fa-check"></i> select</button>',
        },
        {
          targets: [1],
          visible: false,
          searchable: false,
        },
      ],
    },
  );
  return table;
}
