export default class multiSelectMgr {
  constructor(AddUpdateContractMgr) {
    this.addUpdateContractMgr = AddUpdateContractMgr;
  }

  setDistricts() {
    const { contract, districts, districtContractJunctions } = this.addUpdateContractMgr.state;
    const filteredDistrictContractJunctions = districtContractJunctions.filter(
      (x) => x.contractId === contract.id,
    ).map((c) => c.districtId);
    const newDistricts = districts.map(
      (district) => (
        {
          ...district,
          selected: filteredDistrictContractJunctions.includes(district.id),
        }
      ),
    );
    this.addUpdateContractMgr.setState({
      ...this.addUpdateContractMgr.state, districts: newDistricts,
    });
  }


  initDistrictClose() {
    $('#districtid').on('select2:close', () => {
      this.addUpdateContractMgr.setState({ ...this.addUpdateContractMgr.state, elementToFocus: 'altIdCategories' });
      this.addUpdateContractMgr.render();
    });
  }

  initDistrictChange() {
    $('#districtid').on('change', () => {
      const { contract } = this.addUpdateContractMgr.state;
      const districtContractJunctions = $('#districtid').select2('data').map(
        (x) => (
          {
            contractId: contract.id,
            districtId: parseInt(x.id, 10),
          }
        ),
      );
      this.addUpdateContractMgr.setState(
        { ...this.addUpdateContractMgr.state, districtContractJunctions, elementToFocus: 'contractNumOverrideIn' },
      );
      this.setDistricts();
    });
  }

  initWellPaymentsDueChange() {
    $('#wellPaymentsDue').on('change', () => {
      const storageWellPaymentMonths = $('#wellPaymentsDue').select2('data').map((month) => (
        {
          id: parseInt(month.id, 10),
        }
      ));
      this.addUpdateContractMgr.setState(
        { ...this.addUpdateContractMgr.state, storageWellPaymentMonths },
      );
    });
  }

  initBaseRentalPaymentsDueChange() {
    $('#baseRentalPaymentsDue').on('change', () => {
      const storageBaseRentalPaymentMonths = $('#baseRentalPaymentsDue').select2('data').map((month) => (
        {
          id: parseInt(month.id, 10),
        }
      ));
      this.addUpdateContractMgr.setState(
        { ...this.addUpdateContractMgr.state, storageBaseRentalPaymentMonths },
      );
    });
  }

  initRentalPaymentsDueChange() {
    $('#rentalPaymentsDue').on('change', () => {
      const storageRentalPaymentMonths = $('#rentalPaymentsDue').select2('data').map((month) => (
        {
          id: parseInt(month.id, 10),
        }
      ));
      this.addUpdateContractMgr.setState(
        { ...this.addUpdateContractMgr.state, storageRentalPaymentMonths },
      );
    });
    }

    initContractRentalPaymentsDueChange() {
        $('#contractRentalPaymentsDue').on('change', () => {
            const contractRentalPaymentMonths = $('#contractRentalPaymentsDue').select2('data').map((month) => (
                {
                    id: parseInt(month.id, 10),
                }
            ));
            this.addUpdateContractMgr.setState(
                { ...this.addUpdateContractMgr.state, contractRentalPaymentMonths },
            );
        });
    }


  initTownshipChange() {
    $('#townshipid').on('change', () => {
      const { townships } = this.addUpdateContractMgr.state;
      const selectedTownshipIds = $('#townshipid').select2('data').map((x) => parseInt(x.id, 10));
      const newTownships = townships.map(
        (township) => (
          {
            ...township,
            selected: selectedTownshipIds.includes(township.id),
          }
        ),
      );
      this.addUpdateContractMgr.setState(
        { ...this.addUpdateContractMgr.state, townships: newTownships },
      );
    });
  }

  initAssociatedContractChange() {
    $('#associatedContract').on('change', () => {
      const { associatedContracts } = this.addUpdateContractMgr.state;
      const selectedassociatedContractIds = $('#associatedContract').select2('data').map((x) => x.id);
      const newAssociatedContracts = associatedContracts.map(
        (associatedContract) => (
          {
            ...associatedContract,
            selected: selectedassociatedContractIds.includes(associatedContract.id),
          }
        ),
      );
      this.addUpdateContractMgr.setState(
        { ...this.addUpdateContractMgr.state, associatedContracts: newAssociatedContracts },
      );
    });
  }

  initAssociatedTractChange() {
    $('#associatedTract').on('change', () => {
      const { associatedTracts } = this.addUpdateContractMgr.state;
      const selectedassociatedTractIds = $('#associatedTract').select2('data').map((x) => x.id);
      const newAssociatedTracts = associatedTracts.map(
        (associatedTract) => (
          {
            ...associatedTract,
            selected: selectedassociatedTractIds.includes(associatedTract.id),
          }
        ),
      );
      this.addUpdateContractMgr.setState(
        { ...this.addUpdateContractMgr.state, associatedTracts: newAssociatedTracts },
      );
    });
  }
}
