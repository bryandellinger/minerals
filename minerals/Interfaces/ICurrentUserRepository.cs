using Microsoft.AspNetCore.Http;
using Models;

namespace Minerals.Interfaces
{
    public interface ICurrentUserRepository
    {
        User Get(HttpContext httpContext, string identityName = null);
    }
}
