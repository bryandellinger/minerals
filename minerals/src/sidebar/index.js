/* eslint-disable no-unused-expressions */
/* eslint-disable prefer-destructuring */
/* eslint-disable class-methods-use-this */
/* eslint-disable dot-notation */
import getUrlVars from '../services/getUrlVars';

const template = require('../views/sidebar/index.handlebars');

export default class SideBarMgr {
    constructor(card) {
        this.state = {
            menuItems: [
                {
                    name: 'Search Checks',
                    value: 'manageChecksContainer',
                    selected: true,
                    card: 'accounting',
                },
                {
                    name: 'Add Check',
                    value: 'addUpdateCheckContainer',
                    selected: false,
                    card: 'accounting',
                }, 
                {
                    name: 'Search Royalties',
                    value: 'managePaymentsContainer',
                    selected: false,
                    card: 'accounting',
                }, 
                {
                    name: 'Add Royalty',
                    value: 'addUpdatePaymentContainer',
                    selected: false,
                    card: 'accounting',
                }, 
                {
                    name: 'Search Storage',
                    value: 'manageStorageFieldsContainer',
                    selected: false,
                    card: 'accounting',
                }, 
                {
                    name: 'Add Storage',
                    value: 'addUpdateStorageFieldContainer',
                    selected: false,
                    card: 'accounting',
                }, 
                {
                    name: 'Search Rental Payments',
                    value: 'manageContractRentalsContainer',
                    selected: false,
                    card: 'accounting',
                }, 
                {
                    name: 'Add Rental Payments',
                    value: 'addUpdateContractRentalContainer',
                    selected: false,
                    card: 'accounting',
                }, 
                {
                    name: 'Search Other Payments',
                    value: 'manageOtherPaymentsContainer',
                    selected: false,
                    card: 'accounting',
                },
                {
                    name: 'Add Other Payment',
                    value: 'addUpdateOtherPaymentContainer',
                    selected: false,
                    card: 'accounting',
                }, 
                {
                    name: 'Search Payment Uploads',
                    value: 'manageUploadPaymentsContainer',
                    selected: false,
                    card: 'accounting',
                },
                {
                    name: 'Add Payment Upload',
                    value: 'addUpdateUploadPaymentContainer',
                    selected: false,
                    card: 'accounting',
                }, 
                {
                    name: 'Search Contracts',
                    value: 'manageContractsContainer',
                    selected: true,
                    card: 'contracts',
                },
                {
                    name: 'Add Contract',
                    value: 'addUpdateContractContainer',
                    selected: false,
                    card: 'contracts',
                },
                {
                    name: 'Manage Lessees',
                    value: 'manageLesseesContainer',
                    selected: false,
                    card: 'contracts',
                },
                {
                    name: 'Manage Tracts',
                    value: 'manageTractsContainer',
                    selected: false,
                    card: 'contracts',
                },
                {
                    name: 'Search Wells',
                    value: 'manageWellsContainer',
                    selected: true,
                    card: 'wells',
                },
                {
                    name: 'Add Well',
                    value: 'addUpdateWellContainer',
                    selected: false,
                    card: 'wells',
                },
                {
                    name: 'Search Units',
                    value: 'manageUnitsContainer',
                    selected: true,
                    card: 'units',
                },
                {
                    name: 'Add Unit',
                    value: 'addUpdateUnitContainer',
                    selected: false,
                    card: 'units',
                },
                {
                    name: 'Search Templates',
                    value: 'manageTemplatesContainer',
                    selected: true,
                    card: 'administration',
                },
                {
                    name: 'Add Template',
                    value: 'addUpdateTemplateContainer',
                    selected: false,
                    card: 'administration',
                },
                {
                    name: 'Search Surety',
                    value: 'manageSuretiesContainer',
                    selected: true,
                    card: 'surety',
                },
                {
                    name: 'Add Surety',
                    value: 'addUpdateSuretyContainer',
                    selected: false,
                    card: 'surety',
                },
            ],
            activeContainer: null,
            menuBtnVisibleInd: false,
        };
        this.card = card;
    }

    setState(state) {
        this.state = state;
    }

    init() {
        this.filterMenuItems();
        this.checkUrl();
        this.initSelectMenuItem();
        this.render();
    }

    filterMenuItems() {
        const { menuItems } = this.state;
        const filteredMenuItems = menuItems.filter((x) => x.card === this.card);
        const activeContainer = filteredMenuItems.filter((x) => x.selected)[0].value;
        this.setState({ ...this.state, menuItems: filteredMenuItems, activeContainer });
    }

    // eslint-disable-next-line class-methods-use-this
    initHideMenu() {
        $('#sidemenutogglebtn').click(() => {
            const { activeContainer } = this.state;
            $('#accountsidebar').hide('slide', { direction: 'up' }, 1, () => {
                $(`#${activeContainer}`).find('.showmenubtn').show();
                $(`#${activeContainer}`).switchClass('col-10', 'col-12', 1);
                this.setState({ ...this.state, menuBtnVisibleInd: true });
            });
        });
    }

    initShowMenu() {
        $('body').on('click', '.showmenubtn', () => {
            const { activeContainer } = this.state;
            $(`#${activeContainer}`).show();
            $('.showmenubtn').hide();
            $(`#${activeContainer}`).switchClass('col-12', 'col-10', 1);
            $('#accountsidebar').show('slide', { direction: 'down' }, 1);
            this.setState({ ...this.state, menuBtnVisibleInd: false });
        });
    }

    checkUrl() {
        const { activeContainer } = this.state;
        const container = getUrlVars()['container'];
        if (container) {
            $('.accountContainer').hide();
            $(`#${container}`).show();
            this.setState({ ...this.state, activeContainer: container });
            this.setActiveMenuItem(container);
            this.render();
        } else {
            this.setState({ ...this.state, activeContainer });
        }
    }

    setVisibleContainer(container) {
        const { menuBtnVisibleInd } = this.state;
        $('.accountContainer').hide();
        if (menuBtnVisibleInd) {
            $(`#${container}`).switchClass('col-10', 'col-12', 1);
        } else {
            $(`#${container}`).switchClass('col-12', 'col-10', 1);
        }
        $(`#${container}`).show();
        this.setState({ ...this.state, activeContainer: container });
    }

  initSelectMenuItem() {
    $('#accountsidebar').on('click', '.spa-btn', (event) => {
      const container = $(event.target).val();
      $('.accountContainer').hide();
      $(`#${container}`).show();
      this.setActiveMenuItem($(event.target).val());
      this.setState({ ...this.state, activeContainer: container });
      this.render();
    });
  }

  watchContainer() {
    $('body').on('DOMNodeInserted', '.showmenubtn', () => {
      const { activeContainer, menuBtnVisibleInd } = this.state;
      const button = $(`#${activeContainer}`).find('.showmenubtn');
      menuBtnVisibleInd ? button.show() : button.hide();
    });
  }

  setActiveMenuItem(value) {
    const { menuItems } = this.state;
    const newMenuItems = menuItems.map(
      (menuItem) => (
        { ...menuItem, selected: menuItem.value === value }
      ),
    );
    this.setState({ ...this.state, menuItems: newMenuItems });
  }

    render() {
    const {
        menuItems,
        menuBtnVisibleInd,
        activeContainer
    } = this.state;
    const t = template({
      menuItems,
    });
    $('#accountsidebar').empty().append(t);
    this.initHideMenu();
    this.initShowMenu();
    this.watchContainer();
      if (menuBtnVisibleInd) {
          $(`#${activeContainer}`).switchClass('col-10', 'col-12', 1);
      } else {
          $(`#${activeContainer}`).switchClass('col-12', 'col-10', 1);
      }
  }

}
