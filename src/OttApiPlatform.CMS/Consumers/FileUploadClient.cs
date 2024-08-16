namespace OttApiPlatform.CMS.Consumers;

public class FileUploadClient : IFileUploadClient
{
    #region Private Fields

    private readonly IHttpService _httpService;

    #endregion Private Fields

    #region Public Constructors

    public FileUploadClient(IHttpService httpService)
    {
        _httpService = httpService;
    }

    #endregion Public Constructors

    #region Public Methods

    public async Task<ApiResponseWrapper<FileUploadResponse>> UploadFile(MultipartFormDataContent request)
    {
        return await _httpService.PostFormData<MultipartFormDataContent, FileUploadResponse>("fileUpload/uploadFile", request);
    }

    #endregion Public Methods
}