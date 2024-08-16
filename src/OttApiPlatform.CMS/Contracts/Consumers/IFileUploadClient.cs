namespace OttApiPlatform.CMS.Contracts.Consumers;

/// <summary>
/// Provides methods for uploading files.
/// </summary>
public interface IFileUploadClient
{
    #region Public Methods

    /// <summary>
    /// Uploads a file.
    /// </summary>
    /// <param name="request">The file content to upload.</param>
    /// <returns>A <see cref="FileUploadResponse"/>.</returns>
    Task<ApiResponseWrapper<FileUploadResponse>> UploadFile(MultipartFormDataContent request);

    #endregion Public Methods
}