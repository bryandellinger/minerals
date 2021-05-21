using Minerals.Contexts;
using Minerals.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Repositories
{
    public class TownshipRepository : ITownshipRepository
    {
        private readonly DataContext context;

        public TownshipRepository(DataContext ctx) => context = ctx;
        public List<Township> GetAllTownshipsByContract(long Id)
        {
            List<Township> landLeaseTownships = (from township in context.Townships
                                                join junction in context.TownshipLandLeaseAgreementJunctions on township.Id equals junction.TownshipId
                                                join landLeaseAgreement in context.LandLeaseAgreements.Where(x => x.ContractId == Id) on junction.LandLeaseAgreementId equals landLeaseAgreement.Id
                                                select new Township { Id = township.Id, County = township.County, Class = township.Class, Municipality = township.Municipality }).ToList();
            List<Township> suaTownships = (from township in context.Townships
                                           join junction in context.TownshipSurfaceUseAgreementJunctions on township.Id equals junction.TownshipId
                                           join surfaceuseagreement in context.SurfaceUseAgreements.Where(x => x.ContractId == Id) on junction.SurfaceUseAgreementId equals surfaceuseagreement.Id
                                           select new Township { Id = township.Id, County = township.County, Class = township.Class, Municipality = township.Municipality }).ToList();
            List<Township> prospectingTownships = (from township in context.Townships
                                           join junction in context.TownshipProspectingAgreementJunctions on township.Id equals junction.TownshipId
                                           join prospectingagreement in context.ProspectingAgreements.Where(x => x.ContractId == Id) on junction.ProspectingAgreementId equals prospectingagreement.Id
                                           select new Township { Id = township.Id, County = township.County, Class = township.Class, Municipality = township.Municipality }).ToList();
            List<Township> productionTownships = (from township in context.Townships
                                                   join junction in context.TownshipProductionAgreementJunctions on township.Id equals junction.TownshipId
                                                   join productionagreement in context.ProductionAgreements.Where(x => x.ContractId == Id) on junction.ProductionAgreementId equals productionagreement.Id
                                                   select new Township { Id = township.Id, County = township.County, Class = township.Class, Municipality = township.Municipality }).ToList();
            List<Township> seismicTownships = (from township in context.Townships
                                                  join junction in context.TownshipSeismicAgreementJunctions on township.Id equals junction.TownshipId
                                                  join seismicagreement in context.SeismicAgreements.Where(x => x.ContractId == Id) on junction.SeismicAgreementId equals seismicagreement.Id
                                                  select new Township { Id = township.Id, County = township.County, Class = township.Class, Municipality = township.Municipality }).ToList();
            return landLeaseTownships.Concat(suaTownships).Concat(prospectingTownships).Concat(productionTownships).Concat(seismicTownships).ToList();
        }

    }
}
