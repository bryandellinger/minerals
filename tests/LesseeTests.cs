using Minerals.Interfaces;
using Models;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using Minerals.Repositories;
using Minerals.Contexts;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using Newtonsoft.Json;

namespace Minerals.Tests
{
   public  class LesseeTests
    {
        private IGenericRepository<Lessee> repository;

        public LesseeTests() => repository = new GenericRepository<Lessee>(ContextHelper.Getcontext());

       

        [Fact]
        public async void CanGetLessees()
        {
            //Arrange

            var lessees = await repository.GetAllAsync();

            //Assert
            Assert.True(lessees.Count()> 0, "There are no Lessees");

        }

        [Fact]
        public async void CanGetLesseeById()
        {
            //Arrange
            var lessees = await repository.GetAllAsync();
            var lessee1 = lessees.FirstOrDefault();
            var lessee2 = await repository.GetByIdAsync(lessee1.Id);

            //Assert
            Assert.Equal(JsonConvert.SerializeObject(lessee1), JsonConvert.SerializeObject(lessee2));
        }
    }
}
