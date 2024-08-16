namespace OttApiPlatform.CMS.Pages.Account;

public partial class LoginRedirect
{
    #region Public Properties

    [Parameter] public string ReturnUrl { get; set; }

    #endregion Public Properties

    #region Private Properties

    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private IReturnUrlProvider ReturnUrlProvider { get; set; }

    #endregion Private Properties

    #region Private Methods

    private async Task RedirectToLogin()
    {
        if (!string.IsNullOrWhiteSpace(ReturnUrl))
            await ReturnUrlProvider.SetReturnUrl(ReturnUrl);

        NavigationManager.NavigateTo("account/login");
    }

    #endregion Private Methods
}