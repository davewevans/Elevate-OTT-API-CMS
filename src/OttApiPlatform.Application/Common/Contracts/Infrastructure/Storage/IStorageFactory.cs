namespace OttApiPlatform.Application.Common.Contracts.Infrastructure.Storage;

/// <summary>
/// Creates an instance of the required storage provider.
/// </summary>
public interface IStorageFactory
{
    #region Public Methods

    /// <summary>
    /// Creates a new instance of a file storage service based on the specified storage type.
    /// </summary>
    /// <param name="storageTypes">The storage type to use for creating the instance.</param>
    /// <returns>The new instance of the file storage service.</returns>
    IFileStorageService CreateInstance(StorageTypes storageTypes);

    #endregion Public Methods
}