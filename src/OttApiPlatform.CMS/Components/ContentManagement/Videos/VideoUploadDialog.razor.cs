namespace OttApiPlatform.CMS.Components.ContentManagement.Videos;

public partial class VideoUploadDialog : ComponentBase
{
    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }


    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; }

    private void Close() => MudDialog.Close(DialogResult.Ok(true));
}
