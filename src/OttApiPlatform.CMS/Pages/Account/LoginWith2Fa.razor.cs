namespace OttApiPlatform.CMS.Pages.Account;

public partial class LoginWith2Fa
{
    #region Public Properties

    [Parameter] public string UserName { get; set; }

    #endregion Public Properties

    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private IReturnUrlProvider ReturnUrlProvider { get; set; }
    [Inject] private IAppStateManager AppStateManager { get; set; }
    [Inject] private IAccountsClient AccountsClient { get; set; }
    [Inject] private IAuthenticationService AuthenticationService { get; set; }

    private string RecoveryCodeUrl { get; set; }
    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
    private LoginWith2FaCommand LoginWith2FaCommand { get; } = new();

    #endregion Private Properties

    #region Protected Methods

    protected override void OnInitialized()
    {
        RecoveryCodeUrl = $"/account/LoginWithRecoveryCode/{UserName}";
    }

    #endregion Protected Methods

    #region Private Methods

    private async Task LoginWith2FaUser()
    {
        LoginWith2FaCommand.UserName = UserName;
        LoginWith2FaCommand.Password = AppStateManager.UserPasswordFor2Fa;

        var responseWrapper = await AccountsClient.LoginWith2Fa(LoginWith2FaCommand);

        if (responseWrapper.IsSuccessStatusCode)
        {
            // TODO: Implement RememberMachine logic. (Incomplete)
            //if (LoginWith2FaCommand.RememberMachine)
            //    await LocalStorageService.SetItemAsync("rememberMachine", "true");
            //else
            //    await LocalStorageService.RemoveItemAsync("rememberMachine");

            await AuthenticationService.ReAuthenticate(responseWrapper.Payload.AuthResponse);
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