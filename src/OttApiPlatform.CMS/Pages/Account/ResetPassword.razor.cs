namespace OttApiPlatform.CMS.Pages.Account;

public partial class ResetPassword
{
    #region Public Properties

    [Parameter] public string Code { get; set; }

    #endregion Public Properties

    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private IAccountsClient AccountsClient { get; set; }

    private bool PasswordVisibility { get; set; }
    private string PasswordInputIcon { get; set; } = Icons.Material.Filled.VisibilityOff;
    private InputType PasswordInput { get; set; } = InputType.Password;
    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
    private ResetPasswordCommand ResetPasswordCommand { get; } = new();

    #endregion Private Properties

    #region Private Methods

    private async Task ResetUserPassword()
    {
        ResetPasswordCommand.Code = Code;

        var responseWrapper = await AccountsClient.ResetPassword(ResetPasswordCommand);

        if (responseWrapper.IsSuccessStatusCode)
        {
            Snackbar.Add(responseWrapper.Payload, Severity.Success);
            NavigationManager.NavigateTo("account/resetPasswordConfirmation");
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