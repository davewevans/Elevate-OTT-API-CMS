using OttApiPlatform.Application.Features.ContentManagement.Videos.Queries.GetSasToken;

namespace OttApiPlatform.Application.Common.Contracts.Infrastructure.Storage;

/// <summary>
/// Defines a contract for a file storage service that can be used in multiple cloud storage
/// providers like Azure, AWS, and local servers.
/// </summary>
public interface IFileStorageService
{
    #region Public Methods

    /// <summary>
    /// Uploads a file to the specified directory path.
    /// </summary>
    /// <param name="formFile">The file to upload.</param>
    /// <param name="directoryPath">The directory path to upload the file to.</param>
    /// <param name="fileRenameAllowed">Specifies whether file renaming is allowed.</param>
    /// <param name="baseUrl">The base URL to use for generating the file URL (optional).</param>
    /// <param name="cancellationToken">The cancellation token (optional).</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the URL of the
    /// uploaded file.
    /// </returns>
    Task<FileMetaData> UploadFile(IFormFile formFile,
                                  string directoryPath,
                                  bool fileRenameAllowed = false,
                                  string baseUrl = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Uploads multiple files to the specified directory path.
    /// </summary>
    /// <param name="formFiles">The list of files to upload.</param>
    /// <param name="directoryPath">The directory path to upload the files to.</param>
    /// <param name="fileRenameAllowed">Specifies whether file renaming is allowed.</param>
    /// <param name="defaultFileIndex">
    /// The default index to use when renaming files (if file renaming is allowed).
    /// </param>
    /// <param name="baseUrl">The base URL to use for generating file URLs (optional).</param>
    /// <param name="cancellationToken">The cancellation token (optional).</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of file metadata.
    /// </returns>
    Task<List<FileMetaData>> UploadMultipleFiles(IList<IFormFile> formFiles,
                                                 string directoryPath,
                                                 bool fileRenameAllowed = false,
                                                 int defaultFileIndex = 0,
                                                 string baseUrl = null,
                                                 CancellationToken cancellationToken = default);

    /// <summary>
    /// Replaces the old file with the new file in the specified directory path and returns the URL.
    /// of the new file.
    /// </summary>
    /// <param name="formFile">The new file to replace the old file.</param>
    /// <param name="directoryPath">The directory path where the files should be uploaded.</param>
    /// <param name="oldFileUri">The URL of the old file to replace.</param>
    /// <param name="rootFolderName">The folder name that contains the file to replace.</param>
    /// <param name="cancellationToken">The token used to cancel the operation if needed.</param>
    /// <returns>The URL of the new file.</returns>
    Task<FileMetaData> EditFile(IFormFile formFile,
                                string directoryPath,
                                string oldFileUri,
                                string rootFolderName = null,
                                CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the file with the specified URL if it exists.
    /// </summary>
    /// <param name="fileUri">The URL of the file to delete.</param>
    /// <param name="rootFolderName">The folder name that contains the file to delete.</param>
    Task DeleteFileIfExists(string fileUri, string rootFolderName = default);

    /// <summary>
    /// Gets the state of the file (new, deleted, edited, or unchanged).
    /// </summary>
    /// <param name="formFile">The file to get the state of.</param>
    /// <param name="oldUrl">The URL of the old file.</param>
    /// <returns>The state of the file.</returns>
    FileStatus GetFileState(IFormFile formFile, string oldUrl);

    SasTokenResponse GetSasTokenForVideoContainer();

    #endregion Public Methods
}