using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace AMJNReportSystem.Application.Services
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService()
        {
            // Replace these with your Cloudinary account details
            var account = new Account(
                "hamiid137",
                "c667058e2c57edb64e873f81a77634",
                "abulhamiideeee");

            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadPdfAsync(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("The specified file does not exist.", filePath);
            }

            var uploadParams = new RawUploadParams()
            {
                File = new FileDescription(filePath),

                Folder = "pdf_uploads"           
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Upload successful!");
                return uploadResult.SecureUrl.ToString(); // Returns the URL of the uploaded file
            }
            else
            {
               return string.Empty;
                //throw new Exception($"Cloudinary upload error: {uploadResult.Error.Message}");
            }
        }
    }
}
