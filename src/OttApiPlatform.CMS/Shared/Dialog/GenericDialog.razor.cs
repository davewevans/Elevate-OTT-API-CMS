namespace OttApiPlatform.CMS.Shared.Dialog;

public partial class GenericDialog
{
    #region Public Properties

    [Parameter] public string ContentText { get; set; }
    [Parameter] public string ButtonText { get; set; }
    [Parameter] public Color Color { get; set; }

    #endregion Public Properties

    #region Private Properties

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

    #endregion Private Properties

    #region Private Methods

    private void Submit() => MudDialog.Close(DialogResult.Ok(true));

    private void Cancel() => MudDialog.Cancel();

    #endregion Private Methods
}