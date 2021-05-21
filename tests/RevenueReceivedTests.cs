using Minerals.Repositories;
using System.Linq;
using Xunit;

namespace Minerals.Tests
{
    public class RevenueReceivedTests
    {
        private RevenueReceivedRepository repository;
        public RevenueReceivedTests() => repository = new RevenueReceivedRepository(ContextHelper.Getcontext());

        [Fact]
        public async void CanGetRevenueReceived()
        {
            //Arrange

            var revenuesReceived = await repository.GetAllAsync();

            //Assert
            Assert.True(revenuesReceived.ToArray().Count() > 0, "There are no Revenue Receiveds");

        }
    }
}
