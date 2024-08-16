namespace OttApiPlatform.CMS.Models;

public class ApiResponseWrapper<T>
{
    #region Public Constructors

    public ApiResponseWrapper(T payload, bool isSuccessStatusCode, HttpStatusCode httpStatusCode)
    {
        IsSuccessStatusCode = isSuccessStatusCode;
        HttpStatusCode = httpStatusCode;
        Payload = payload;
    }

    public ApiResponseWrapper(ApiErrorResponse apiErrorResponse, bool isSuccessStatusCode, HttpStatusCode httpStatusCode)
    {
        IsSuccessStatusCode = isSuccessStatusCode;
        HttpStatusCode = httpStatusCode;
        ApiErrorResponse = apiErrorResponse;
    }

    #endregion Public Constructors

    #region Public Properties

    public bool IsSuccessStatusCode { get; }
    public HttpStatusCode HttpStatusCode { get; set; }
    public T Payload { get; }
    public ApiErrorResponse ApiErrorResponse { get; }

    #endregion Public Properties
}