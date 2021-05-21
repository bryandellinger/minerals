using Minerals.Contexts;
using Minerals.Interfaces;
using Models;
using System.Linq;

namespace Minerals.Repositories
{
    public class CwopaAgencyFileRepository : ICwopaAgencyFileRepository
    {
        private readonly DataContext context;

        public CwopaAgencyFileRepository(DataContext ctx) => context = ctx;

        public CwopaAgencyFile GetByDomain(string domain) => context.CwopaAgencyFiles.FirstOrDefault(x => x.DomainName == domain);

    }
}
