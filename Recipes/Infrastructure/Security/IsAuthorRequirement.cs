using System;
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
            if (httpContextAccessor.HttpContext != null)
            {
                var currentUserName = httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                byte[] buffer;
                if (httpContextAccessor.HttpContext.Request.ContentLength != null)
                {
                    long size = httpContextAccessor.HttpContext.Request.ContentLength.Value;
                    buffer = new byte[size];
                }
                else
                {
                    return Task.FromException(new Exception());
                }

                // Copy body to memory stream
                var memoryStream = new MemoryStream();
                httpContextAccessor.HttpContext.Request.Body.CopyToAsync(memoryStream);

                // reset position after CopyToAsync
                memoryStream.Seek(0, SeekOrigin.Begin);
                memoryStream.ReadAsync(buffer);

                var str = Encoding.UTF8.GetString(buffer);

                // reset position after ReadAsync
                memoryStream.Seek(0, SeekOrigin.Begin);
                memoryStream.Dispose();
                httpContextAccessor.HttpContext.Request.Body = memoryStream;

                if (!string.IsNullOrEmpty(str))
                {
                    var obj = JObject.Parse(str);

                    if (httpContextAccessor.HttpContext.Request.Path == "/api/recipe/edit")
                    {
                        var recipeId = obj["id"]?.ToString();

                        var recipes = dbContext.Recipes.Include(x => x.Author);

                        var recipe = recipes.FirstOrDefault(x => x.Id == recipeId);

                        if (recipe?.Author.UserName == currentUserName)
                            context.Succeed(requirement);
                        else
                            context.Fail();
                    }
                    else
                    {
                        var commentId = obj["id"]?.ToString();

                        var comments = dbContext.Comments.Include(x => x.Author);

                        var comment = comments.FirstOrDefault(x => x.Id == commentId);

                        if(comment?.Author.UserName == currentUserName)
                            context.Succeed(requirement);
                        else
                            context.Fail();
                    }
                }
                else
                    context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}