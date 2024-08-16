namespace OttApiPlatform.Application.Common.Models;

/// <summary>
/// Envelope that wraps an HTTP response and includes a payload of type TResponse.
/// </summary>
/// <typeparam name="TResponse">The type of the payload.</typeparam>
public class Envelope<TResponse> : EnvelopeBase
{
    #region Private Fields

    /// <summary>
    /// Concurrent dictionary to store the result, which allows multiple threads to read and write
    /// to it simultaneously without any locks.
    /// </summary>
    private static readonly ConcurrentDictionary<Type, object> _payload = new();

    #endregion Private Fields

    #region Public Constructors

    /// <summary>
    /// Constructor that initializes a success envelope with a payload.
    /// </summary>
    /// <param name="keyValuePairs">The dictionary of errors to include in the envelope.</param>
    public Envelope(Dictionary<string, string> keyValuePairs) : base(keyValuePairs)
    {
    }

    /// <summary>
    /// Constructor that initializes a success envelope with a payload.
    /// </summary>
    /// <param name="payload">The payload to include in the envelope.</param>
    public Envelope(TResponse payload) : this(null)
    {
        IsError = false;
        Payload = payload;
    }

    #endregion Public Constructors

    #region Private Constructors

    /// <summary>
    /// Default constructor that sets default values for properties.
    /// </summary>
    private Envelope()
    {
        IsError = false;
        Title = string.Empty;
        ValidationErrors = new Dictionary<string, string>();
    }

    #endregion Private Constructors

    #region Public Properties

    /// <summary>
    /// A generic envelope that wraps the response payload along with additional metadata.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response payload.</typeparam>
    public static Envelope<TResponse> Result
    {
        get
        {
            var type = typeof(Envelope<TResponse>);
            if (_payload.TryGetValue(type, out var payload))
                return (Envelope<TResponse>)payload;

            var newPayload = new Envelope<TResponse>();
            _payload.TryAdd(type, newPayload);
            return newPayload;
        }
    }

    /// <summary>
    /// Gets or sets the response payload of the envelope.
    /// </summary>
    public TResponse Payload { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether the rollback is disabled.
    /// </summary>
    public bool RollbackDisabled { get; private set; }

    #endregion Public Properties

    #region Public Methods

    /// <summary>
    /// Sets the response envelope to Ok status with a message of "Success".
    /// </summary>
    /// <returns>The current envelope instance.</returns>
    public Envelope<TResponse> Ok()
    {
        IsError = false;
        ValidationErrors = new Dictionary<string, string>();
        HttpStatusCode = HttpStatusCode.OK;
        Title = "Success";
        return this;
    }

    /// <summary>
    /// Sets the response envelope to Ok status with the provided payload and a message of "Success".
    /// </summary>
    /// <param name="payload">The response payload to include in the envelope.</param>
    /// <returns>The current envelope instance.</returns>
    public Envelope<TResponse> Ok(TResponse payload)
    {
        IsError = false;
        ValidationErrors = new Dictionary<string, string>();
        Title = "Success";
        Payload = payload;
        return this;
    }

    /// <summary>
    /// Sets the response envelope to ServerError status with the provided error message and
    /// rollback disabled status.
    /// </summary>
    /// <param name="title">The error message to include in the envelope.</param>
    /// <param name="rollbackDisabled">An optional boolean indicating whether to disable rollback.</param>
    /// <returns>The current envelope instance.</returns>
    public Envelope<TResponse> ServerError(string title = "Internal Server Error", bool rollbackDisabled = false)
    {
        IsError = true;
        RollbackDisabled = rollbackDisabled;
        ValidationErrors = new Dictionary<string, string>();
        HttpStatusCode = HttpStatusCode.InternalServerError;
        Title = title;
        return this;
    }

    /// <summary>
    /// Sets the response envelope to NotFound status with the provided error message and rollback
    /// disabled status.
    /// </summary>
    /// <param name="title">The error message to include in the envelope.</param>
    /// <param name="rollbackDisabled">An optional boolean indicating whether to disable rollback.</param>
    /// <returns>The current envelope instance.</returns>
    public Envelope<TResponse> NotFound(string title = "Not Found", bool rollbackDisabled = false)
    {
        IsError = true;
        RollbackDisabled = rollbackDisabled;
        ValidationErrors = new Dictionary<string, string>();
        HttpStatusCode = HttpStatusCode.NotFound;
        Title = title;
        return this;
    }

    /// <summary>
    /// Sets the response envelope to BadRequest status with the provided error message and rollback
    /// disabled status.
    /// </summary>
    /// <param name="title">The error message to include in the envelope.</param>
    /// <param name="rollbackDisabled">An optional boolean indicating whether to disable rollback.</param>
    /// <returns>The current envelope instance.</returns>
    public Envelope<TResponse> BadRequest(string title = "Bad Request", bool rollbackDisabled = false)
    {
        IsError = true;
        RollbackDisabled = rollbackDisabled;
        ValidationErrors = new Dictionary<string, string>();
        HttpStatusCode = HttpStatusCode.BadRequest;
        Title = title;
        return this;
    }

    /// <summary>
    /// Adds errors to the envelope, setting the <see cref="EnvelopeBase.IsError"/> property to
    /// true, the <see cref="RollbackDisabled"/> property to the specified value, the <see
    /// cref="HttpStatusCode"/> property to the specified value, and the <see cref="Message"/>
    /// property to the specified message.
    /// </summary>
    /// <param name="keyValuePairs">A dictionary containing key-value pairs representing errors.</param>
    /// <param name="httpStatusCode">The <see cref="HttpStatusCode"/> for the error.</param>
    /// <param name="message">An optional string representing a custom error message.</param>
    /// <param name="rollbackDisabled">An optional boolean indicating whether to disable rollback.</param>
    /// <returns>The <see cref="Envelope{TResponse}"/> instance with the added errors.</returns>
    public Envelope<TResponse> AddErrors(Dictionary<string, string> keyValuePairs, HttpStatusCode httpStatusCode, string message = null, bool rollbackDisabled = false)
    {
        IsError = true;
        RollbackDisabled = rollbackDisabled;
        Title = message;
        HttpStatusCode = httpStatusCode;
        ValidationErrors = new Dictionary<string, string>(keyValuePairs);
        return this;
    }

    /// <summary>
    /// Adds an unauthorized error to the envelope, setting the <see cref="EnvelopeBase.IsError"/>
    /// property to true, the <see cref="RollbackDisabled"/> property to the specified value, the
    /// <see cref="HttpStatusCode"/> property to <see cref="HttpStatusCode.Unauthorized"/>, and the
    /// <see cref="Message"/> property to the specified error message.
    /// </summary>
    /// <param name="title">A string representing the error message.</param>
    /// <param name="rollbackDisabled">An optional boolean indicating whether to disable rollback.</param>
    /// <returns>The <see cref="Envelope{TResponse}"/> instance with the added error.</returns>
    public Envelope<TResponse> Unauthorized(string title, bool rollbackDisabled = false)
    {
        IsError = true;
        RollbackDisabled = rollbackDisabled;
        ValidationErrors = new Dictionary<string, string>();
        HttpStatusCode = HttpStatusCode.Unauthorized;
        Title = title;
        return this;
    }

    #endregion Public Methods
}