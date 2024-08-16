namespace OttApiPlatform.CMS.Providers;

public class ReturnUrlProvider : IReturnUrlProvider
{
    #region Private Fields

    private const string ReturnUrl = "ReturnUrl";
    private readonly ILocalStorageService _localStorageService;

    #endregion Private Fields

    #region Public Constructors

    public ReturnUrlProvider(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    #endregion Public Constructors

    #region Public Methods

    public async Task<string> GetReturnUrl()
    {
        var returnUrl = await _localStorageService.GetItemAsync<string>(ReturnUrl);
        return returnUrl ?? string.Empty;
    }

    public async Task SetReturnUrl(string returnUrl)
    {
        if (!returnUrl.Contains("401"))
            await _localStorageService.SetItemAsync(ReturnUrl, returnUrl);
    }

    public async Task RemoveReturnUrl()
    {
        await _localStorageService.RemoveItemAsync(ReturnUrl);
    }

    #endregion Public Methods
}