namespace OttApiPlatform.CMS.Pages.Account.Manage;

public partial class GenerateRecoveryCodesDialog
{
    #region Private Properties

    private string[] RecoveryCodes { get; set; }

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private IManageClient ManageClient { get; set; }

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
    private string StatusMessage { get; set; }
    private bool IsTwoFactorEnabled { get; set; } = true;

    #endregion Private Properties

    #region Protected Methods

    protected override async Task OnInitializedAsync()
    {
        var responseWrapper = await ManageClient.CheckUser2FaState();

        if (responseWrapper.IsSuccessStatusCode)
        {
            if (responseWrapper.Payload.IsTwoFactorEnabled == false)
            {
                IsTwoFactorEnabled = false;
                StatusMessage = responseWrapper.Payload.StatusMessage;
            }
        }
        else
        {
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }

    #endregion Protected Methods

    #region Private Methods

    private async Task Generate2FaRecoveryCodes()
    {
        var responseWrapper = await ManageClient.GenerateRecoveryCodes();

        if (responseWrapper.IsSuccessStatusCode)
        {
            if (responseWrapper.Payload.RecoveryCodes?.Any() != true)
                NavigationManager.NavigateTo("account/manage/twoFactorAuthentication");

            if (responseWrapper.Payload.RecoveryCodes != null)
            {
                RecoveryCodes = responseWrapper.Payload.RecoveryCodes.ToArray();
                MudDialog.Close(DialogResult.Ok("success"));
                Snackbar.Add(responseWrapper.Payload.StatusMessage, Severity.Success);
                ShowRecoveryCodes();
            }
        }
        else
        {
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
            if (responseWrapper.ApiErrorResponse is { Status: 401 })
                MudDialog.Cancel();
        }
    }

    private void ShowRecoveryCodes()
    {
        MudDialog.Close(DialogResult.Ok("success"));
        var dialogParameters = new DialogParameters { { nameof(RecoveryCodes), RecoveryCodes } };
        DialogService.Show<RecoveryCodesDialog>(string.Empty, dialogParameters);
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }

    #endregion Private Methods
}