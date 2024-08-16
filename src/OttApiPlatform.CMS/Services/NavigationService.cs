namespace OttApiPlatform.CMS.Services;

public class NavigationService : INavigationService
{
    #region Private Fields

    private readonly IJSRuntime _jsRuntime;
    private readonly NavigationManager _navigationManager;

    #endregion Private Fields

    #region Public Constructors

    public NavigationService(IJSRuntime jsRuntime, NavigationManager navigationManager)
    {
        _jsRuntime = jsRuntime;
        _navigationManager = navigationManager;
    }

    #endregion Public Constructors

    #region Public Methods

    public async Task NavigateToUrlAsync(string url, bool openInNewTab)
    {
        if (openInNewTab)
            await _jsRuntime.InvokeVoidAsync("open", url, "_blank");
        else
            await _jsRuntime.InvokeVoidAsync("open", url);
    }

    public async Task NavigateToTenantPortalAsync(string tenantName, bool redirectToRegistration, bool openInNewTab)
    {
        var url = _navigationManager.BaseUri.Replace("//", $"//{tenantName}.");

        if (redirectToRegistration)
            url = $"{url}account/register";

        if (openInNewTab)
            await _jsRuntime.InvokeVoidAsync("open", url, "_blank");
        else
            await _jsRuntime.InvokeVoidAsync("open", url);
    }

    #endregion Public Methods
}