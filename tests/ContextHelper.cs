using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Minerals.Contexts;

namespace Minerals.Tests
{
    public static class ContextHelper
    {
        internal static DataContext Getcontext()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json");

            var configuration = builder.Build();

            string connectionString = configuration["DefaultConnection"];

            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlServer(connectionString);
            DataContext ctx = new DataContext(optionsBuilder.Options);
            return ctx;
        }
    }
}
