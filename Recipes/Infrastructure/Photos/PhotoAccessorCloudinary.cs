using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Recipes.Domain;
using Recipes.Infrastructure.Interfaces;
using System;
using System.Threading.Tasks;

namespace Recipes.Infrastructure.Photos
{
    public class PhotoAccessorCloudinary : IPhotoAccessor
    {
        private readonly Cloudinary cloudinary;
        public PhotoAccessorCloudinary(IOptions<CloudinarySettings> config)
        {
            cloudinary = new Cloudinary(new Account
            {
                ApiKey = config.Value.ApiKey,
                ApiSecret = config.Value.ApiSecret,
                Cloud = config.Value.CloudName
            });
        }

        public Photo CreatePhoto(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.FileName, stream),
                        Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                    };
                    uploadResult = cloudinary.Upload(uploadParams);
                }
            }

            if (uploadResult.Error != null)
                throw new Exception(uploadResult.Error.Message);

            return new Photo()
            {
                Id = uploadResult.PublicId,
                Url = uploadResult.SecureUrl.AbsoluteUri,
                Size = uploadResult.Bytes

            };

        }

        public async Task<bool> DeletePhotoAsync(Photo photo)
        {
            var deleteParams = new DeletionParams(photo.Id);

            var result = await cloudinary.DestroyAsync(deleteParams);

            return result.Result == "ok";
        }
    }
}
