using API.Helpers;
using API.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace API.Services
{
    public class PhotoService : IPhotoService
    {

        private readonly Cloudinary cloudinary;

        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(
                config.Value.CloudName, // access to configuration value in appsettings.json
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            cloudinary = new Cloudinary(acc);
        }
        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult(); 
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream(); // open file stream to read file 
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream), // file name and stream
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face"), // transformation to apply to image  
                    // face - crop image to face
                    Folder= "da-net7"
                };
                uploadResult = await cloudinary.UploadAsync(uploadParams);
            }
            return uploadResult;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var result = await cloudinary.DestroyAsync(deleteParams);
            return result;
        }
    }
}

