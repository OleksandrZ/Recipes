using Microsoft.AspNetCore.Http;
using Recipes.Features.Image;
using System.Threading.Tasks;

namespace Recipes.Infrastructure.Interfaces
{
    public interface IPhotoAccessor
    {
        PhotoUploadResult AddPhoto(IFormFile file);
        public Task<bool> DeletePhotoAsync(string id, RecipesDbContext context);
    }
}