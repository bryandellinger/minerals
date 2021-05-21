using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Minerals.Contexts;
using Minerals.Infrastructure;
using Minerals.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext context;

        public UserRepository(DataContext ctx) => context = ctx;

        public User AddFromCwopa(CwopaAgencyFile cwopaAgencyFile)
        {
            User user = new User
            {
                ActiveEmployee = true,
                Company = cwopaAgencyFile?.Company,
                CreateDate = DateTime.Now,
                DomainName = cwopaAgencyFile?.DomainName,
                EmailAddress = cwopaAgencyFile?.EmailAddress,
                EmployeeNum = cwopaAgencyFile?.EmployeeNum,
                JobTitle = null,
                LastUpdateDate = DateTime.Now,
                NameFirst = cwopaAgencyFile?.NameFirst,
                NameLast = cwopaAgencyFile?.NameLast,
                WorkAddr = cwopaAgencyFile.WorkAddr,
                WorkPhone = cwopaAgencyFile.WorkPhone,
                Id = 0,
                UserRoles = new List<UserRoleJunction>()
                {
                    new UserRoleJunction()
                    {
                        Id = 0,
                        RoleId = this.context.Roles.First(x => x.RoleName == "read").Id
                    }
                }
            };
            this.context.Users.Add(user);
            this.context.SaveChanges();
            return user;
        }

        public List<Role> getRoles(long id) => context.UserRoleJunctions.Where(x => x.UserId == id).Include(x => x.Role).Select(x => x.Role).ToList();


        public ValueTask<User> FindAsync(long id) => context.Users.FindAsync(id);


        public async Task<IEnumerable<User>> GetAllAsync() => await context.Users.ToListAsync().ConfigureAwait(false);

        public User GetByDomain(string domain) => context.Users.FirstOrDefault(x => x.DomainName == domain);

        public async Task UpdateAsync(User user, HttpContext httpContext)
        {
            context.Entry(user).State = EntityState.Modified;
            await context.SaveChangesAsync().ConfigureAwait(false);
            httpContext.Session.SetJson("CurrentUser", user);
        }

    }
}
