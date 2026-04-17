using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using StockManager.API.Interfaces.CatalogInterfaces;

namespace StockManager.API.Services.CatalogServices
{
    public class ImageService(Cloudinary cloudinary) : IImageService
    {

        private readonly Cloudinary? _cloudinary = cloudinary;

        public async Task<string> UploadImage(Stream file, string name)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(name, file),
                PublicId = name,
                Folder = "Products",
                UseFilename = true,
                UniqueFilename = false,
                Overwrite = true
            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return result.SecureUrl.ToString();
            }

            throw new Exception("Failed to upload image to Cloudinary");
        }
    }
}
