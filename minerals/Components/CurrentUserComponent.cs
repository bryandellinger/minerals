using Microsoft.AspNetCore.Mvc;
using Minerals.Interfaces;

namespace Minerals.Components
{
    public class CurrentUserComponent : ViewComponent
    {
        private ICurrentUserRepository repository;

        public CurrentUserComponent(ICurrentUserRepository repo) => repository = repo;

        public IViewComponentResult Invoke() => View(repository.Get(HttpContext, User.Identity.Name));
    }
}
