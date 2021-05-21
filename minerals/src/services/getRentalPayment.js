import AjaxService from './ajaxService';

const getRentalPayment = (opts) => {
    return new Promise((resolve) => {
        AjaxService.ajaxPost('./api/ContractMgrApi/GetStorageRentalInformation',
            {
                id: opts.contractId,
                leasedTractAcreage: opts.acreage ? parseFloat(opts.acreage) : 0,
                effectiveDate: opts.effectiveDate,
                secondThroughFourthYearDelayRental: opts.secondThroughFourthYearDelayRental,
                fifthYearOnwardDelayRental: opts.fifthYearOnwardDelayRental

            })
            .then((d) => {
                resolve(d);
            });
    });
}

export  default getRentalPayment;