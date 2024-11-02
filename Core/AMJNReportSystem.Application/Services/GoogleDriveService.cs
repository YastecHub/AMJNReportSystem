using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMJNReportSystem.Application.Services
{
    using Google.Apis.Auth.OAuth2;
    using Google.Apis.Drive.v3;
    using Google.Apis.Services;
    using Google.Apis.Upload;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    public class GoogleDriveService
    {
        private readonly DriveService _driveService;

        public GoogleDriveService(IConfiguration configuration)
        {
            var clientId = configuration["GoogleDrive:ClientId"];
            var clientSecret = configuration["GoogleDrive:ClientSecret"];
            var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret
                },
                new[] { DriveService.Scope.DriveFile },
                "user",
                CancellationToken.None).Result;

            _driveService = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "YourAppName"
            });
        }

        public async Task<string> UploadFileAsync(string path)
        {
            var fileMetadata = new Google.Apis.Drive.v3.Data.File
            {
                Name = Path.GetFileName(path)
            };

            using (var stream = new FileStream(path, FileMode.Open))
            {
                var request = _driveService.Files.Create(fileMetadata, stream, GetMimeType(path));
                request.Fields = "id";
                var result = await request.UploadAsync();

                if (result.Status == UploadStatus.Failed)
                {
                    throw new Exception($"Error uploading file: {result.Exception.Message}");
                }

                return request.ResponseBody.Id;
            }
        }

        public string GetFileLink(string fileId)
        {
            return $"https://drive.google.com/file/d/{fileId}/view";
        }

        private string GetMimeType(string fileName)
        {
            var mimeType = "application/octet-stream";
            var ext = Path.GetExtension(fileName).ToLowerInvariant();
            switch (ext)
            {
                case ".pdf":
                    mimeType = "application/pdf";
                    break;
                case ".doc":
                case ".docx":
                    mimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;
                case ".xls":
                case ".xlsx":
                    mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
                case ".ppt":
                case ".pptx":
                    mimeType = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                    break;
            }
            return mimeType;
        }
    }
}
