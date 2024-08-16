namespace OttApiPlatform.CMS.Pages.Identity.Users;

public partial class UserAttachmentsForEdit
{
    #region Public Properties

    [Parameter] public List<AssignedUserAttachmentItem> AttachmentsList { get; set; } = new();
    [Parameter] public EventCallback<Guid> OnAttachmentRemoved { get; set; }

    #endregion Public Properties

    #region Private Properties

    [Inject] private IDialogService DialogService { get; set; }

    private string SearchText { get; set; }

    #endregion Private Properties

    #region Private Methods

    private bool FilterAttachments(AssignedUserAttachmentItem attachmentItem)
    {
        return string.IsNullOrWhiteSpace(SearchText) || attachmentItem.FileName.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
    }

    private async Task RemoveAttachment(AssignedUserAttachmentItem item)
    {
        var dialog = await DialogService.ShowAsync<RemoveConfirmationDialog>(Resource.Remove);

        var dialogResult = await dialog.Result;

        if (!dialogResult.Canceled)
        {
            AttachmentsList.Remove(item);
            await OnAttachmentRemoved.InvokeAsync(item.Id);
        }
    }

    #endregion Private Methods
}