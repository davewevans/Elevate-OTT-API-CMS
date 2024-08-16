namespace OttApiPlatform.CMS.Providers;

public class AccessTokenProvider : IAccessTokenProvider
{
    #region Private Fields

    private readonly ILocalStorageService _localStorage;

    #endregion Private Fields

    #region Public Constructors

    public AccessTokenProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    #endregion Public Constructors

    #region Public Methods

    public async Task<string> TryGetAccessToken()
    {
        return await _localStorage.GetItemAsync<string>("AccessToken");
    }

    #endregion Public Methods
}