namespace OttApiPlatform.CMS.Pages.Account;

public partial class Login
{
    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private IReturnUrlProvider ReturnUrlProvider { get; set; }
    [Inject] private IAppStateManager AppStateManager { get; set; }
    [Inject] private IAccountsClient AccountsClient { get; set; }
    [Inject] private IAuthenticationService AuthenticationService { get; set; }

    private bool PasswordVisibility { get; set; }
    private string PasswordInputIcon { get; set; } = Icons.Material.Filled.VisibilityOff;
    private InputType PasswordInput { get; set; } = InputType.Password;
    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
    private LoginCommand LoginCommand { get; } = new();

    #endregion Private Properties

    #region Protected Methods

    protected override void OnInitialized()
    {
        LoginCommand.Email = "davewevans72+45@gmail.com";
        LoginCommand.Password = "Passw0rd!";
    }

    #endregion Protected Methods

    #region Private Methods

    private async Task LoginUser()
    {
        var responseWrapper = await AccountsClient.Login(LoginCommand);

        if (responseWrapper.IsSuccessStatusCode)
        {
            // TODO: Implement RememberMachine logic. (Incomplete)
            //var rememberThisMachine = await LocalStorageService.GetItemAsync<string>("rememberMachine") is null;

            if (responseWrapper.Payload.RequiresTwoFactor)
            {
                NavigationManager.NavigateTo($"account/loginWith2Fa/{LoginCommand.Email}");
                AppStateManager.UserPasswordFor2Fa = LoginCommand.Password;
            }
            else
            {
                await AuthenticationService.Login(responseWrapper.Payload.AuthResponse);
                var returnUrl = await ReturnUrlProvider.GetReturnUrl();
                await ReturnUrlProvider.RemoveReturnUrl();
                NavigationManager.NavigateTo(returnUrl);
            }
        }
        else
        {
            EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }

    private void TogglePasswordVisibility()
    {
        if (PasswordVisibility)
        {
            PasswordVisibility = false;
            PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
            PasswordInput = InputType.Password;
        }
        else
        {
            PasswordVisibility = true;
            PasswordInputIcon = Icons.Material.Filled.Visibility;
            PasswordInput = InputType.Text;
        }
    }

    #endregion Private Methods
}