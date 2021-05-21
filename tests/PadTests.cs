using Xunit;
using System.Linq;
using Minerals.Repositories;
using Newtonsoft.Json;
using Minerals.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Minerals.Tests
{
   public class PadTests
    {
        private PadRepository repository;
        private DataContext ctx;

        public PadTests()
        {
            ctx = ContextHelper.Getcontext();
            repository = new PadRepository(ctx);
        }

        [Fact]
        public async void CanGetAllPadsByTract()
        {
            //arrange
            var tract = ctx.Tracts.Include(x => x.Pads).First(x => x.Pads != null && x.Pads.Count() > 0);
            var pads1 = tract.Pads;
            var pads2 = await repository.GetAllPadsByTractAsync(tract.Id);

            string pads1AsJSON = JsonConvert.SerializeObject(pads1.ToArray(), Formatting.Indented,
                                new JsonSerializerSettings
                                {
                                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                });

            string pads2AsJSON = JsonConvert.SerializeObject(pads2.ToArray(), Formatting.Indented,
                               new JsonSerializerSettings
                               {
                                   ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                               });

            //Assert
            Assert.Equal(pads1AsJSON, pads2AsJSON);
        }
    }
}
