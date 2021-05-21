using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Minerals.Contexts;
using Minerals.Repositories;
using System;
using System.IO;
using System.Linq;

namespace Minerals.Infrastructure
{
    [System.AttributeUsage(System.AttributeTargets.All,
                   AllowMultiple = false,
                   Inherited = true)]
    public class AuthorizeUserAttribute : Attribute, IAuthorizationFilter
    {
        public string Roles { get; set; }
        /// <summary> Authorization filter accepts a comma delimited list of roles and checks if user has necessary roles</summary>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var roles = this.Roles;
            IConfigurationBuilder builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

            var configuration = builder.Build();

            string connectionString = configuration["connectionstrings:DefaultConnection"];

            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlServer(connectionString);
            DataContext ctx = new DataContext(optionsBuilder.Options);

            CwopaAgencyFileRepository cwopaFileRepository = new CwopaAgencyFileRepository(ctx);

            UserRepository userRepository = new UserRepository(ctx);

            CurrentUserRepository currentUserRepository = new CurrentUserRepository(cwopaFileRepository, userRepository);

            var user = currentUserRepository.Get(context.HttpContext);

            string[] userRoles = userRepository.getRoles(user.Id).Select(x => x.RoleName).ToArray();

            string[] neededRoles = this.Roles.Split(",");

            bool userHasAllNeedRoles = neededRoles.Intersect(userRoles).Count() == neededRoles.Length;

            if (!userHasAllNeedRoles)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            }
        }
    }
}
