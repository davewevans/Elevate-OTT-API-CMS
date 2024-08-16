namespace OttApiPlatform.CMS.Pages.Account;

public partial class ForgotPasswordConfirmation
{
    #region Public Properties

    [Parameter] public string Code { get; set; }
    [Parameter] public bool DisplayConfirmPasswordLink { get; set; }

    #endregion Public Properties

    #region Private Properties

    [Inject] private NavigationManager NavigationManager { get; set; }

    #endregion Private Properties

    #region Private Methods

    private void ResetPassword()
    {
        NavigationManager.NavigateTo($"account/resetPassword/{Code}");
    }

    #endregion Private Methods
}