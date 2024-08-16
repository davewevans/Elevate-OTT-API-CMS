namespace OttApiPlatform.CMS.Pages.Account.Manage;

public partial class EmailChangeConfirmed
{
    #region Public Properties

    [Parameter] public string ReturnUrl { get; set; }

    #endregion Public Properties

    #region Private Properties

    [Inject] private NavigationManager NavigationManager { get; set; }

    #endregion Private Properties

    #region Private Methods

    private void RedirectToLogin()
    {
        NavigationManager.NavigateTo("account/login");
    }

    #endregion Private Methods
}