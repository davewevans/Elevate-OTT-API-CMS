namespace OttApiPlatform.CMS.Pages.Identity.Users;

public partial class AddUser
{
    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private IBreadcrumbService BreadcrumbService { get; set; }
    [Inject] private IUsersClient UsersClient { get; set; }
    [Inject] private IFileUploadClient FileUploadClient { get; set; }

    private string AvatarImageSrc { get; set; }
    private bool PasswordVisibility { get; set; }
    private string PasswordInputIcon { get; set; } = Icons.Material.Filled.VisibilityOff;
    private InputType PasswordInput { get; set; } = InputType.Password;
    private bool SendActivationEmailDisabled { get; }

    private CreateUserCommand CreateUserCommand { get; } = new();
    private BpFileUpload BpFileUploadReference { get; set; }
    private BpMultipleFileUpload BpMultipleFileUploadReference { get; set; }
    private Dictionary<StreamContent, string> AttachmentDictionary { get; } = new();
    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }

    #endregion Private Properties

    #region Protected Methods

    protected override void OnInitialized()
    {
        BreadcrumbService.SetBreadcrumbItems(new List<BreadcrumbItem>
        {
            new(Resource.Home, "/"),
            new(Resource.Identity, "Identity/users"),
            new(Resource.Users, "Identity/users"),
            new(Resource.Add_User, "#", true)
        });
    }

    #endregion Protected Methods

    #region Private Methods

    private void GetBase64StringImageUrl(string avatarImageSrc)
    {
        AvatarImageSrc = avatarImageSrc;
    }

    private void UpdateUserRoles(List<RoleItem> updatedUserRoles)
    {
        CreateUserCommand.AssignedRoleIds = updatedUserRoles.Select(ri => ri.Id).ToList();
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
                CreateUserCommand.IsAvatarAdded = true;
                CreateUserCommand.AvatarUri = responseWrapper.Payload.FileUri;
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
        CreateUserCommand.IsAvatarAdded = false;
        CreateUserCommand.AvatarUri = null;
    }

    private async Task AttachmentSelected(StreamContent streamContent)
    {
        using var fileFormData = new MultipartFormDataContent
                                 {
                                     { streamContent, "File", streamContent.Headers.GetValues("FileName").LastOrDefault() ?? throw new ArgumentNullException(nameof(streamContent)) },
                                     { new StringContent(BpMultipleFileUploadReference.GetFileRenameAllowed().ToString()), "FileRenameAllowed" }
                                 };
        try
        {
            var responseWrapper = await FileUploadClient.UploadFile(fileFormData);

            if (responseWrapper.IsSuccessStatusCode)
            {
                Snackbar.Add(Resource.File_has_been_uploaded_successfully, Severity.Success);
                CreateUserCommand.NumberOfAttachments++;
                AttachmentDictionary.Add(streamContent, responseWrapper.Payload.FileUri);
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

    private void AttachmentUnSelected(StreamContent streamContent)
    {
        CreateUserCommand.NumberOfAttachments--;
        AttachmentDictionary.Remove(streamContent);
    }

    private async Task SubmitForm()
    {
        CreateUserCommand.AttachmentUris = AttachmentDictionary.Values.ToList();

        var responseWrapper = await UsersClient.CreateUser(CreateUserCommand);

        if (responseWrapper.IsSuccessStatusCode)
        {
            Snackbar.Add(responseWrapper.Payload.SuccessMessage, Severity.Success);
            NavigationManager.NavigateTo("identity/users");
        }
        else
        {
            EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }

    private void TogglePasswordVisibility()
    {
        if (PasswordVisibility)
        {
            PasswordVisibility = false;
            PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
            PasswordInput = InputType.Password;
        }
        else
        {
            PasswordVisibility = true;
            PasswordInputIcon = Icons.Material.Filled.Visibility;
            PasswordInput = InputType.Text;
        }
    }

    #endregion Private Methods
}