namespace OttApiPlatform.CMS.Pages.Account.Manage;

public partial class EnableAuthenticatorDialog
{
    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private IManageClient ManageClient { get; set; }
    [Inject] private IJSRuntime Js { get; set; }

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
    private LoadSharedKeyAndQrCodeUriResponse LoadSharedKeyAndQrCodeUriResponse { get; set; } = new();
    private EnableAuthenticatorCommand EnableAuthenticatorCommand { get; } = new();

    #endregion Private Properties

    #region Protected Methods

    protected override async Task OnInitializedAsync()
    {
        var responseWrapper = await ManageClient.LoadSharedKeyAndQrCodeUri();

        if (responseWrapper.IsSuccessStatusCode)
        {
            LoadSharedKeyAndQrCodeUriResponse = responseWrapper.Payload;
            await GenerateQrCode();
        }
        else
        {
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }

    #endregion Protected Methods

    #region Private Methods

    private async Task GenerateQrCode()
    {
        await Js.InvokeVoidAsync("generateQrCode", LoadSharedKeyAndQrCodeUriResponse.AuthenticatorUri);
    }

    private async Task EnableAuthenticatorApp()
    {
        var responseWrapper = await ManageClient.EnableAuthenticator(EnableAuthenticatorCommand);

        if (responseWrapper.IsSuccessStatusCode)
        {
            Snackbar.Add(responseWrapper.Payload.SuccessMessage, Severity.Success);

            if (responseWrapper.Payload.ShowRecoveryCodes)
            {
                MudDialog.Close(DialogResult.Ok("success"));
                ShowRecoveryCodes(responseWrapper.Payload.RecoveryCodes);
            }
            else
            {
                Snackbar.Add(responseWrapper.Payload.SuccessMessage, Severity.Success);
                MudDialog.Close(DialogResult.Ok("success"));
            }
        }
        else
        {
            EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
            MudDialog.Cancel();
        }
    }

    private void ShowRecoveryCodes(string[] recoveryCodes)
    {
        var dialogParameters = new DialogParameters { { nameof(recoveryCodes), recoveryCodes } };
        DialogService.Show<RecoveryCodesDialog>(string.Empty, dialogParameters);
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }

    #endregion Private Methods
}