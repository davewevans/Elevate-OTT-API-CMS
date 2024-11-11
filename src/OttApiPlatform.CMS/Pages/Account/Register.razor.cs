namespace OttApiPlatform.CMS.Pages.Account;

// TODO check if email is already used
// TODO add link to log in if user already has an account
// TODO country flag icons
// TODO check if channel name is already used
// TODO add terms and conditions
// TODO add privacy policy
// TODO add recaptcha
// TODO Replace text with Resources for localization

public partial class Register
{
    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private IReturnUrlProvider ReturnUrlProvider { get; set; }
    [Inject] private IAccountsClient AccountsClient { get; set; }
    [Inject] private IAuthenticationService AuthenticationService { get; set; }

    private bool PasswordVisibility { get; set; }
    private string PasswordInputIcon { get; set; } = Icons.Material.Filled.VisibilityOff;
    private InputType PasswordInput { get; set; } = InputType.Password;
    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
    private RegisterCommand RegisterCommand { get; } = new();
    private string SelectedCountryCode { get; set; } = "+1";

    #endregion Private Properties

    #region Private Methods

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

    private void OnChannelNameChanged(string channelName)
    {
        RegisterCommand.ChannelName = channelName;

        // Remove non-alphanumeric characters and spaces
        var sanitizedChannelName = new string(channelName
                .Where(char.IsLetterOrDigit)
                .ToArray())
                .Replace(" ", "")
                .ToLower();

        RegisterCommand.SubDomain = sanitizedChannelName;
    }

    private async Task RegisterUser()
    {
        Console.WriteLine("RegisterUser called");

        var responseWrapper = await AccountsClient.Register(RegisterCommand);

        if (responseWrapper.IsSuccessStatusCode)
        {
            if (responseWrapper.Payload.RequireConfirmedAccount)
            {
                NavigationManager.NavigateTo(responseWrapper.Payload.DisplayConfirmAccountLink
                    ? $"account/registerConfirmation/{responseWrapper.Payload.DisplayConfirmAccountLink}/{responseWrapper.Payload.EmailConfirmationUrl}"
                    : "account/registerConfirmation");
            }
            else
            {
                await AuthenticationService.Login(responseWrapper.Payload.AuthResponse);
                var returnUrl = await ReturnUrlProvider.GetReturnUrl();
                await ReturnUrlProvider.RemoveReturnUrl();
                NavigationManager.NavigateTo(returnUrl, forceLoad: true);
            }
        }
        else
        {
            EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }

    #endregion Private Methods
}