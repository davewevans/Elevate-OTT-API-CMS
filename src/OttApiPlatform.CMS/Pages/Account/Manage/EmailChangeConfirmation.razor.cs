namespace OttApiPlatform.CMS.Pages.Account.Manage;

public partial class EmailChangeConfirmation
{
    #region Public Properties

    [Parameter] public bool DisplayConfirmAccountLink { get; set; }
    [Parameter] public string EmailConfirmationUrl { get; set; }

    #endregion Public Properties

    #region Private Properties

    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private IAuthenticationService AuthenticationService { get; set; }

    #endregion Private Properties

    #region Private Methods

    private async Task RedirectToEmailConfirmationUrl(string emailConfirmationUrl)
    {
        await AuthenticationService.Logout();

        NavigationManager.NavigateTo(emailConfirmationUrl);
    }

    #endregion Private Methods
}