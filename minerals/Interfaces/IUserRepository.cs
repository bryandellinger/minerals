using Microsoft.AspNetCore.Http;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minerals.Interfaces
{
    public interface IUserRepository
    {
        User GetByDomain(string domain);
        User AddFromCwopa(CwopaAgencyFile cwopaAgencyFile);
        Task UpdateAsync(User user, HttpContext httpContext);
        Task<IEnumerable<User>> GetAllAsync();
        ValueTask<User> FindAsync(long id);
        List<Role> getRoles(long id);
    }
}
