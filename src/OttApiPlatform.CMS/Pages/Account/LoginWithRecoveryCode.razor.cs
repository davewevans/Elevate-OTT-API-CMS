namespace OttApiPlatform.CMS.Pages.Account;

public partial class LoginWithRecoveryCode
{
    #region Public Properties

    [Parameter] public string Username { get; set; }

    #endregion Public Properties

    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private IReturnUrlProvider ReturnUrlProvider { get; set; }
    [Inject] private IAccountsClient AccountsClient { get; set; }
    [Inject] private IAuthenticationService AuthenticationService { get; set; }

    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
    private LoginWithRecoveryCodeCommand LoginWithRecoveryCodeCommand { get; } = new();

    #endregion Private Properties

    #region Private Methods

    private async Task LoginWith2FaRecoveryCode()
    {
        LoginWithRecoveryCodeCommand.UserName = Username;

        var responseWrapper = await AccountsClient.LoginWithRecoveryCode(LoginWithRecoveryCodeCommand);

        if (responseWrapper.IsSuccessStatusCode)
        {
            await AuthenticationService.Login(responseWrapper.Payload.AuthResponse);
            var returnUrl = await ReturnUrlProvider.GetReturnUrl();
            await ReturnUrlProvider.RemoveReturnUrl();
            NavigationManager.NavigateTo(returnUrl);
        }
        else
        {
            EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }

    #endregion Private Methods
}