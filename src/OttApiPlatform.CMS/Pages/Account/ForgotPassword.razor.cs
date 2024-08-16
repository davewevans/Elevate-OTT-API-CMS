namespace OttApiPlatform.CMS.Pages.Account;

public partial class ForgotPassword
{
    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private IAccountsClient AccountsClient { get; set; }

    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
    private ForgetPasswordCommand ForgetPasswordCommand { get; } = new();

    #endregion Private Properties

    #region Private Methods

    private async Task ForgetPassword()
    {
        var responseWrapper = await AccountsClient.ForgetPassword(ForgetPasswordCommand);

        if (responseWrapper.IsSuccessStatusCode)
        {
            Snackbar.Add(responseWrapper.Payload.SuccessMessage, Severity.Success);
            var token = !string.IsNullOrEmpty(responseWrapper.Payload.Code) ? responseWrapper.Payload.Code : Guid.NewGuid().ToString();
            NavigationManager.NavigateTo(responseWrapper.Payload.DisplayConfirmPasswordLink
                                             ? $"account/forgotPasswordConfirmation/{responseWrapper.Payload.DisplayConfirmPasswordLink}/{token}"
                                             : "account/forgotPasswordConfirmation");
        }
        else
        {
            EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }

    #endregion Private Methods
}