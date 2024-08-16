namespace OttApiPlatform.Application.Common.Models;

/// <summary>
/// Base class for an envelope that wraps an HTTP response.
/// </summary>
public abstract class EnvelopeBase
{
    #region Protected Constructors

    /// <summary>
    /// Default constructor that sets default values for properties.
    /// </summary>
    protected EnvelopeBase()
    {
        IsError = false;
        Title = string.Empty;
        ValidationErrors = new Dictionary<string, string>();
    }

    /// <summary>
    /// Constructor that initializes an error envelope.
    /// </summary>
    /// <param name="keyValuePairs">Dictionary of model state errors.</param>
    protected EnvelopeBase(Dictionary<string, string> keyValuePairs)
    {
        IsError = true;
        Title = null;
        ValidationErrors = keyValuePairs;
    }

    #endregion Protected Constructors

    #region Public Properties

    /// <summary>
    /// Gets or sets a value indicating whether this is an error envelope.
    /// </summary>
    public bool IsError { get; protected set; }

    /// <summary>
    /// Gets or sets the HTTP status code.
    /// </summary>
    public HttpStatusCode HttpStatusCode { get; protected set; }

    /// <summary>
    /// Gets or sets the message associated with the envelope.
    /// </summary>
    public string Title { get; protected set; }

    /// <summary>
    /// Gets or sets a dictionary of model state errors.
    /// </summary>
    public Dictionary<string, string> ValidationErrors { get; protected set; }

    #endregion Public Properties
}