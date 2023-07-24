using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Expense_Tracker_2._0.Services.Interfaces;

namespace Expense_Tracker_2._0.Services
{
    public class CloudService : ICloudService
    {
        private readonly IConfiguration _configuration;

        public CloudService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string UploadImage() //IFormFile imageFile
        {

            //these values are in the Secret Manager

            string cloudName = _configuration["CloudName"];
            string apiKey = _configuration["ApiKey"];
            string apiSecret = _configuration["ApiSecret"];

            Account account = new Account(cloudName, apiKey, apiSecret);

            Cloudinary cloudinary = new Cloudinary(account);
            cloudinary.Api.Secure = true;

            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(@"{filePath}"),
                Folder = "ProfilePictures",
                PublicId = "123456", //every time new unique code
            };

            var uploadResult = cloudinary.Upload(uploadParams);

            return uploadResult.SecureUrl.AbsoluteUri;
        }
        public void DeleteImage()
        {
            throw new NotImplementedException();
        }        
    }
}
