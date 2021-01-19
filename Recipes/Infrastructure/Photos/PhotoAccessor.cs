using Microsoft.AspNetCore.Http;
using Recipes.Features.Image;
using Recipes.Infrastructure.Errors;
using Recipes.Infrastructure.Interfaces;
using System;
using System.IO;
using System.Net;

namespace Recipes.Infrastructure.Photos
{
    public class PhotoAccessor : IPhotoAccessor
    {
        private readonly string[] _permittedExtensions = { ".jpg", ".gif", ".png" };

        public PhotoUploadResult AddPhoto(IFormFile image)
        {
            PhotoUploadResult result = new PhotoUploadResult() { Id = Guid.NewGuid().ToString() };
            using (var stream = image.OpenReadStream())
            {
                if (FileHelper.IsValidFileExtensionAndSignature(image.FileName, stream, _permittedExtensions))
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images");
                    if (!Directory.Exists(path))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(path);
                    }
                    string fileName = result.Id + Path.GetExtension(image.FileName);
                    string fullPath = Path.Combine(path, fileName);
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        using (var binaryWriter = new BinaryWriter(fileStream))
                        {
                            using (var binaryReader = new BinaryReader(image.OpenReadStream()))
                            {
                                binaryWriter.Write(binaryReader.ReadBytes((int)image.Length));
                            }
                        }
                    }
                    HttpContextAccessor httpContextAccessor = new HttpContextAccessor();
                    result.Url = httpContextAccessor.HttpContext.Request.Host.Value + @"\images\" + fileName;
                    result.FileName = fileName;
                    result.Path = fullPath;
                    result.Size = image.Length;
                }
            }
            return result;
        }

        public async System.Threading.Tasks.Task<bool> DeletePhotoAsync(string id, RecipesDbContext context)
        {
            var img = await context.Images.FindAsync(id);

            File.Delete(img.Path);
            context.Images.Remove(img);

            if (await context.SaveChangesAsync() > 0)
            {
                return true;
            }
            throw new Exception("Error deleting image");
        }
    }
}