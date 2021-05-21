export default function initCurrentUserModalBtn() {
  $('body').on('click', '#currentUserModal', () => {
    $('.hideonedit').show();
    $('.showonedit').hide();
    $('.enableonedit').attr('readonly', true).attr('disabled', true);
    ['NameFirst', 'NameLast', 'EmailAddress', 'WorkPhone', 'WorkAddr', 'Company', 'JobTitle'].forEach((item) => {
      $(`#${item}`).val($(`#${item}Original`).val());
    });
  });
}
