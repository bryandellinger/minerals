import AjaxService from '../../services/ajaxService';
import formatFullAddress from './formatFullAddress';
import contactElementsToBeValidated from './contactElementsToBeValidated';

const template = require('../../views/contracts/manageLessees/lesseeContact.handlebars');


export default class ContactMgr {
  constructor(manageLeseesMgr) {
    this.manageLeseesMgr = manageLeseesMgr;
    this.container = 'contacttablecontainer';
  }

  getData(id) {
    $('.spinner').show();
    AjaxService.ajaxGet(`./api/LesseesContactMgrApi/${id}`)
      .then((lesseeContact) => {
        if (lesseeContact) {
          this.manageLeseesMgr.setState(
            {
              ...this.manageLeseesMgr.state,
              lesseeContact,
              contactEditMode: false,
              contactSubmitInd: false,
            },
          );
        } else {
          this.manageLeseesMgr.setState(
            {
              ...this.manageLeseesMgr.state,
              contactEditMode: true,
              lesseeContact: { id: 0, lesseeId: this.manageLeseesMgr.state.lessee.id },
              contactSubmitInd: false,
            },
          );
        }
        this.render();
        $('.spinner').hide();
      });
  }

  render() {
    const { lesseeContact, lessee, contactEditMode } = this.manageLeseesMgr.state;
    const data = {
      title: ` ${lesseeContact.id ? 'contact information for ' : 'add contact'}`,
      editmode: contactEditMode,
      lessee,
      lesseeContact,
      mapUrl: `https://www.google.com/maps/search/?api=1&query=${encodeURI(formatFullAddress(lesseeContact, false))}`,
    };
    const t = template(data);
    $(`#${this.container}`).empty().append(t);
    this.initStateAutoComplete();
    this.initCloseButton();
    this.initInputChange();
    this.initFormSubmit();
    this.initDeleteButton();
    this.initEditButton();
  }

  initEditButton() {
    $('#editContactInfo').click(() => {
      this.manageLeseesMgr.setState(
        {
          ...this.manageLeseesMgr.state,
          contactEditMode: true,
        },
      );
      this.render();
    });
  }

  initStateAutoComplete() {
    const { states } = this.manageLeseesMgr.state;
    const source = Array.from(new Set(states.map((x) => (x.code))));
    const sourceWithNoNulls = source.filter((e) => e === 0 || e).sort();
    $('#state').autocomplete({
      source: sourceWithNoNulls,
      minLength: 0,
      select: (event, ui) => {
        const { lesseeContact } = this.manageLeseesMgr.state;
        lesseeContact.state = ui.item.label;
        this.manageLeseesMgr.setState(
          { ...this.manageLeseesMgr.state, lesseeContact },
        );
      },
    // eslint-disable-next-line func-names
    }).focus(function () {
      $(this).autocomplete('search', '');
    });
  }

  initCloseButton() {
    $('#closeContactInfo, #cancelContactChangesBtn').click(() => {
      this.manageLeseesMgr.getLesseeContactData(this.manageLeseesMgr.state.lessee);
    });
  }

  initInputChange() {
    $('.contactInput').change((e) => {
      const { lesseeContact } = this.manageLeseesMgr.state;
      const element = $(e.target).attr('id');
      lesseeContact[element] = $(e.target).val();
      this.manageLeseesMgr.setState({ ...this.manageLeseesMgr.state, lesseeContact });
      this.doesElementHaveError(element);
    });
  }

  initDeleteButton() {
    $('#deleteContactBtn').click(() => {
      const { lesseeContact, lessee } = this.manageLeseesMgr.state;
      $('.spinner').show();
      AjaxService.ajaxDelete(`./api/LesseesContactMgrApi/${lesseeContact.id}`)
        // eslint-disable-next-line no-unused-vars
        .then((d) => {
          window.Toastr.success(`${lesseeContact.firstName} ${lesseeContact.lastName} has been deleted`, 'Delete Successful', { positionClass: 'toast-top-center' });
          $('.spinner').hide();
          this.manageLeseesMgr.getLesseeContactData(lessee);
        })
        .catch(() => {
          $('.spinner').hide();
        });
    });
  }

  doesElementHaveError(element) {
    const { contactSubmitInd } = this.manageLeseesMgr.state;
    const elementToValidate = contactElementsToBeValidated.find((x) => x.element === element);
    if (!elementToValidate) {
      return false;
    }
    if (!$(`#${element}`).length) {
      return false;
    }
    $(`#${element}Error`).hide();
    $(`#${element}`).removeClass('is-invalid');

    if (elementToValidate.showOnlyIfFormSubmitted && !contactSubmitInd) {
      return false;
    }

    if (!$(`#${element}`)[0].checkValidity()) {
      $(`#${element}`).addClass('is-invalid');
      $(`#${element}Error`).show();
      return true;
    }
    return false;
  }

  initFormSubmit() {
    // eslint-disable-next-line consistent-return
    $('#addUpdateLesseeContactForm').submit(() => {
      this.manageLeseesMgr.setState({ ...this.manageLeseesMgr.state, contactSubmitInd: true });
      let formHasError = false;
      // eslint-disable-next-line no-restricted-syntax
      for (const item of contactElementsToBeValidated) {
        if (this.doesElementHaveError(item.element)) {
          formHasError = true;
        }
      }
      if (formHasError) {
        window.Toastr.error('Please fix errors and resubmit');
        return false;
      }
      $('.spinner').show();
      AjaxService.ajaxPost('./api/LesseesContactMgrApi', this.manageLeseesMgr.state.lesseeContact)
        .then((d) => {
          window.Toastr.success('Your contact has been saved', 'Save Successful', { positionClass: 'toast-top-center' });
          $('.spinner').hide();
          this.getData(d.id);
        })
        .catch(() => {
          $('.spinner').hide();
        });
    });
  }
}
