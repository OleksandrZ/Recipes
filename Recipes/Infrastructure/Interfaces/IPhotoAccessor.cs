using Microsoft.AspNetCore.Http;
using Recipes.Domain;
using Recipes.Features.Image;
using System.Threading.Tasks;

namespace Recipes.Infrastructure.Interfaces
{
    public interface IPhotoAccessor
    {
        Photo CreatePhoto(IFormFile file);
        public Task<bool> DeletePhotoAsync(Photo photo);
    }
}