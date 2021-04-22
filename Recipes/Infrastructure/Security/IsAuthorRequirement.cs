using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Recipes.Domain;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
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

            long size = httpContextAccessor.HttpContext.Request.ContentLength.Value;
            byte[] buffer = new byte[size];
            var body = httpContextAccessor.HttpContext.Request.BodyReader.AsStream();
            using var streamReader = new StreamReader(body);

            var res = streamReader.ReadToEndAsync();
            string str = "";

            if(res.IsCompleted)
            {
                str = res.Result;

                var obj = JObject.Parse(str);

                var recipeId = obj["id"].ToString();

                var recipes = dbContext.Recipes.Include(x => x.Author);

                var recipe = recipes.Where(x => x.Id == recipeId).FirstOrDefault();

                if (recipe.Author.UserName == currentUserName)
                    context.Succeed(requirement);
                else
                    context.Fail();
            }
            else
                context.Fail();

            return Task.CompletedTask;
        }
    }
}