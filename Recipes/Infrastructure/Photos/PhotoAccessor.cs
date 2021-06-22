using ImageMagick;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Recipes.Domain;
using Recipes.Infrastructure.Interfaces;
using System;
using System.IO;

namespace Recipes.Infrastructure.Photos
{
    public class PhotoAccessor : IPhotoAccessor
    {
        private readonly string[] _permittedExtensions = { ".jpg", ".gif", ".png" };
        private readonly RecipesDbContext context;

        public PhotoAccessor(RecipesDbContext context)
        {
            this.context = context;
        }

        public Photo CreatePhoto(IFormFile image)
        {
            Photo photo = new Photo() { Id = Guid.NewGuid().ToString() };
            using (var stream = image.OpenReadStream())
            {
                if (FileHelper.IsValidFileExtensionAndSignature(image.FileName, stream, _permittedExtensions))
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images");
                    if (!Directory.Exists(path))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(path);
                    }
                    string fileName = photo.Id + Path.GetExtension(image.FileName);
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

                    var file = new FileInfo(fullPath);

                    var optimizer = new ImageOptimizer();
                    optimizer.Compress(file);

                    file.Refresh();

                    photo.Url = "https://" + httpContextAccessor.HttpContext.Request.Host.Value + @"\images\" + fileName;
                    photo.FileName = fileName;
                    photo.Path = fullPath;
                    photo.Size = image.Length;
                }
            }
            return photo;
        }

        public async System.Threading.Tasks.Task<bool> DeletePhotoAsync(Photo photo)
        {
            File.Delete(photo.Path);
            await context.Users.LoadAsync();
            context.Images.Remove(photo);

            try
            {
                if (await context.SaveChangesAsync() > 0)
                {
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            throw new Exception("Problem deleting image");

        }
    }
}