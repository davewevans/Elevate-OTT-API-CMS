namespace OttApiPlatform.WebAPI.Models;

/// <summary>
/// Represents an error in validation.
/// </summary>
public class ValidationError
{
    #region Public Properties

    /// <summary>
    /// Gets or sets the name of the property that failed validation.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the reason why the property failed validation.
    /// </summary>
    public string Reason { get; set; }

    #endregion Public Properties
}