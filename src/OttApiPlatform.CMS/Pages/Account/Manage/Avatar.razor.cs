namespace OttApiPlatform.CMS.Pages.Account.Manage;

public partial class Avatar
{
    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private IManageClient ManageClient { get; set; }
    [Inject] private IFileUploadClient FileUploadClient { get; set; }

    private string AvatarImageSrc { get; set; }
    private BpFileUpload BpFileUploadReference { get; set; }
    private CancellationTokenSource FileUploadCancellationTokenSource { get; set; } = new();
    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
    private UpdateUserAvatarCommand UpdateUserAvatarCommand { get; set; } = new();
    private UserAvatarForEdit UserAvatarForEditVm { get; set; } = new();

    #endregion Private Properties

    #region Protected Methods

    protected override async Task OnInitializedAsync()
    {
        var responseWrapper = await ManageClient.GetUserAvatar();

        if (responseWrapper.IsSuccessStatusCode)
        {
            UserAvatarForEditVm = responseWrapper.Payload;
            if (!string.IsNullOrWhiteSpace(UserAvatarForEditVm.AvatarUri))
                UserAvatarForEditVm.IsAvatarAdded = true;
        }
        else
        {
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }

    #endregion Protected Methods

    #region Private Methods

    private void GetBase64StringImageUrl(string avatarImageSrc)
    {
        AvatarImageSrc = avatarImageSrc;
    }

    private async Task AvatarSelected(StreamContent streamContent)
    {
        using var fileFormData = new MultipartFormDataContent
        {
            { streamContent, "File", streamContent.Headers.GetValues("FileName").LastOrDefault() ?? throw new ArgumentNullException(nameof(streamContent)) },
            { new StringContent(BpFileUploadReference.GetFileRenameAllowed().ToString()), "FileRenameAllowed" }
        };

        try
        {
            var responseWrapper = await FileUploadClient.UploadFile(fileFormData);

            if (responseWrapper.IsSuccessStatusCode)
            {
                Snackbar.Add(Resource.File_has_been_uploaded_successfully, Severity.Success);
                UserAvatarForEditVm.IsAvatarAdded = true;
                UserAvatarForEditVm.AvatarUri = responseWrapper.Payload.FileUri;
            }
            else
            {
                EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
                SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
            }
        }
        catch (OperationCanceledException)
        {
            Snackbar.Add(Resource.File_upload_was_cancelled, Severity.Error);
        }
    }

    private void AvatarUnSelected(StreamContent streamContent)
    {
        UserAvatarForEditVm.IsAvatarAdded = false;
        UserAvatarForEditVm.AvatarUri = null;
    }

    private void FileUploadCancelled(CancellationTokenSource cancellationTokenSource)
    {
        FileUploadCancellationTokenSource = cancellationTokenSource;
    }

    private async Task UpdateUserAvatar()
    {
        var dialog = await DialogService.ShowAsync<SaveConfirmationDialog>(Resource.Confirm);

        var dialogResult = await dialog.Result;

        if (!dialogResult.Canceled)
        {
            UpdateUserAvatarCommand = new UpdateUserAvatarCommand
            {
                AvatarUri = UserAvatarForEditVm.AvatarUri,
                IsAvatarAdded = UserAvatarForEditVm.IsAvatarAdded,
            };

            var responseWrapper = await ManageClient.UpdateUserAvatar(UpdateUserAvatarCommand);

            if (responseWrapper.IsSuccessStatusCode)
            {
                Snackbar.Add(responseWrapper.Payload, Severity.Success);
            }
            else
            {
                EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
                SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
            }
        }
    }

    #endregion Private Methods
}