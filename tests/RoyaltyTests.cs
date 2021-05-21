using Xunit;
using System.Linq;
using Minerals.Repositories;
using Newtonsoft.Json;
using Minerals.Contexts;
using Models;
using Microsoft.EntityFrameworkCore;

namespace Minerals.Tests
{
    public class RoyaltyTests
    {
        private RoyaltyRepository repository;
        private DataContext ctx;

        public RoyaltyTests()
        {
            ctx = ContextHelper.Getcontext();
            repository = new RoyaltyRepository(ctx);
        }

   
        [Fact]
        public async void CanInsert()
        {
            //arrange
            Royalty royalty = new Royalty
            {
                Id = 0,
                PostMonth = 1,
                PostYear = 2019,
                GasRoyalty = 50,
                GasProd = 50
            };

            using (var transaction = ctx.Database.BeginTransaction())
            {
                ctx.Royalties.Add(royalty);
                await ctx.SaveChangesAsync();

                Assert.True(royalty.Id > 0, "royalty was not inserted");
                transaction.Rollback();
            }

        }
    }
}
