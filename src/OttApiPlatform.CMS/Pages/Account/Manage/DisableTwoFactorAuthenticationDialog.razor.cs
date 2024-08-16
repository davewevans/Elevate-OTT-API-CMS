namespace OttApiPlatform.CMS.Pages.Account.Manage;

public partial class DisableTwoFactorAuthenticationDialog
{
    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private IManageClient ManageClient { get; set; }

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

    #endregion Private Properties

    #region Private Methods

    private async Task DisableAuthenticatorApp()
    {
        var responseWrapper = await ManageClient.Disable2Fa();

        if (responseWrapper.IsSuccessStatusCode)
        {
            MudDialog.Close(DialogResult.Ok("success"));
            Snackbar.Add(responseWrapper.Payload, Severity.Success);
        }
        else
        {
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
            if (responseWrapper.ApiErrorResponse is { Status: 401 })
                MudDialog.Cancel();
        }
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }

    #endregion Private Methods
}