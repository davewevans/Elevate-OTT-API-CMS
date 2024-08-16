namespace OttApiPlatform.Application.Common.Models;

/// <summary>
/// Represents metadata of a file.
/// </summary>
public class FileMetaData
{
    #region Public Properties

    /// <summary>
    /// Gets or sets the name of the file.
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// Gets or sets the URI of the file.
    /// </summary>
    public string FileUri { get; set; }

    /// <summary>
    /// Gets or sets the content type of the file.
    /// </summary>
    public string ContentType { get; set; }

    /// <summary>
    /// Gets or sets whether the file is the default file.
    /// </summary>
    public bool IsDefault { get; set; }

    #endregion Public Properties
}