import updateCheckSubmissionPeriods from './updateCheckSubmissionPeriods';
import * as constants from '../../constants';

export default class GetDataMgr {
  constructor(AddUpdateContractMgr) {
    this.addUpdateContractMgr = AddUpdateContractMgr;
  }

  getData() {
    $('.spinner').show();
    let selectedTownshipIds = [];
    let selectedAssociatedContracts = [];
    let associatedContracts = [];
    let selectedAssociatedTracts = [];
    let associatedTracts = [];
    $.when(
      $.get('./api/AltIdCategoryMgrApi/GetAssociatedContractsDropDownList', (d) => {
        associatedContracts = d;
      }),

      $.get(`./api/ContractMgrApi/${this.addUpdateContractMgr.id}`, (contract) => {
        if (this.addUpdateContractMgr.id) {
          this.addUpdateContractMgr.setState({
            ...this.addUpdateContractMgr.state, contract, originalContract: { ...contract },
          });
          if (contract.contractNumOverride) {
            this.addUpdateContractMgr.setState({
              ...this.addUpdateContractMgr.state, contractNumOverrideIn: true,
            });
          }
        }
      }),
      $.get('./api/ContractTypeMgrApi', (d) => {
        function compare(a, b) {
          // Use toUpperCase() to ignore character casing
          const nameA = a.contractTypeName.toUpperCase();
          const nameB = b.contractTypeName.toUpperCase();

          let comparison = 0;
          if (nameA > nameB) {
            comparison = 1;
          } else if (nameA < nameB) {
            comparison = -1;
          }
          return comparison;
        }
        const contractTypes = d.sort(compare);

        this.addUpdateContractMgr.setState({
          ...this.addUpdateContractMgr.state, contractTypes,
        });
      }),

      $.get(`./api/LandLeaseAgreementMgrApi/landleaseagreementbycontract/${this.addUpdateContractMgr.id}`, (d) => {
        if (d) {
          const agreement = {
            ...d,
            effectiveDate: d.effectiveDate ? new Date(d.effectiveDate) : null,
            terminationDate: d.terminationDate ? new Date(d.terminationDate) : null,
            expirationDate: d.expirationDate ? new Date(d.expirationDate) : null,
            reversionDate: null,
            dataReceivedDate: null,
            lesseeEffectiveDate: null,
          };
          this.addUpdateContractMgr.setState({
            ...this.addUpdateContractMgr.state, agreement, initialAcreage: d.acreage,
          });
        }
      }),

      $.get(`./api/SurfaceUseAgreementMgrApi/surfaceuseagreementbycontract/${this.addUpdateContractMgr.id}`, (d) => {
        if (d) {
          const agreement = {
            ...d,
            effectiveDate: d.effectiveDate ? new Date(d.effectiveDate) : null,
            terminationDate: d.terminationDate ? new Date(d.terminationDate) : null,
            expirationDate: null,
            reversionDate: d.reversionDate ? new Date(d.reversionDate) : null,
            dataReceivedDate: null,
            lesseeEffectiveDate: null,
          };
          this.addUpdateContractMgr.setState({
            ...this.addUpdateContractMgr.state, agreement, initialAcreage: d.acreage,
          });
        }
      }),

      $.get(`./api/ProspectingAgreementMgrApi/prospectingagreementbycontract/${this.addUpdateContractMgr.id}`, (d) => {
        if (d) {
          const agreement = {
            ...d,
            effectiveDate: d.effectiveDate ? new Date(d.effectiveDate) : null,
            terminationDate: d.terminationDate ? new Date(d.terminationDate) : null,
            expirationDate: null,
            reversionDate: null,
            dataReceivedDate: null,
            lesseeEffectiveDate: null,
          };
          this.addUpdateContractMgr.setState({
            ...this.addUpdateContractMgr.state, agreement, initialAcreage: d.acreage,
          });
        }
      }),

      $.get(`./api/ProductionAgreementMgrApi/productionagreementbycontract/${this.addUpdateContractMgr.id}`, (d) => {
        if (d) {
          const agreement = {
            ...d,
            effectiveDate: d.effectiveDate ? new Date(d.effectiveDate) : null,
            terminationDate: d.terminationDate ? new Date(d.terminationDate) : null,
            expirationDate: null,
            reversionDate: null,
            dataReceivedDate: null,
            lesseeEffectiveDate: null,
          };
          this.addUpdateContractMgr.setState({
            ...this.addUpdateContractMgr.state, agreement, initialAcreage: d.acreage,
          });
        }
      }),

      $.get(`./api/StorageMgrApi/getByContract/${this.addUpdateContractMgr.id}`, (storage) => {
        if (storage && storage.id) {
          this.addUpdateContractMgr.setState({
            ...this.addUpdateContractMgr.state, storage,
          });
        }
      }),

        $.get(`./api/AdditionalBonusMgrApi/getByContract/${this.addUpdateContractMgr.id}`, (additionalBonuses) => {
            if (additionalBonuses && additionalBonuses.length) {
                this.addUpdateContractMgr.setState({
                    ...this.addUpdateContractMgr.state, additionalBonuses,
                });
            }
        }),

      $.get(`./api/SeismicAgreementMgrApi/seismicagreementbycontract/${this.addUpdateContractMgr.id}`, (d) => {
        if (d) {
          const agreement = {
            ...d,
            effectiveDate: d.effectiveDate ? new Date(d.effectiveDate) : null,
            terminationDate: d.terminationDate ? new Date(d.terminationDate) : null,
            expirationDate: null,
            reversionDate: null,
            dataReceivedDate: d.dataReceivedDate ? new Date(d.dataReceivedDate) : null,
            lesseeEffectiveDate: null,
          };
          this.addUpdateContractMgr.setState({
            ...this.addUpdateContractMgr.state, agreement, initialAcreage: d.acreage,
          });
        }
      }),

      $.get('./api/ContractMgrApi', (contracts) => {
        this.addUpdateContractMgr.setState({
          ...this.addUpdateContractMgr.state, contracts,
        });
      }),

        $.get('./api/AdditionalContractInformationMgrApi', (additionalContractInformations) => {
            this.addUpdateContractMgr.setState({
                ...this.addUpdateContractMgr.state, additionalContractInformations,
            });
     }),

      $.get('./api/MonthMgrApi', (months) => {
        this.addUpdateContractMgr.setState({
          ...this.addUpdateContractMgr.state, months,
        });
      }),
      $.get(`./api/StorageWellPaymentMonthJunctionMgrApi/StorageWellPaymentMonthsByContract/${this.addUpdateContractMgr.id}`, (storageWellPaymentMonths) => {
        this.addUpdateContractMgr.setState({
          ...this.addUpdateContractMgr.state, storageWellPaymentMonths,
        });
      }),
        $.get(`./api/SuretyMgrApi/GetSuretiesByContract/${this.addUpdateContractMgr.id}`, (sureties) => {
            if (sureties && sureties.length) {
                this.addUpdateContractMgr.setState({
                    ...this.addUpdateContractMgr.state, sureties,
                });
            }
        }),
      $.get(`./api/StorageBaseRentalPaymentMonthJunctionMgrApi/StorageBaseRentalPaymentMonthsByContract/${this.addUpdateContractMgr.id}`, (storageBaseRentalPaymentMonths) => {
        this.addUpdateContractMgr.setState({
          ...this.addUpdateContractMgr.state, storageBaseRentalPaymentMonths,
        });
      }),
      $.get(`./api/StorageRentalPaymentMonthJunctionMgrApi/StorageRentalPaymentMonthsByContract/${this.addUpdateContractMgr.id}`, (storageRentalPaymentMonths) => {
        this.addUpdateContractMgr.setState({
          ...this.addUpdateContractMgr.state, storageRentalPaymentMonths,
        });
      }),
        $.get(`./api/ContractRentalPaymentMonthJunctionMgrApi/ContractRentalPaymentMonthsByContract/${this.addUpdateContractMgr.id}`, (contractRentalPaymentMonths) => {
            this.addUpdateContractMgr.setState({
                ...this.addUpdateContractMgr.state, contractRentalPaymentMonths,
            });
        }),
      $.get('./api/ContractSubTypeMgrApi', (d) => {
        function compare(a, b) {
          // Use toUpperCase() to ignore character casing
          const nameA = a.contractSubTypeName.toUpperCase();
          const nameB = b.contractSubTypeName.toUpperCase();

          let comparison = 0;
          if (nameA > nameB) {
            comparison = 1;
          } else if (nameA < nameB) {
            comparison = -1;
          }
          return comparison;
        }
        const contractSubTypes = d.sort(compare);
        this.addUpdateContractMgr.setState({
          ...this.addUpdateContractMgr.state, contractSubTypes,
        });
      }),
      $.get('./api/tractMgrApi', (tracts) => {
        this.addUpdateContractMgr.setState({
          ...this.addUpdateContractMgr.state,
          tracts: tracts.filter((x) => !x.terminated),
          terminatedTracts: tracts.filter((x) => x.terminated),
        });
        const tractNums = tracts.map((x) => x.tractNum);
        const tractNumsNoNulls = tractNums.filter((tractNum) => tractNum != null);
        const distinctTractNums = [...new Set(tractNumsNoNulls)];
        const orderedTractNums = distinctTractNums.sort();
        associatedTracts = orderedTractNums;
      }),

      $.get(`./api/associatedTractMgrApi/associatedtractsbycontract/${this.addUpdateContractMgr.id}`, (d) => {
        selectedAssociatedTracts = d.map((x) => x.tractNum);
      }),

      $.get(`./api/wellTractInformationMgrApi/welltractinfobycontract/${this.addUpdateContractMgr.id}`, (wellTractInfos) => {
        this.addUpdateContractMgr.setState({
          ...this.addUpdateContractMgr.state, wellTractInfos,
        });
      }),

      $.get(`./api/WellMgrApi/wellsbycontract/${this.addUpdateContractMgr.id}`, (wells) => {
        this.addUpdateContractMgr.setState({
          ...this.addUpdateContractMgr.state, wells,
        });
      }),

      $.get('./api/ContractEventDetailReasonMgrApi', (contractEventDetailReasonsForChange) => {
        this.addUpdateContractMgr.setState({
          ...this.addUpdateContractMgr.state, contractEventDetailReasonsForChange,
        });
      }),

      $.get('./api/lesseeMgrApi', (d) => {
        function compare(a, b) {
          // Use toUpperCase() to ignore character casing
          const nameA = a.lesseeName;
          const nameB = b.lesseeName;

          let comparison = 0;
          if (nameA > nameB) {
            comparison = 1;
          } else if (nameA < nameB) {
            comparison = -1;
          }
          return comparison;
        }
        this.addUpdateContractMgr.setState({
          ...this.addUpdateContractMgr.state, lessees: d.sort(compare),
        });
      }),

      $.get(`./api/townshipMgrApi/townshipsbycontract/${this.addUpdateContractMgr.id}`, (d) => {
        selectedTownshipIds = d.map((x) => x.id);
      }),

      $.get(`./api/associatedContractMgrApi/associatedcontractsbycontract/${this.addUpdateContractMgr.id}`, (d) => {
        selectedAssociatedContracts = d.map((x) => x.associatedContractName);
      }),

      $.get(`./api/ContractEventDetailMgrApi/eventdetailsbycontract/${this.addUpdateContractMgr.id}`, (d) => {
        this.addUpdateContractMgr.setState(
          {
            ...this.addUpdateContractMgr.state,
            contractEventDetails: d.filter((x) => x.activeInd).map(
              (event) => (
                {
                  ...event,
                  overrideMinimumRoyaltySalesPriceIn: false,
                }
              ),
            ),
            contractEventDetailsHistory: d.filter((x) => !x.activeInd).map(
              (event) => (
                {
                  ...event,
                  effectiveDate: event.effectiveDate ? new Date(event.effectiveDate) : null,
                  endDate: event.endDate ? new Date(event.endDate) : null,
                }
              ),
            ),
          },
        );
      }),

      $.get(`./api/paymentRequirementMgrApi/paymentRequirementbycontract/${this.addUpdateContractMgr.id}`, (d) => {
        if (d) {
          const paymentRequirement = {
            ...d,
            rentalShutInDueDate: d.rentalShutInDueDate ? new Date(d.rentalShutInDueDate) : null,
          };

          this.addUpdateContractMgr.setState({
            ...this.addUpdateContractMgr.state,
            paymentRequirement,
            checkSubmissionPeriods: updateCheckSubmissionPeriods(
              paymentRequirement.checkSubmissionPeriod,
              this.addUpdateContractMgr.state.checkSubmissionPeriods,
            ),
          });
        }
      }),

      $.get(`./api/rowContractMgrApi/rowcontractsbycontract/${this.addUpdateContractMgr.id}`, (d) => {
        this.addUpdateContractMgr.setState({
          ...this.addUpdateContractMgr.state, rowContracts: d.map((x) => x.rowContractName),
        });
      }),

      $.get(`./api/pluggingSuretyDetailMgrApi/pluggingsuretydetailsbycontract/${this.addUpdateContractMgr.id}`, (pluggingSuretyDetails) => {
        if (pluggingSuretyDetails && pluggingSuretyDetails.length) {
          this.addUpdateContractMgr.setState({
            ...this.addUpdateContractMgr.state, pluggingSuretyDetails,
          });
          if (pluggingSuretyDetails[0].measurementType) {
            this.addUpdateContractMgr.setState({
              ...this.addUpdateContractMgr.state,
              pluggingSuretyDetailsMeasurementType: pluggingSuretyDetails[0].measurementType,
            });
          }
        }
      }),

      $.get(`./api/pluggingSuretyDetailMgrApi/pluggingsuretydetailsbycontract/${this.addUpdateContractMgr.id}`, (pluggingSuretyDetails) => {
        if (pluggingSuretyDetails && pluggingSuretyDetails.length) {
          this.addUpdateContractMgr.setState({
            ...this.addUpdateContractMgr.state,
            pluggingSuretyDetails,
            pluggingSuretyDetailsMeasurementType:
            pluggingSuretyDetails[0].measurementType || constants.MeasurementType_TVD,
          });
        }
      }),

      $.get('./api/townshipMgrApi', (d) => {
        function compare(a, b) {
          // Use toUpperCase() to ignore character casing
          const nameA = a.municipality.toUpperCase();
          const nameB = b.municipality.toUpperCase();

          let comparison = 0;
          if (nameA > nameB) {
            comparison = 1;
          } else if (nameA < nameB) {
            comparison = -1;
          }
          return comparison;
        }
        const sortedtownships = d.sort(compare);
        const townships = sortedtownships.map(
          (township) => (
            {
              ...township,
              name: `${township.municipality} (${township.county})`,
            }
          ),
        );
        this.addUpdateContractMgr.setState({
          ...this.addUpdateContractMgr.state, townships,
        });
      }),

      $.get('./api/DistrictMgrApi', (d) => {
        function compare(a, b) {
          // Use toUpperCase() to ignore character casing
          const nameA = a.districtId;
          const nameB = b.districtId;

          let comparison = 0;
          if (nameA > nameB) {
            comparison = 1;
          } else if (nameA < nameB) {
            comparison = -1;
          }
          return comparison;
        }
        const sorteddistricts = d.sort(compare);
        const districts = sorteddistricts.map(
          (district) => (
            {
              ...district,
              name: `(${district.districtId.toString().padStart(2, '0')}) ${district.name}`,
            }
          ),
        );
        this.addUpdateContractMgr.setState({
          ...this.addUpdateContractMgr.state, districts,
        });
      }),
      $.get('./api/DistrictContractJunctionMgrApi', (districtContractJunctions) => {
        this.addUpdateContractMgr.setState({
          ...this.addUpdateContractMgr.state, districtContractJunctions,
        });
      }),
      $.get('./api/TerminationReasonMgrApi', (terminationReasons) => {
        this.addUpdateContractMgr.setState({
          ...this.addUpdateContractMgr.state, terminationReasons,
        });
      }),
      $.get('./api/AltIdCategoryMgrApi', (altIdCategories) => {
        this.addUpdateContractMgr.setState({
          ...this.addUpdateContractMgr.state, altIdCategories,
        });
      }),
    ).then(() => {
      const {
        tracts, contract, townships,
      } = this.addUpdateContractMgr.state;
      if (this.addUpdateContractMgr.id) {
        this.addUpdateContractMgr.setDistricts();
        const newTownships = townships.map((township) => (
          {
            ...township,
            selected: selectedTownshipIds.includes(township.id),
          }
        ));
        const tract = tracts.find((x) => x.id === contract.tractId);
        this.addUpdateContractMgr.setState({
          ...this.addUpdateContractMgr.state,
          tractNum: tract && tract.administrative ? `${tract.tractNum}(admin)` : tract ? tract.tractNum : '',
          townships: newTownships,
        });
      }


      const newAssociatedContracts = associatedContracts.map((associatedContract) => (
        {
          id: associatedContract,
          name: associatedContract,
          selected: selectedAssociatedContracts.includes(associatedContract),
        }
      ));

      const newAssociatedTracts = associatedTracts.map((associatedTract) => (
        {
          id: associatedTract,
          name: associatedTract,
          selected: selectedAssociatedTracts.includes(associatedTract),
        }
      ));

      this.addUpdateContractMgr.setState({
        ...this.addUpdateContractMgr.state,
        associatedContracts: newAssociatedContracts,
        associatedTracts: newAssociatedTracts,
      });
      this.addUpdateContractMgr.render();
      $('.spinner').hide();
    });
  }
}
