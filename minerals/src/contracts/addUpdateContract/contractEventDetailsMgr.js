import initEventHistoryDataTable from './initEventHistoryDataTable';
import getShowExtentOfOwnershipInd from './getShowExtentOfOwnership';
import * as constants from '../../constants';
import roundDecimals from '../../services/roundDecimals';

const template = require('../../views/contracts/addUpdateContract/contractEventDetails.handlebars');
const currentEventsTableTemplate = require('../../views/contracts/addUpdateContract/currentEventsTable.handlebars');
const historicalEventsTableTemplate = require('../../views/contracts/addUpdateContract/historicalEventsTable.handlebars');

export default class ContractEventDetailsMgr {
  constructor(AddUpdateContractMgr) {
    this.addUpdateContractMgr = AddUpdateContractMgr;
    this.container = 'contractEventDetailsContainer';
    this.currentEventsTableContainer = 'activeEventsTableContainer';
    this.historicalEventsTableContainer = 'historicalEventsTableContainer';
    this.historicalEventsTable = null;
    this.detailRows = [];
  }

  render() {
    const {
      editmode,
    } = this.addUpdateContractMgr.state;
    const handlebarData = {
      editmode,
    };
    const t = template(handlebarData);
    $(`#${this.container}`).empty().append(t);
    this.renderCurrentEventsTable();
    this.renderHistoricalEventsTable();
  }

  renderHistoricalEventsTable() {
    const {
      contractEventDetailsHistory,
      contractEventDetailReasonsForChange,
    } = this.addUpdateContractMgr.state;

    const t = historicalEventsTableTemplate();
    $(`#${this.historicalEventsTableContainer}`).empty().append(t);
    const data = contractEventDetailsHistory.map(
      (contractEventDetail) => (
        {
          ...contractEventDetail,
          reason: `${contractEventDetailReasonsForChange.find(
            (element) => element.id === contractEventDetail.contractEventDetailReasonForChangeId,
          ).reason} ${contractEventDetail.description ? `(${contractEventDetail.description})` : ''}`,
        }
      ),
    );

    this.historicalEventsTable = initEventHistoryDataTable(data);
    this.initShowHideDetails();
    this.addUpdateContractMgr.historicalOwnershipMgr.init();
  }

  // eslint-disable-next-line class-methods-use-this
  format(d) {
    return `Horizon: ${d.horizon || 'N/A'} <br>
    Top Vertical Extent of Ownership: ${d.topVerticalExtentOfOwnership || 'N/A'} <br>
    Bottom Vertical Extent of Ownership: ${d.bottomVerticalExtentOfOwnership || 'N/A'} <br>
    Excluded from Ownership: ${d.excludedFromVerticalExtentOfOwnership || 'N/A'} <br>
    Minimum Royalty: ${d.minimumRoyalty || '0.00'} <br>
    Minimum Royalty Sales Price: ${d.minimumRoyaltySalesPrice || '0.00'} <br>
    `;
  }

  initShowHideDetails() {
    this.detailRows = [];
    $('#historicalEventsTable tbody').on('click', 'tr td.details-control', (e) => {
      const tr = $(e.target).closest('tr');
      const row = this.historicalEventsTable.row(tr);
      const idx = $.inArray(tr.attr('id'), this.detailRows);
      if (row.child.isShown()) {
        // tr.removeClass('details');
        row.child.hide();

        // Remove from the 'open' array
        this.detailRows.splice(idx, 1);
      } else {
        // tr.addClass( 'details' );
        row.child(this.format(row.data())).show();

        // Add to the 'open' array
        if (idx === -1) {
          this.detailRows.push(tr.attr('id'));
        }
      }
    });
    // On each draw, loop over the `detailRows` array and show any child rows
    this.historicalEventsTable.on('draw', () => {
      $.each(this.detailRows, (i, id) => {
        $(`#${id} td.details-control`).trigger('click');
      });
    });
  }

  renderCurrentEventsTable() {
    const {
      editmode,
      contractEventDetails,
      lessees,
      contractEventDetailReasonsForChange,
      reasonForChangeId,
      contractTypes,
      contractSubTypes,
      contract,
      reasonForChangeDescription,
    } = this.addUpdateContractMgr.state;

    const handlebarData = {
      baseUrl: window.location.href.split('?')[0],
      showReasonForChangeDescriptionInd:
       reasonForChangeId === contractEventDetailReasonsForChange
         .find((x) => x.reason === constants.ContractEventDetailReasonForChange_LeaseAmendment)
         .id,
      reasonForChangeDescription,
      showExtentOfOwnershipInd:
       getShowExtentOfOwnershipInd(contractTypes, contractSubTypes, contract),
      editmode: reasonForChangeId
       || (editmode && (!contractEventDetails.filter((x) => x.id).length)),
      contractEventDetailReasonsForChange: contractEventDetailReasonsForChange.map(
        (reasonForChange) => (
          {
            ...reasonForChange,
            selected: reasonForChange.id === reasonForChangeId,
          }
        ),
      ),
      showReasonsForChangeInd: editmode
       && contractEventDetails.filter((x) => x.id).length,
      contractEventDetails: contractEventDetails.map(
        (contractEventDetail) => (
          {
            ...contractEventDetail,
            lessees: lessees.map(
              (lessee) => (
                {
                  ...lessee,
                  selected: lessee.id === contractEventDetail.lesseeId,
                }
              ),
            ),
          }
        ),
      ),
    };
    const t = currentEventsTableTemplate(handlebarData);
    $(`#${this.currentEventsTableContainer}`).empty().append(t);
    $('.eventDetailLesseeSelect').select2({ placeholder: 'Select a company' });
    $('#reasonForChange').select2({ placeholder: 'I am not changing the current lessee information' });
    $('.industryLeaseIndToggle').bootstrapToggle({
      on: 'Yes',
      off: 'No',
      onstyle: 'secondary',
      offstyle: 'secondary',
    });
    this.industryLeaseIndToggleChange();
    this.initEventDetailLesseeSelectChange();
    this.initAcreageChange();
    this.initRoyaltyPercentChange();
    this.initMinimumRoyaltyChange();
    this.initMinimumRoyaltySalesPriceChange();
    this.initMinimumRoyaltySalesPriceCheckboxChange();
    this.initShareOfLeasePercentageChange();
    this.initTopVerticalExtentOfOwnershipChange();
    this.initBottomVerticalExtentOfOwnershipChange();
    this.initInterestTypeChange();
    this.initHorizonChange();
    this.initExcludedFromVerticalExtentOfOwnershipChange();
    this.initCreateCurrentEventDetailLinkClick();
    this.initAddCurrentEvent();
    this.initDeleteCurrentEvent();
    this.initInterestTypeAutoComplete();
    this.initHorizonAutoComplete();
    this.initReasonForChangeSelectChange();
    this.initreasonForChangeDescriptionChange();
    if (handlebarData.editmode && $('#currentContractEventDetailsTableTbody').find('tr').length && $('#currentContractEventDetailsTableTbody').find('tr').length > 1) {
      $('#currentContractEventDetailsTableTbody').sortable({
        axis: 'y',
        update: () => {
          this.reorderCurrentContractEventDetailsTable($('#currentContractEventDetailsTableTbody').sortable('toArray').map((x) => parseInt(x.split('_').pop(), 10)));
        },
      });
      $('#currentContractEventDetailsTableTbody').css('cursor', 'grab');
    }
    this.addUpdateContractMgr.datePickerMgr.init('lesseeEffectiveDate', 'mm/dd/yy', true, true, true);
  }

  reorderCurrentContractEventDetailsTable(indexArray) {
    const { contractEventDetails } = this.addUpdateContractMgr.state;
    const reorderedContractEventDetails = [];
    indexArray.forEach((index) => reorderedContractEventDetails.push(contractEventDetails[index]));
    this.addUpdateContractMgr.setState(
      { ...this.addUpdateContractMgr.state, contractEventDetails: reorderedContractEventDetails },
    );
    this.renderCurrentEventsTable();
  }

  initreasonForChangeDescriptionChange() {
    $('#reasonForChangeDescription').change((e) => {
      this.addUpdateContractMgr.setState(
        { ...this.addUpdateContractMgr.state, reasonForChangeDescription: $(e.target).val() },
      );
    });
  }

  initReasonForChangeSelectChange() {
    $('#reasonForChange').change((e) => {
      const data = $(e.target).select2('data');
      this.addUpdateContractMgr.setState(
        {
          ...this.addUpdateContractMgr.state,
          reasonForChangeId: parseInt(data[0].id, 10),
          reasonForChangeDescription: null,
        },
      );
      this.renderCurrentEventsTable();
    });
  }


  initInterestTypeAutoComplete() {
    $('.interestType')
      .autocomplete(
        {
          source: ['Lease', 'Production'],
          minLength: 0,
          select: (event, ui) => {
            this.updateEventDetail($(event.target).closest('tr').index(), ui.item.label, 'interestType');
          },
        },
      )
    // trigger the search on focus
      // eslint-disable-next-line func-names
      .focus(function () {
        $(this).autocomplete('search', $(this).val());
      });
  }

  initHorizonAutoComplete() {
    $('.horizon')
      .autocomplete(
        {
          source: ['All', 'Deep', 'Shallow'],
          minLength: 0,
          select: (event, ui) => {
            this.updateEventDetail($(event.target).closest('tr').index(), ui.item.label, 'horizon');
          },
        },
      )
    // trigger the search on focus
    // eslint-disable-next-line func-names
      .focus(function () {
        $(this).autocomplete('search', $(this).val());
      });
  }

  initAddCurrentEvent() {
    $('.addCurentEvent').click((e) => {
      this.insertAt($(e.target).closest('tr').index());
      this.addUpdateContractMgr.setState({ ...this.addUpdateContractMgr.state, updateWellInfoInd: true });
    });
  }

  initCreateCurrentEventDetailLinkClick() {
    $(`#${this.currentEventsTableContainer}`).on('click', '#createCurrentEventDetailLink', (e) => {
      e.preventDefault();
      this.insertAt(0);
    });
  }

  initDeleteCurrentEvent() {
    $('.removeCurrentEvent').click((e) => {
      const { contractEventDetails } = this.addUpdateContractMgr.state;
      const index = $(e.target).closest('tr').index();
      // eslint-disable-next-line no-unused-vars
      const removed = contractEventDetails.splice(index, 1);
      this.addUpdateContractMgr.setState(
        {
          ...this.addUpdateContractMgr.state,
          contractEventDetails,
          updateWellInfoInd: true,
        },
      );
      this.render();
    });
  }

  insertAt(index) {
    const {
      contractEventDetails,
      contract,
    } = this.addUpdateContractMgr.state;
    contractEventDetails.splice(index + 1, 0, {
      id: 0,
      contractId: contract.id,
      lesseeName: null,
      topVerticalExtentOfOwnership: null,
      bottomVerticalExtentOfOwnership: null,
      ExcludedFromVerticalExtentOfOwnership: null,
      shareOfLeasePercentage: 0,
      acres: 0,
      activeInd: true,
      contractEventDetailReasonForChangeId: null,
      interestType: null,
      industryLeaseInd: false,
      royaltyPercent: 0,
      minimumRoyalty: 0,
      minimumRoyaltySalesPrice: 0,
      overrideMinimumRoyaltySalesPriceIn: false,
    });
    this.addUpdateContractMgr.setState(
      { ...this.addUpdateContractMgr.state, contractEventDetails },
    );
    this.renderCurrentEventsTable();
  }

  updateEventDetail(index, value, element, eventTarget) {
    const { contractEventDetails } = this.addUpdateContractMgr.state;
    contractEventDetails[index][element] = value;
    this.addUpdateContractMgr.setState(
      { ...this.addUpdateContractMgr.state, contractEventDetails },
    );
    this.doesElementHaveError(eventTarget);
  }

  // eslint-disable-next-line class-methods-use-this
  doesElementHaveError(eventTarget) {
    if (!eventTarget) {
      return false;
    }
    $(eventTarget).removeClass('is-invalid');
    $(eventTarget).next('.currentEventError').hide();
    if ($(eventTarget).data('select2')) {
      $($(eventTarget).$container).removeClass('is-invalid');
    }
    if (!eventTarget.checkValidity()) {
      $(eventTarget).addClass('is-invalid');
      if ($(eventTarget).data('select2')) {
        $($(eventTarget).data('select2').$container).addClass('is-invalid');
      }
      $(eventTarget).next('.currentEventError').show();
      return true;
    }
    return false;
  }

  initEventDetailLesseeSelectChange() {
    $('.eventDetailLesseeSelect').change((e) => {
      const data = $(e.target).select2('data');
      this.updateEventDetail($(e.target).closest('tr').index(), parseInt(data[0].id, 10), 'lesseeId', e.target);
      this.updateEventDetail($(e.target).closest('tr').index(), data[0].text, 'lesseeName', e.target);
      this.addUpdateContractMgr.setState(
        { ...this.addUpdateContractMgr.state, updateWellInfoInd: true },
      );
    });
  }

  initAcreageChange() {
    $('.eventDetailAcres').change((e) => {
      this.updateEventDetail($(e.target).closest('tr').index(), parseFloat($(e.target).val()), 'acres', e.target);
    });
  }

  initRoyaltyPercentChange() {
      $('.royaltyPercent').change((e) => {
          this.addUpdateContractMgr.setState(
              { ...this.addUpdateContractMgr.state, updateWellInfoInd: true },
          );
      this.updateEventDetail($(e.target).closest('tr').index(), parseFloat($(e.target).val()), 'royaltyPercent', e.target);
      this.updateMinimumRoyaltySalesPrice($(e.target).closest('tr').index());
    });
  }

  initMinimumRoyaltyChange() {
      $('.minimumRoyalty').change((e) => {
      this.updateEventDetail($(e.target).closest('tr').index(), parseFloat($(e.target).val()), 'minimumRoyalty', e.target);
      this.updateMinimumRoyaltySalesPrice($(e.target).closest('tr').index());
    });
  }

  initMinimumRoyaltySalesPriceChange() {
    $('.minimumRoyaltySalesPrice').change((e) => {
      this.updateEventDetail($(e.target).closest('tr').index(), parseFloat($(e.target).val()), 'minimumRoyaltySalesPrice', e.target);
    });
  }

  initMinimumRoyaltySalesPriceCheckboxChange() {
    $('.overrideMinimumRoyaltySalesPriceIn').change((e) => {
      this.updateEventDetail($(e.target).closest('tr').index(), $(e.target).prop('checked'), 'overrideMinimumRoyaltySalesPriceIn', e.target);
      this.updateMinimumRoyaltySalesPrice($(e.target).closest('tr').index());
    });
  }

  updateMinimumRoyaltySalesPrice(index) {
    const { contractEventDetails } = this.addUpdateContractMgr.state;
    if (!contractEventDetails[index].overrideMinimumRoyaltySalesPriceIn) {
      const minimumRoyalty = contractEventDetails[index].minimumRoyalty || 0;
      const royaltyPercent = contractEventDetails[index].royaltyPercent || 0;
      const minimumRoyaltySalesPrice = roundDecimals(
        minimumRoyalty / (
          royaltyPercent / 100
        ),
        4,
      );
      contractEventDetails[index].minimumRoyaltySalesPrice = minimumRoyaltySalesPrice;
      this.addUpdateContractMgr.setState(
        { ...this.addUpdateContractMgr.state, contractEventDetails },
      );
    }
    this.renderCurrentEventsTable();
  }

  industryLeaseIndToggleChange() {
    $('.industryLeaseIndToggle').change((e) => {
      this.updateEventDetail($(e.target).closest('tr').index(), $(e.target).prop('checked'), 'industryLeaseInd', e.target);
    });
  }

  initShareOfLeasePercentageChange() {
    $('.shareOfLeasePercentage').change((e) => {
      this.updateEventDetail($(e.target).closest('tr').index(), parseFloat($(e.target).val()), 'shareOfLeasePercentage', e.target);
      this.addUpdateContractMgr.setState(
        { ...this.addUpdateContractMgr.state, updateWellInfoInd: true },
      );
    });
  }

  initInterestTypeChange() {
    $('.interestType').change((e) => {
      this.updateEventDetail($(e.target).closest('tr').index(), $(e.target).val(), 'interestType', e.target);
    });
  }

  initTopVerticalExtentOfOwnershipChange() {
    $('.topVerticalExtentOfOwnership').change((e) => {
      this.updateEventDetail($(e.target).closest('tr').index(), $(e.target).val(), 'topVerticalExtentOfOwnership', e.target);
    });
  }

  initBottomVerticalExtentOfOwnershipChange() {
    $('.bottomVerticalExtentOfOwnership').change((e) => {
      this.updateEventDetail($(e.target).closest('tr').index(), $(e.target).val(), 'bottomVerticalExtentOfOwnership', e.target);
    });
  }

  initHorizonChange() {
    $('.horizon').change((e) => {
      this.updateEventDetail($(e.target).closest('tr').index(), $(e.target).val(), 'horizon', e.target);
    });
  }

  initExcludedFromVerticalExtentOfOwnershipChange() {
    $('.excludedFromVerticalExtentOfOwnership').change((e) => {
      this.updateEventDetail($(e.target).closest('tr').index(), $(e.target).val(), 'excludedFromVerticalExtentOfOwnership', e.target);
    });
  }
}
