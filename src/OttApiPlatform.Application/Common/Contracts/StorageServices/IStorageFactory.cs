namespace OttApiPlatform.Application.Common.Contracts.StorageServices;

public interface IStorageFactory
{
    #region Public Methods
    IAzureFileStorageService CreateInstance(StorageTypes storageTypes);
    #endregion Public Methods
}
