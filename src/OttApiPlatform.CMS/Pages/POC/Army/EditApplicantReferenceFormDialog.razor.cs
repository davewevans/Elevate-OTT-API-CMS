namespace OttApiPlatform.CMS.Pages.POC.Army
{
    public partial class EditApplicantReferenceFormDialog
    {
        #region Public Properties

        [Parameter] public ReferenceItemForEdit ReferenceItemForEdit { get; set; } = new();

        #endregion Public Properties

        #region Private Properties

        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        #endregion Private Properties

        #region Private Methods

        private void SubmitForm()
        {
            MudDialog.Close(DialogResult.Ok(ReferenceItemForEdit));
        }

        private void Cancel()
        {
            MudDialog.Cancel();
        }

        #endregion Private Methods
    }
}