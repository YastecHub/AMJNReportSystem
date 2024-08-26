using AMJNReportSystem.Application.Interfaces;
using AMJNReportSystem.Application.Models;

namespace AMJNReportSystem.Application.FileStorage
{
    public interface IFileStorageService : ITransientService
    {
        public Task<string> UploadAsync<T>(FileUploadRequest? request, FileType supportedFileType, CancellationToken cancellationToken = default)
        where T : class;

        public void Remove(string? path);
    }
}