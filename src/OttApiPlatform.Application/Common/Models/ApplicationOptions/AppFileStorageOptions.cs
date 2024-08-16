namespace OttApiPlatform.Application.Common.Models.ApplicationOptions;

/// <summary>
/// Represents the options related to file storage in the application.
/// </summary>
public class AppFileStorageOptions
{
    #region Public Fields

    /// <summary>
    /// Gets the name of the section in the configuration file where these options can be found.
    /// </summary>
    public const string Section = "AppFileStorageOptions";

    #endregion Public Fields

    #region Public Properties

    /// <summary>
    /// Gets or sets the identifier of the file storage settings.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the type of the file storage. 0 for local file system storage and 1 for
    /// cloud-based storage.
    /// </summary>
    public int StorageType { get; set; }

    #endregion Public Properties

    #region Public Methods

    /// <summary>
    /// Maps the <see cref="AppFileStorageOptions"/> object to a <see cref="FileStorageSettings"/> object.
    /// </summary>
    /// <returns>A <see cref="FileStorageSettings"/> object that is mapped from this object.</returns>
    public FileStorageSettings MapToEntity()
    {
        return new()
        {
            StorageType = (StorageTypes)Enum.Parse(typeof(StorageTypes), StorageType.ToString(), true),
        };
    }

    #endregion Public Methods
}