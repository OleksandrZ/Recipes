using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Recipes.Domain;
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
                    photo.Url = httpContextAccessor.HttpContext.Request.Host.Value + @"\images\" + fileName;
                    photo.FileName = fileName;
                    photo.Path = fullPath;
                    photo.Size = image.Length;
                }
            }
            return photo;
        }

        public async System.Threading.Tasks.Task<bool> DeletePhotoAsync(Photo photo)
        {
            using (var context = new RecipesDbContext())
            {
                File.Delete(photo.Path);
                context.Users.Load();
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
}