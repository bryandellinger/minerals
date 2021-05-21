export default function initEditBtn() {
  $('body').on('click', '#currentUserEditBtn', () => {
    $('.hideonedit').hide();
    $('.showonedit').show();
    $('.enableonedit').removeAttr('readonly').removeAttr('disabled');
  });
}
