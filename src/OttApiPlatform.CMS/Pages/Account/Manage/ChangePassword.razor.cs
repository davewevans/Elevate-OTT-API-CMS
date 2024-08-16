namespace OttApiPlatform.CMS.Pages.Account.Manage;

public partial class ChangePassword
{
    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private IManageClient ManageClient { get; set; }
    [Inject] private IAuthenticationService AuthenticationService { get; set; }

    private bool PasswordVisibility { get; set; }
    private string PasswordInputIcon { get; set; } = Icons.Material.Filled.VisibilityOff;
    private InputType PasswordInput { get; set; } = InputType.Password;
    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
    private ChangePasswordCommand ChangePasswordCommand { get; set; } = new();

    #endregion Private Properties

    #region Protected Methods

    protected override async Task OnInitializedAsync()
    {
        var responseWrapper = await ManageClient.GetUser();

        if (!responseWrapper.IsSuccessStatusCode)
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
    }

    #endregion Protected Methods

    #region Private Methods

    private async Task ChangeUserPassword()
    {
        var responseWrapper = await ManageClient.ChangePassword(ChangePasswordCommand);

        if (responseWrapper.IsSuccessStatusCode)
        {
            await AuthenticationService.ReAuthenticate(responseWrapper.Payload.AuthResponse);
            Snackbar.Add(responseWrapper.Payload.SuccessMessage, Severity.Success);
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