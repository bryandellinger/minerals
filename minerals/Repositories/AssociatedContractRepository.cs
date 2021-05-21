using Microsoft.EntityFrameworkCore;
using Minerals.Contexts;
using Minerals.Interfaces;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Repositories
{
    public class AssociatedContractRepository : IAssociatedContractRepository
    {
        private readonly DataContext context;
        public AssociatedContractRepository(DataContext ctx) => context = ctx;
        public async Task<IEnumerable<AssociatedContract>> GetAllAssociatedContractsByContractAsync(long id) => await context.AssociatedContracts.Where(x => x.ContractId == id).ToListAsync().ConfigureAwait(false);

        public List<string> GetAssociatedContractsDropDownList()
        {
            List<string> contractNums = context.Contracts.Where(x => x.ContractNum != null).Select(e => e.ContractNum).ToList();
            List<string> landLeaseAltIdInformation = context.LandLeaseAgreements.Where(x => x.AltIdInformation != null).Select(e => e.AltIdInformation).ToList();
            List<string> surfaceUseAgreementIdInformation = context.SurfaceUseAgreements.Where(x => x.AltIdInformation != null).Select(e => e.AltIdInformation).ToList();
            List<string> prospectingAgreementIdInformation = context.ProspectingAgreements.Where(x => x.AltIdInformation != null).Select(e => e.AltIdInformation).ToList();
            List<string> productionAgreementIdInformation = context.ProductionAgreements.Where(x => x.AltIdInformation != null).Select(e => e.AltIdInformation).ToList();
            List<string> seismicAgreementIdInformation = context.SeismicAgreements.Where(x => x.AltIdInformation != null).Select(e => e.AltIdInformation).ToList();
            return contractNums
                .Concat(landLeaseAltIdInformation)
                .Concat(surfaceUseAgreementIdInformation)
                .Concat(prospectingAgreementIdInformation)
                .Concat(productionAgreementIdInformation)
                .Concat(seismicAgreementIdInformation)
                .Distinct()
                .OrderBy(q => q)
                .ToList();
        }
 
    }
}
