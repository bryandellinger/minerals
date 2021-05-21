using Xunit;
using System.Linq;
using Minerals.Repositories;
using Newtonsoft.Json;
using Minerals.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Minerals.Tests
{
    public class RoyaltyAdjustmentCardViewModelTests
    {
        private RoyaltyAdjustmentCardViewModelRepository repository;
        private DataContext ctx;

        public RoyaltyAdjustmentCardViewModelTests()
        {
            ctx = ContextHelper.Getcontext();
            repository = new RoyaltyAdjustmentCardViewModelRepository(ctx);
        }

        [Fact]
        public async void CanSearchById()
        {
            //arrange 
            var result1 = (await repository.Get(null)).First();
            var idAsString = result1.Id.ToString();
            var result2 = (await repository.Get(idAsString)).First();

            //assert
            Assert.Equal(JsonConvert.SerializeObject(result1), JsonConvert.SerializeObject(result2));
        }
    }
}
