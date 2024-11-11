namespace OttApiPlatform.Application.Common.Contracts.StorageServices;

public interface IStorageProvider
{
    #region Public Methods
    Task<IAzureFileStorageService> InvokeInstanceAsync();
    IAzureFileStorageService InvokeInstanceForAzureStorage();

    #endregion Public Methods
}
