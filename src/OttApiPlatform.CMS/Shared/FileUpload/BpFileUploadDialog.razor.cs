namespace OttApiPlatform.CMS.Shared.FileUpload;

public partial class BpFileUploadDialog
{
    #region Public Properties

    [Parameter] public string FileName { get; set; }

    #endregion Public Properties

    #region Private Properties

    [Inject] private ISnackbar Snackbar { get; set; }

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

    #endregion Private Properties

    #region Private Methods

    private void RenameFile()
    {
        MudDialog.Close(DialogResult.Ok(FileName));
    }

    private void Cancel()
    {
        Snackbar.Add(Resource.File_upload_has_been_cancelled, Severity.Success);
        MudDialog.Cancel();
    }

    #endregion Private Methods
}