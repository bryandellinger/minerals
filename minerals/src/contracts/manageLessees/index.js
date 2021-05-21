import initCardHeader from '../initCardHeader';
import AjaxService from '../../services/ajaxService';
import initState from './initState';
import formatFullAddress from './formatFullAddress';
import initLesseeDataTable from './initLesseeDataTable';
import initContactDataTable from './initContactDataTable';
import 'datatables.net/js/jquery.dataTables';
import 'datatables.net-buttons/js/dataTables.buttons';
import 'datatables.net-buttons/js/buttons.html5';
import 'datatables.net-buttons/js/buttons.flash';
import 'datatables.net-dt/css/jquery.dataTables.css';
import '../../datatablebutton.css';
import ContactMgr from './contactMgr';
import LesseeMgr from './lesseeMgr';

const template = require('../../views/contracts/manageLessees/index.handlebars');
const lesseeTableTemplate = require('../../views/contracts/manageLessees/lesseeTable.handlebars');
const contactTableTemplate = require('../../views/contracts/manageLessees/contactTable.handlebars');
const historyTableTemplate = require('../../views/contracts/manageLessees/historyTable.handlebars');
const lesseeTabTemplate = require('../../views/contracts/manageLessees/lesseeTabs.handlebars');

export default class ManageLeseesMgr {
  constructor(options) {
    this.sideBarMgr = options.sideBarMgr;
    this.container = $('#manageLesseesContainer');
    this.lesseetablecontainer = 'lesseetablecontainer';
    this.contacttablecontainer = 'contacttablecontainer';
    this.historytablecontainer = 'historytablecontainer';
    this.lesseetabcontainer = 'lesseeTabContainer';
    this.lesseetable = null;
    this.contacttable = null;
    this.contactMgr = null;
    this.lesseeMgr = null;
    this.initMgrs();
  }


  setState(state) {
    this.state = state;
  }

  initMgrs() {
    this.contactMgr = new ContactMgr(this);
    this.lesseeMgr = new LesseeMgr(this);
  }


    init(lessee, lesseeId) {
        this.state = initState();
        if (lesseeId) {
            this.setState({ ...this.state, addCloseButton: true });
            if (!lessee) {
                lessee = {
                    id: lesseeId
                }
            }
            else
            {
              lessee.id = lesseeId;
            }
            }
    this.render();
    this.getLesseeData(lessee);
    this.getStates();
  }

  getStates() {
    AjaxService.ajaxGet('./api/StateMgrApi')
      .then((d) => {
        function compare(a, b) {
        // Use toUpperCase() to ignore character casing
          const nameA = a.name.toUpperCase();
          const nameB = b.name.toUpperCase();

          let comparison = 0;
          if (nameA > nameB) {
            comparison = 1;
          } else if (nameA < nameB) {
            comparison = -1;
          }
          return comparison;
        }
        const states = d.sort(compare);

        this.setState({
          ...this.state, states,
        });
      });
  }


  getLesseeData(lessee) {
    AjaxService.ajaxGet('./api/LesseeMgrApi')
      .then((lessees) => {
          this.setState({ ...this.state, lessees });
          if (lessee) {
              if (!lessee.lesseeName) {
                  lessee.lesseeName = lessees.find(x => x.id === lessee.id).lesseeName;
              }
          }
        this.renderLesseeTable(lessee);
      });
  }

  getLesseeContactData(lessee) {
    $('.spinner').show();
    AjaxService.ajaxGet(`./api/LesseesContactMgrApi/lesseesContactsbylessee/${lessee.id}`)
      .then((d) => {
        const lesseeContacts = d || [];
        const lesseeContactsWithMap = lesseeContacts.map(
          (lesseeContact) => (
            {
              ...lesseeContact,
              fullAddress: formatFullAddress(lesseeContact, true),
              mapUrl: `https://www.google.com/maps/search/?api=1&query=${encodeURI(formatFullAddress(lesseeContact, false))}`,

            }
          ),
        );
        this.setState({
          ...this.state,
          lesseeContacts: lesseeContactsWithMap,
          lessee,
          historyFormEditMode: false,
          legalNameChangeInd: true,
        });
        this.renderLesseeTabs();
        this.renderContactTable();
        this.renderHistoryTable();
        $('.spinner').hide();
      });
  }

  render() {
    const t = template({ headerInfo: initCardHeader() });
    this.container.empty().append(t);
  }

  renderLesseeTabs() {
    const { lessee } = this.state;
    const tabTemplate = lesseeTabTemplate(lessee.lesseeName);
    $(`#${this.lesseetabcontainer}`).empty().append(tabTemplate);
  }

  renderLesseeTable(lessee) {
    const { lessees } = this.state;
    const lesseetemplate = lesseeTableTemplate();
    $(`#${this.lesseetablecontainer}`).empty().append(lesseetemplate);
    this.lesseetable = initLesseeDataTable(lessees);
    this.lesseeMgr.addLesseeButton();
    this.initContactClick();
    this.lesseeMgr.initAddLesseeClick();
    if (lessee) {
      this.getLesseeContactData(lessee);
      $('#lesseeTable').DataTable().search(lessee.lesseeName).draw();
    }
  }

  renderHistoryTable() {
    const { lessee } = this.state;
    const data = { lessee };
    const historytemplate = historyTableTemplate(data);
    $(`#${this.historytablecontainer}`).empty().append(historytemplate);
    this.lesseeMgr.renderHistoryForm();
  }

  renderContactTable() {
    const { lesseeContacts, lessee } = this.state;
    const contacttemplate = contactTableTemplate(lessee.lesseeName);
    $(`#${this.contacttablecontainer}`).empty().append(contacttemplate);
    this.contacttable = initContactDataTable(lesseeContacts, lessee);
    this.initEditClick();
    this.initAddContactClick();
    $('html, body').animate({ scrollTop: $(document).height() }, 'slow');
  }


  initContactClick() {
    $('#lesseeTable tbody').on('click', 'button', (event) => {
      const data = this.lesseetable.row($(event.target).parents('tr')).data();
      const lessee = { id: data.id, lesseeName: data.lesseeName };
      this.getLesseeContactData(lessee);
    });
  }

  initAddContactClick() {
    $('#addContactBtn').click(() => {
      this.contactMgr.getData(0);
    });
  }

  initEditClick() {
    $('#contactTable tbody').on('click', '.edit', (event) => {
      const data = this.contacttable.row($(event.target).parents('tr')).data();
      this.contactMgr.getData(data.id);
    });
  }
}
