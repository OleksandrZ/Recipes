using Recipes.Domain;

namespace Recipes.Infrastructure.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string CreateToken(AppUser user);
    }
}