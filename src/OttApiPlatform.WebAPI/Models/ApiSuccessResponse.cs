namespace OttApiPlatform.WebAPI.Models;

public class ApiSuccessResponse<T>
{
    #region Public Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiSuccessResponse{T}"/> class.
    /// </summary>
    /// <param name="response">The envelope containing the response payload.</param>
    /// <param name="path">The request path.</param>
    public ApiSuccessResponse(Envelope<T> response, PathString path)
    {
        Status = (int)HttpStatusCode.OK;
        Type = "https://httpstatuses.com/200";
        Instance = path;
        Payload = response.Payload;
    }

    #endregion Public Constructors

    #region Public Properties

    /// <summary>
    /// Gets or sets the HTTP status code.
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// Gets or sets the type of the response.
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Gets or sets the instance of the request.
    /// </summary>
    public string Instance { get; set; }

    /// <summary>
    /// Gets or sets the response payload.
    /// </summary>
    public T Payload { get; set; }

    #endregion Public Properties
}