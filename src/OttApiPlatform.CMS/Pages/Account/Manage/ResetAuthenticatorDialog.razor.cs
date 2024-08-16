namespace OttApiPlatform.CMS.Pages.Account.Manage;

public partial class ResetAuthenticatorDialog
{
    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private IManageClient ManageClient { get; set; }
    [Inject] private IAuthenticationService AuthenticationService { get; set; }

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

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

    private async Task ResetAuthenticatorApp()
    {
        var responseWrapper = await ManageClient.ResetAuthenticator();

        if (responseWrapper.IsSuccessStatusCode)
        {
            await AuthenticationService.ReAuthenticate(responseWrapper.Payload.AuthResponse);
            MudDialog.Close(DialogResult.Ok("success"));
            Snackbar.Add(responseWrapper.Payload.StatusMessage, Severity.Success);
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