using Microsoft.AspNetCore.Http;
using Minerals.Infrastructure;
using Minerals.Interfaces;
using Models;


namespace Minerals.Repositories
{
    public class CurrentUserRepository : ICurrentUserRepository
    {
        private readonly ICwopaAgencyFileRepository cwoparepository;
        private readonly IUserRepository userrepository;

        public CurrentUserRepository(ICwopaAgencyFileRepository cwoparepo, IUserRepository userrepo)
        {
            cwoparepository = cwoparepo;
            userrepository = userrepo;
        }

        public User Get(HttpContext httpContext, string identityName = null)
        {
            User currentUser;

            if (identityName != null)
            {
                string domain = identityName.Split('\\')[1];
                string userNameFromQueryString = httpContext.Request.Query["username"].ToString();
                if (userNameFromQueryString != null && !string.IsNullOrEmpty(userNameFromQueryString))
                {
                    domain = userNameFromQueryString.Split('\\')[1];
                    currentUser = GetCurrentUserFromDomain(domain);
                    httpContext.Session.SetJson("CurrentUser", currentUser);
                }
                else
                {
                    currentUser = httpContext.Session.GetJson<User>("CurrentUser");
                    if (currentUser == null)
                    {
                        currentUser = GetCurrentUserFromDomain(domain);
                        httpContext.Session.SetJson("CurrentUser", currentUser);
                    }
                }
            }
            currentUser = httpContext.Session.GetJson<User>("CurrentUser");

            return currentUser;
        }

        private User GetCurrentUserFromDomain(string domain)
        {
            User currentUser = userrepository.GetByDomain(domain);
            if (currentUser == null)
            {
                currentUser = GetCurrentUserFromCwopa(domain);
            }
            return currentUser;
        }

        private User GetCurrentUserFromCwopa(string domain)
        {
            CwopaAgencyFile cwopaAgencyFile = cwoparepository.GetByDomain(domain);
            var user = userrepository.AddFromCwopa(cwopaAgencyFile);
            return user;
        }
    }
}
