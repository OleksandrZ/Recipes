using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Recipes.Infrastructure.Security
{
    public class IsAuthorRequirement : IAuthorizationRequirement
    {
    }

    public class IsAuthorRequirementHandler : AuthorizationHandler<IsAuthorRequirement>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly RecipesDbContext dbContext;

        public IsAuthorRequirementHandler(IHttpContextAccessor httpContextAccessor, RecipesDbContext context)
        {
            this.dbContext = context;
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsAuthorRequirement requirement)
        {
            var currentUserName = httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var recipeId = httpContextAccessor.HttpContext.Request.Query["id"].ToString();

            var recipes = dbContext.Recipes.Include(x => x.Author);

            var recipe = recipes.Where(x => x.Id == recipeId).FirstOrDefault();

            if (recipe.Author.UserName == currentUserName)
                context.Succeed(requirement);
            else
                context.Fail();

            return Task.CompletedTask;
        }
    }
}