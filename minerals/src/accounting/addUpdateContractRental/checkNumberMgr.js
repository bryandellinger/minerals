/* eslint-disable max-len */
import AjaxService from '../../services/ajaxService';

export default class checkNumberMgr {
  constructor(AddUpdateContractRentalMgr) {
    this.addUpdateContractRentalMgr = AddUpdateContractRentalMgr;
  }

  init() {
    const { check } = this.addUpdateContractRentalMgr.state;
    $('#contractRentalCheckNum').select2({
      placeholder: 'Select a Check Number',
      minimumInputLength: 2,
      ajax: {
        url: './api/CheckMgrApi/GetSelect2Data',
        contentType: 'application/json; charset=utf-8',
        data(params) {
          const query = {
            id: params.term,
          };
          return query;
        },
        processResults: (results) => ({
          results,
        }),
      },
    });
    if (check && check.id) {
      const data = {
        id: check.id,
        text: check.checkNum,
      };
      if ($('#contractRentalCheckNum').find(`option[value='${data.id}']`).length) {
        $('contractRentalCheckNum').val(data.id).trigger('change');
      } else {
        // Create a DOM Option and pre-select by default
        const newOption = new Option(data.text, data.id, true, true);
        // Append it to the select
        $('#contractRentalCheckNum').append(newOption).trigger('change');
      }
    }
  }

  initCheckNumChange() {
    $('#contractRentalCheckNum').on('select2:select', (e) => {
      AjaxService.ajaxGet(`./api/CheckMgrApi/${e.params.data.id}`)
        .then((check) => {
          if (check && check.id) {
            const { contractRental } = this.addUpdateContractRentalMgr.state;
            this.addUpdateContractRentalMgr.setState({
              ...this.addUpdateContractRentalMgr.state,
              check: {
                ...check,
                checkDate: check.checkDate ? new Date(check.checkDate).toLocaleDateString() : null,
                receivedDate: check.receivedDate ? new Date(check.receivedDate).toLocaleDateString() : null,
              },
              contractRental: {
                ...contractRental,
                checkId: check.id,
              },
            });

            AjaxService.ajaxGet(`./api/LesseeMgrApi/${check.lesseeId}`)
              .then((lessee) => {
                this.addUpdateContractRentalMgr.setState({
                  ...this.addUpdateContractRentalMgr.state, lessee,
                });
                AjaxService.ajaxGet(`./api/ContractMgrApi/GetByLessee/${check.lesseeId}`)
                  .then((contracts) => {
                    if (contracts) {
                      this.addUpdateContractRentalMgr.setState({
                        ...this.addUpdateContractRentalMgr.state,
                        contracts:
                        contracts.map((contract) => (
                          {
                            ...contract,
                            terminationDate: contract.terminationDate ? new Date(contract.terminationDate) : null,
                            effectiveDate: contract.effectiveDate ? new Date(contract.effectiveDate) : null,
                            expirationDate: contract.expirationDate ? new Date(contract.expirationDate) : null,
                          }
                        )),
                      });
                    } else {
                      this.addUpdateContractRentalMgr.setState({
                        ...this.addUpdateContractRentalMgr.state,
                        contracts: [],
                      });
                    }
                    this.addUpdateContractRentalMgr.render();
                  });
              });
          }
        });
    });
  }
}
