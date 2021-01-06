using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Recipes.Domain;

namespace Recipes.Infrastructure
{
    public class RecipesDbContext : IdentityDbContext<User>
    {
        public RecipesDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}