using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Recipes.Infrastructure.Interfaces;

namespace Recipes.Infrastructure.Security
{
    public class CurrentUserAccessor : ICurrentUserAccessor
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CurrentUserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentUsername()
        {
            return httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
