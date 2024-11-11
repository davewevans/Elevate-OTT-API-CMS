using OttApiPlatform.Application.Features.ContentManagement.Videos.Queries.GetSasToken;

namespace OttApiPlatform.Application.Common.Contracts.StorageServices;

public interface IAzureFileStorageService
{
    #region Public Methods
    SasTokenResponse? GetSasTokenForVideoContainer();

    Task<string?> UploadFile(IFormFile formFile, string containerName, string fileNamePrefix);

    Task<List<FileMetaData>> UploadMultipleFiles(IList<IFormFile> formFiles, string containerName, string fileNamePrefix, int defaultFileIndex = 0, string subContainerName = "attachments");

    Task<string?> EditFile(IFormFile formFile, string containerName, string fileNamePrefix, string oldFileUri);

    Task DeleteFileIfExists(string fileUri, string containerName);

    Task DeleteContainer(string containerName, string subContainerName);

    FileStatus GetFileState(IFormFile? formFile, string? oldUrl);

    #endregion Public Methods
}
