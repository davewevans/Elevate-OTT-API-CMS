namespace OttApiPlatform.WebAPI.Models;

/// <summary>
/// Represents an API error response.
/// </summary>
public class ApiErrorResponse
{
    #region Public Properties

    /// <summary>
    /// Gets or sets the title of the error.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the HTTP status code of the response.
    /// </summary>
    public HttpStatusCode Status { get; set; }

    /// <summary>
    /// Gets or sets the type of the error.
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Gets or sets the instance of the error.
    /// </summary>
    public string Instance { get; set; }

    /// <summary>
    /// Gets or sets the error message.
    /// </summary>
    public string ErrorMessage { get; set; }

    /// <summary>
    /// Gets or sets the inner exception that caused the error.
    /// </summary>
    public object InnerException { get; set; }

    /// <summary>
    /// Gets or sets representation of the onDemand frames on the call stack at the time when the
    /// current exception was thrown.
    /// </summary>
    public string StackTrace { get; set; }

    /// <summary>
    /// Gets or sets the list of validation errors.
    /// </summary>
    public List<ValidationError> ValidationErrors { get; set; }

    #endregion Public Properties
}