using Xunit;
using System.Linq;
using Minerals.Repositories;
using Newtonsoft.Json;
using Minerals.Contexts;

namespace Minerals.Tests
{
    public class CwopaTests
    {
        private CwopaAgencyFileRepository repository;
        private DataContext ctx;

        public CwopaTests()
        {
            ctx = ContextHelper.Getcontext();
            repository = new CwopaAgencyFileRepository(ctx);
        }

        [Fact]
        public  void CanGetByDomain()
        {
            //Arrange
            var cwopa1 = ctx.CwopaAgencyFiles.First(x => x.DomainName != null);
            var cwopa2 =  repository.GetByDomain(cwopa1.DomainName);

            //Assert
            Assert.Equal(JsonConvert.SerializeObject(cwopa1), JsonConvert.SerializeObject(cwopa2));
        }
    }
}
