namespace OttApiPlatform.Gateway.Models;

public class ApiResponseWrapper<T>
{
    #region Public Constructors

    public ApiResponseWrapper(T payload, bool isSuccessStatusCode, HttpStatusCode statusCode)
    {
        IsSuccessStatusCode = isSuccessStatusCode;
        StatusCode = statusCode;
        Payload = payload;
    }

    public ApiResponseWrapper(ApiErrorResponse apiErrorResponse, bool isSuccessStatusCode, HttpStatusCode statusCode)
    {
        IsSuccessStatusCode = isSuccessStatusCode;
        StatusCode = statusCode;
        ApiErrorResponse = apiErrorResponse;
    }

    #endregion Public Constructors

    #region Public Properties

    public bool IsSuccessStatusCode { get; }
    public HttpStatusCode StatusCode { get; set; }
    public T Payload { get; }
    public ApiErrorResponse ApiErrorResponse { get; }

    #endregion Public Properties
}