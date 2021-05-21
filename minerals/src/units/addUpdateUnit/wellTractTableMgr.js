/* eslint-disable no-plusplus */
import AjaxService from '../../services/ajaxService';

const template = require('../../views/units/addUpdateUnit/wellTractTable.handlebars');

export default class WellTractTableMgr {
  constructor(addUpdateUnitMgr) {
    this.addUpdateUnitMgr = addUpdateUnitMgr;
    this.container = 'wellTractTableContainer';
  }

  render() {
    const {
      editmode, tracts, tractUnitJunctions, wells,
    } = this.addUpdateUnitMgr.state;

    const AllTractUnitJunctionWellIds = tractUnitJunctions.map((x) => x.wellIds).reduce(
      (a, b) => a.concat(b),
      [],
    );

    const AllavailableWellIds = tractUnitJunctions.map((x) => x.availableWellIds).reduce(
      (a, b) => a.concat(b),
      [],
    );

    const uniqueAvailableWellIds = Array.from(new Set(AllavailableWellIds));

    const uniqueTractUnitJunctionWellIds = Array.from(new Set(AllTractUnitJunctionWellIds));

    const wellsForUnits = wells.filter((x) => uniqueAvailableWellIds.includes(x.id)).map(
      (well) => (
        {
          ...well,
          selected: uniqueTractUnitJunctionWellIds.includes(well.id),
        }
      ),
    );

    const handlebarData = {
      editmode,
      contractsUrl: window.location.href.split('?')[0].replace('Units', 'Contracts'),
      tractUnitJunctions: tractUnitJunctions.map(
        (tractUnitJunction) => (
          {
            ...tractUnitJunction,
            tracts: tracts.map(
              (tract) => (
                {
                  ...tract,
                  name: tract.administrative ? `${tract.tractNum}(admin)` : tract.tractNum,
                  selected: tract.id === tractUnitJunction.tractId,
                }
              ),
            ),
            wells: wells.filter((x) => tractUnitJunction.availableWellIds.includes(x.id)).map(
              (well) => (
                {
                  ...well,
                  selected: tractUnitJunction.wellIds.includes(well.id),
                }
              ),
            ),
          }
        ),
      ),
      wells: wellsForUnits,
    };
    const t = template(handlebarData);
    $(`#${this.container}`).empty().append(t);
    $('#wellTractInfoTbody .tractId').select2({ placeholder: 'Select a tract', width: '100%' });
    $('#wellTractInfoTfoot .select2-selectall').select2({ width: '100%', closeOnSelect: false });
    this.initCreateTractUnitJunctionLinkClick();
    this.initAddTractUnitJunction();
    this.initDeleteTractUnitJunction();
    this.initTractUnitJunctionFromTractSelectChange();
    this.initSelectAll();
    this.initMultiSelectChange();
    this.initCopAcresChange();
    this.initMultiSelectLink();
    this.addUpdateUnitMgr.calculatePrivateAcreage();

    if (handlebarData.editmode && $('#wellTractInfoTbody').find('tr').length && $('#wellTractInfoTbody').find('tr').length > 1) {
      $('#wellTractInfoTbody').sortable({
        axis: 'y',
        update: () => {
          this.reorderWellTractInfoTable($('#wellTractInfoTbody').sortable('toArray').map((x) => parseInt(x.split('_').pop(), 10)));
        },
      });
      $('#wellTractInfoTbody').css('cursor', 'grab');
    }
    this.updateWells();
  }

  updateWells() {
    const { tractUnitJunctions } = this.addUpdateUnitMgr.state;
    if ($('#wellTractInfoTfoot .select2-selectall').length) {
      const ids = $('#wellTractInfoTfoot .select2-selectall').select2('data').map((x) => (parseInt(x.id, 10))).filter((x) => x > 0);
      let i;
      for (i = 0; i < tractUnitJunctions.length; i++) {
        tractUnitJunctions[i].wellIds = ids;
      }
    }
    this.addUpdateUnitMgr.setState(
      {
        ...this.addUpdateUnitMgr.state,
        tractUnitJunctions,
      },
    );
  }

  initMultiSelectLink() {
    const { wells, editmode, unit } = this.addUpdateUnitMgr.state;
    if (!editmode) {
      const wellsUrl = window.location.href.split('?')[0].replace('Units', 'Wells');
      const arr = $('span.select2-selection__choice__remove').toArray();
      // eslint-disable-next-line no-restricted-syntax
      for (const item of arr) {
        const li = $(item).closest('li');
        const title = $(li).attr('title');
        const re = /\((.*)\)/i;
        const apiNum = title.match(re)[1];
        const well = wells.find((x) => x.apiNum === apiNum);
        $(`<a href="${wellsUrl}?container=addUpdateWellContainer&wellId=${well.id}&unitId=${unit.id}"><i class="fas fa-link"></i></a>`).insertAfter(item);
      }
    }
  }

  initSelectAll() {
    $('#wellTractInfoTfoot .select2-selectall').on('select2:select', (e) => {
      const { tractUnitJunctions } = this.addUpdateUnitMgr.state;
      const { data } = e.params;
      if (parseInt(data.id, 10) === 0) {
        $('.select2-selectall').select2('close');
        let index;
        for (index = 0; index < tractUnitJunctions.length; index++) {
          tractUnitJunctions[index].wellIds = [...tractUnitJunctions[index].availableWellIds];
        }
        this.addUpdateUnitMgr.setState(
          {
            ...this.addUpdateUnitMgr.state,
            tractUnitJunctions,
          },
        );
        this.render();
      }
    });
  }

  initCopAcresChange() {
    $('#wellTractInfoTbody .copAcres').change((e) => {
      const { tractUnitJunctions } = this.addUpdateUnitMgr.state;
      const index = $(e.target).closest('tr').index();
      const input = $('.copAcres').eq(index);
      const error = $('.copAcresError').eq(index);
      error.hide();
      input.removeClass('is-invalid');
      const copAcres = parseFloat($(e.target).val() || 0);
      tractUnitJunctions[index].copAcres = copAcres;
      this.addUpdateUnitMgr.setState(
        {
          ...this.addUpdateUnitMgr.state,
          tractUnitJunctions,
        },
      );
      if (!input[0].checkValidity()) {
        input.addClass('is-invalid');
        error.show();
      }
      this.addUpdateUnitMgr.calculatePrivateAcreage();
    });
  }

  initMultiSelectChange() {
    $('#wellTractInfoTfoot .select2-selectall').on('change', (e) => {
      const { tractUnitJunctions } = this.addUpdateUnitMgr.state;
      // const index = $(e.target).closest('tr').index();
      const ids = $(e.target).select2('data').map((x) => (parseInt(x.id, 10))).filter((x) => x > 0);
      let index;
      for (index = 0; index < tractUnitJunctions.length; index++) {
        tractUnitJunctions[index].wellIds = ids;
      }
      this.addUpdateUnitMgr.setState(
        {
          ...this.addUpdateUnitMgr.state,
          tractUnitJunctions,
        },
      );
      this.addUpdateUnitMgr.calculatePrivateAcreage();
    });
  }


  initTractUnitJunctionFromTractSelectChange() {
    $('#wellTractInfoTbody .tractId').change((e) => {
      const { tractUnitJunctions, unit } = this.addUpdateUnitMgr.state;
      const data = $(e.target).select2('data');
      const index = $(e.target).closest('tr').index();
      $('.spinner').show();
      AjaxService.ajaxGet(`./api/TractUnitJunctionMgrApi/junctionbytract/${data[0].id}`)
        .then((tractUnitJunction) => {
          $('.tractIdError').eq(index).hide();
          $($('.tractId').eq(index).data('select2').$container).removeClass('is-invalid');
          tractUnitJunctions[index] = tractUnitJunction || {
            id: 0,
            unitId: unit.id,
            tractId: parseInt(data[0].id, 10),
            copAcres: 0,
            wellIds: [],
availableWellIds            : [],
          };

          this.addUpdateUnitMgr.setState(
            {
              ...this.addUpdateUnitMgr.state,
              tractUnitJunctions,
            },
          );

          $('.spinner').hide();
          this.render();
        });
    });
  }

  reorderWellTractInfoTable(indexArray) {
    const { tractUnitJunctions } = this.addUpdateUnitMgr.state;
    const reorderedTractUnitJunctions = [];
    indexArray.forEach((index) => reorderedTractUnitJunctions.push(tractUnitJunctions[index]));
    this.addUpdateUnitMgr.setState(
      { ...this.addUpdateUnitMgr.state, tractUnitJunctions: reorderedTractUnitJunctions },
    );
    this.render();
  }


  initCreateTractUnitJunctionLinkClick() {
    $('#createTractUnitJunctionLink').click((e) => {
      e.preventDefault();
      this.insertAt(0);
    });
  }

  initAddTractUnitJunction() {
    $('#wellTractInfoTbody .addTractUnitJunction').click((e) => {
      this.insertAt($(e.target).closest('tr').index());
    });
  }

  initDeleteTractUnitJunction() {
    $('#wellTractInfoTbody .removeTractUnitJunction').click((e) => {
      const { tractUnitJunctions } = this.addUpdateUnitMgr.state;
      const index = $(e.target).closest('tr').index();
      // eslint-disable-next-line no-unused-vars
      const removed = tractUnitJunctions.splice(index, 1);
      this.addUpdateUnitMgr.setState(
        {
          ...this.addUpdateUnitMgr.state,
          tractUnitJunctions,
        },
      );
      this.render();
    });
  }

  insertAt(index) {
    const {
      tractUnitJunctions,
      unit,
    } = this.addUpdateUnitMgr.state;
    tractUnitJunctions.splice(index + 1, 0, {
      id: 0,
      unitId: unit.id,
      tractId: 0,
      copAcres: 0,
      wellIds: [],
      availableWellIds: [],
    });

    if ($('#wellTractInfoTfoot .select2-selectall').length) {
      const ids = $('#wellTractInfoTfoot .select2-selectall').select2('data').map((x) => (parseInt(x.id, 10))).filter((x) => x > 0);
      let i;
      for (i = 0; i < tractUnitJunctions.length; i++) {
        tractUnitJunctions[i].wellIds = ids;
      }
    }
    this.addUpdateUnitMgr.setState(
      { ...this.addUpdateUnitMgr.state, tractUnitJunctions },
    );
    this.render();
  }
}
