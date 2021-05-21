import initCardHeader from '../initCardHeader';
import 'datatables.net/js/jquery.dataTables';
import 'datatables.net-buttons/js/dataTables.buttons';
import 'datatables.net-buttons/js/buttons.html5';
import 'datatables.net-buttons/js/buttons.flash';
import 'datatables.net-dt/css/jquery.dataTables.css';
import '../../datatablebutton.css';
import AjaxService from '../../services/ajaxService';
import initState from './initState';
import initDataTable from './initDataTable';
import initSearchParams from './initSearchParams';

const template = require('../../views/contracts/manageContracts/index.handlebars');
const searchTemplate = require('../../views/datatables/search.handlebars');
const tableTemplate = require('../../views/datatables/genericTable.handlebars');

export default class ManageContractsMgr {
  constructor(options) {
    this.addUpdateContractMgr = options.addUpdateContractMgr;
    this.sideBarMgr = options.sideBarMgr;
    this.container = $('#manageContractsContainer');
    this.searchbarcontainer = 'searchbarcontainer';
    this.tablecontainer = 'tablecontainer';
    this.table = null;
    this.redrawTableOnPageShow();
    }

    redrawTableOnPageShow() {
        var self = this;
        var targetNode = document.getElementById('manageContractsContainer');
        var observer = new MutationObserver(function () {
            if (targetNode.style.display != 'none' && self.table) {
                self.table.draw(false);
            }
        });
        observer.observe(targetNode, { attributes: true, childList: true });
}
    


  setState(state) {
    this.state = state;
  }

    getData() {
        $('.spinner').show();
    $.when(
        $.get('./api/ContractMgrApi', (contracts) => {
        this.setState({
          ...this.state, contracts,
        });
      }),
      $.get('./api/ContractTypeMgrApi', (contractTypes) => {
        this.setState({
          ...this.state, contractTypes,
        });
      }),
      $.get('./api/ContractSubTypeMgrApi', (contractSubTypes) => {
        this.setState({
          ...this.state, contractSubTypes,
        });
      }),
      $.get('./api/DistrictMgrApi', (districts) => {
        this.setState({
          ...this.state, districts,
        });
      }),
      $.get('./api/DistrictContractJunctionMgrApi', (districtContractJunctions) => {
        this.setState({
          ...this.state, districtContractJunctions,
        });
      }),
      $.get('./api/tractMgrApi', (tracts) => {
        this.setState({
          ...this.state, tracts,
        });
      }),
    ).then(() => {
      const {
        contracts, contractTypes, contractSubTypes,
        districts, districtContractJunctions,  tracts,
      } = this.state;
      const data = contracts
        .map(
          (contract) => (
            {
              id: contract.id,
              contractNum: contract.contractNum,
              contractTypeId: contract.contractTypeId,
              contractSubTypeId: contract.contractSubTypeId,
              contractTypeName: contract.contractTypeId && contractTypes
                .filter((x) => x.id === contract.contractTypeId)[0].contractTypeName,
              contractSubTypeName: contract.contractSubTypeId && contractSubTypes
                .filter((x) => x.id === contract.contractSubTypeId)[0].contractSubTypeName,
              name: districtContractJunctions.filter((x) => x.contractId === contract.id)
                .map(
                  (districtContractJunction) => (
                    districts.filter((x) => x.id === districtContractJunction.districtId)
                      .map((y) => `(${y.districtId}) ${y.name}`)
                  ),
                ).join(', '),
              tractNum: tracts.filter((x) => x.id === contract.tractId)
                .map((y) => y.tractNum).join(),
            }
          ),
        );
      this.setState({
        ...this.state, data, filteredData: Object.assign([], data),
      });
        this.renderTable();
        $('.spinner').hide();
    });
  }

  renderTable() {
    const { filteredData } = this.state;
    const tblTemplate = tableTemplate(['ID', 'Contract Num', 'Type', 'SubType', 'Tract', 'District']);
    $(`#${this.tablecontainer}`).empty().append(tblTemplate);
    this.table = initDataTable(filteredData);
    this.initSelectClick();
  }

  

  initSelectClick() {
    const self = this;
    $('#mytable tbody').on('click', 'button', (event) => {
      const data = self.table.row($(event.target).parents('tr')).data();
      self.addUpdateContractMgr.init(data.id);
      self.sideBarMgr.setVisibleContainer('addUpdateContractContainer');
    });
  }

  init() {
    this.table = null;
    this.state = initState();
    this.getData();
    const t = template({ headerInfo: initCardHeader() });
    this.container.empty().append(t);
    this.renderSearchBar();
    this.initClear();
    this.initSearch();
  }

  initSearch() {
    $(`#${this.searchbarcontainer}`).on('submit', '#searchform', () => {
      const { data } = this.state;
      const search = {};
      // eslint-disable-next-line array-callback-return
      $('#searchform').serializeArray().map((x) => { search[x.name] = x.value; });
      this.setState({ ...this.state, search });
      if (Object.values(search).every((x) => (x === null || x === ''))) {
        this.setState({ ...this.state, search: null, filteredData: data });
        this.renderTable();
      } else {
        this.filterData();
        this.renderTable();
      }
    });
  }

  filterData() {
    const { data, search } = this.state;
    this.setState({ ...this.state, filteredData: Object.assign([], data) });
    const entries = Object.entries(search);
    // eslint-disable-next-line no-restricted-syntax
    for (const [key, value] of entries) {
      const { filteredData } = this.state;
      if (value) {
        const newFilteredData = filteredData.filter((x) => (x[key] || '~')
          .toLowerCase().includes(value.toLowerCase()));
        this.setState({ ...this.state, filteredData: newFilteredData });
      }
    }
  }


  initClear() {
    const self = this;
    $(`#${this.searchbarcontainer}`).on('click', '#clearsearchbtn', () => {
      const { data } = self.state;
      const filteredData = Object.assign([], data);
      $('#searchbarcontainer input').val('');
      self.setState({ ...self.state, search: null, filteredData });
      self.renderTable();
    });
  }


  renderSearchBar() {
    const searchParams = initSearchParams();
    const t = searchTemplate(searchParams);
    $(`#${this.searchbarcontainer}`).empty().append(t);
    searchParams.forEach((element) => {
      AjaxService.ajaxGet(element.url).then((d) => {
        const source = Array.from(new Set(d.map((x) => (x[element.idColumn] ? `(${x[element.idColumn]}) ${x[element.column]}` : x[element.column]))));
        const sourceWithNoNulls = source.filter((e) => e === 0 || e);
        $(`#${element.column}`).autocomplete({
          source: sourceWithNoNulls,
          select: (event, ui) => {
            $(`#${element.column}`).val(ui.item.label);
            $('#searchform').submit();
          },
        });
      });
    });
  }
}
