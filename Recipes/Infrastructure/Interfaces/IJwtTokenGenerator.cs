using Recipes.Domain;
using System.Threading.Tasks;

namespace Recipes.Infrastructure.Interfaces
{
    public interface IJwtTokenGenerator
    {
        Task<string> CreateToken(AppUser user);
    }
}