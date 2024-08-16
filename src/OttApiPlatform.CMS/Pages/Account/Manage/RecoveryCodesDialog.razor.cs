namespace OttApiPlatform.CMS.Pages.Account.Manage;

public partial class RecoveryCodesDialog
{
    #region Public Properties

    [Parameter] public string[] RecoveryCodes { get; set; }

    #endregion Public Properties

    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private IManageClient ManageClient { get; set; }
    [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

    #endregion Private Properties

    #region Protected Methods

    protected override async Task OnInitializedAsync()
    {
        var responseWrapper = await ManageClient.GetUser();

        if (!responseWrapper.IsSuccessStatusCode)
        {
            RecoveryCodes = Array.Empty<string>();
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }

    #endregion Protected Methods

    #region Private Methods

    private void Close()
    {
        MudDialog.Close();
    }

    #endregion Private Methods
}