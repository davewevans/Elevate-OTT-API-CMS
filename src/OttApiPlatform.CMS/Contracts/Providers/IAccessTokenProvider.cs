namespace OttApiPlatform.CMS.Contracts.Providers;

public interface IAccessTokenProvider
{
    #region Public Methods

    Task<string> TryGetAccessToken();

    #endregion Public Methods
}