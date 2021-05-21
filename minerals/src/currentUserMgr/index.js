
import AjaxService from '../services/ajaxService';
import initEditButton from './initEditBtn';
import initCurrentUserModalBtn from './initCurrentUserModalBtn';

export default class CurrentUserMgr {
  constructor() {
    this.data = {};
    this.validator = null;
  }

  init() {
    initEditButton();
    initCurrentUserModalBtn();
    this.registerValidator();
  }

  registerValidator() {
    this.validator = $('#userForm').validate({
      rules: {
        nameFirst: {
          required: true,
        },
        nameLast: {
          required: true,
        },

      },
      messages: {
        nameFirst: {
          required: 'Please enter your first name',
        },
        nameLast: {
          required: 'Please enter your last name',
        },
        emailAddress: {
          required: 'Please enter a valid email',
          email: true,
        },
      },
      submitHandler: () => {
        this.submitForm();
        return false;
      },
    });
  }

  submitForm() {
    $($('#userForm').serializeArray()).each((index, obj) => {
      this.data[obj.name] = obj.value;
    });
    this.data.id = parseInt(this.data.id, 10);
    AjaxService.ajaxPut(`./api/UserMgrApi/${this.data.id}`, this.data)
      .then((d) => {
        this.data = d;
        window.Toastr.success('success', 'changes saved', { closeButton: true, positionClass: 'toast-top-center' });
        $('.hideonedit').show();
        $('.showonedit').hide();
        $('.enableonedit').attr('readonly', true).attr('disabled', true);
        $('.userName').text(`${d.nameFirst} ${d.nameLast}`);
        ['NameFirst', 'NameLast', 'EmailAddress', 'WorkPhone', 'WorkAddr', 'Company', 'JobTitle'].forEach((item) => {
          $(`#${item}Original`).val(d[item.charAt(0).toLowerCase() + item.slice(1)]);
        });
      })
      .catch(() => {});
  }
}
