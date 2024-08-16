namespace OttApiPlatform.CMS.Pages.Identity.Users;

public partial class EditUser
{
    #region Public Properties

    [Parameter] public string UserId { get; set; }

    #endregion Public Properties

    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private IBreadcrumbService BreadcrumbService { get; set; }
    [Inject] private IUsersClient UsersClient { get; set; }
    [Inject] private IFileUploadClient FileUploadClient { get; set; }

    //private string PasswordInputIcon { get; set; } = Icons.Material.Filled.VisibilityOff;
    //private bool PasswordVisibility { get; set; }
    //private InputType PasswordInput { get; set; } = InputType.Password;
    private bool SendActivationEmailDisabled { get; set; }

    private string AvatarImageSrc { get; set; }
    private string CurrentEmail { get; set; }
    private List<RoleItem> AssignedUserRoles { get; set; } = new();
    private List<AssignedUserAttachmentItem> UserAttachmentList { get; set; } = new();
    private UserForEdit UserForEditVm { get; set; } = new();
    private UpdateUserCommand UpdateUserCommand { get; set; } = new();
    private BpFileUpload BpFileUploadReference { get; set; }
    private BpMultipleFileUpload BpMultipleFileUploadReference { get; set; }
    private Dictionary<StreamContent, string> AttachmentDictionary { get; } = new();
    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }

    #endregion Private Properties

    #region Protected Methods

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        BreadcrumbService.SetBreadcrumbItems(new List<BreadcrumbItem>
        {
            new(Resource.Home, "/"),
            new(Resource.Identity, "identity/users"),
            new(Resource.Users, "identity/users"),
            new(Resource.Edit_User, "#", true)
        });

        var responseWrapper = await UsersClient.GetUser(new GetUserForEditQuery { Id = UserId });

        if (responseWrapper.IsSuccessStatusCode)
        {
            UserForEditVm = responseWrapper.Payload;

            AssignedUserRoles = UserForEditVm.AssignedRoles.Select(ri => new RoleItem
            {
                Id = ri.Id,
                Name = ri.Name,
                IsDefault = ri.IsDefault,
                IsStatic = ri.IsStatic,
            }).ToList();

            UserAttachmentList = UserForEditVm.AssignedAttachments;

            if (!string.IsNullOrWhiteSpace(UserForEditVm.AvatarUri))
                UserForEditVm.IsAvatarAdded = true;

            if (UserForEditVm.AssignedAttachments != null)
                UserForEditVm.NumberOfAttachments = UserForEditVm.AssignedAttachments.Count;

            CurrentEmail = UserForEditVm.Email;
        }
        else
        {
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }

    #endregion Protected Methods

    #region Private Methods

    private void CanSendActivationEmail(string newEmail)
    {
        if (newEmail == CurrentEmail)
        {
            UserForEditVm.Email = CurrentEmail;
            UserForEditVm.MustSendActivationEmail = false;
            SendActivationEmailDisabled = true;
        }
        else
        {
            UserForEditVm.Email = newEmail;
            UserForEditVm.MustSendActivationEmail = true;
            SendActivationEmailDisabled = false;
        }
    }

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
                UserForEditVm.IsAvatarAdded = true;
                UserForEditVm.AvatarUri = responseWrapper.Payload.FileUri;
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
        UserForEditVm.IsAvatarAdded = false;
        UserForEditVm.AvatarUri = null;
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
                UserForEditVm.NumberOfAttachments++;
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
        UserForEditVm.NumberOfAttachments--;
        AttachmentDictionary.Remove(streamContent);
    }

    private void RemoveFromUserCurrentAttachments(Guid removedUserAttachmentId)
    {
        UserAttachmentList.RemoveAll(ua => ua.Id == removedUserAttachmentId);
        UserForEditVm.NumberOfAttachments--;
    }

    private void UpdateUserRoles(List<RoleItem> updatedUserRoles)
    {
        AssignedUserRoles = updatedUserRoles.Select(ur => new RoleItem
        {
            Id = ur.Id,
            Name = ur.Name,
            IsDefault = ur.IsDefault,
            IsStatic = ur.IsStatic,
            CreatedOn = ur.CreatedOn,
            CreatedBy = ur.CreatedBy,
            ModifiedOn = ur.ModifiedOn,
            ModifiedBy = ur.ModifiedBy
        }).ToList();
    }

    private async Task SubmitForm()
    {
        var dialog = await DialogService.ShowAsync<SaveConfirmationDialog>(Resource.Confirm);

        var dialogResult = await dialog.Result;

        if (!dialogResult.Canceled)
        {
            UpdateUserCommand = new UpdateUserCommand
            {
                Id = UserForEditVm.Id,
                Email = UserForEditVm.Email,
                Name = UserForEditVm.Name,
                Surname = UserForEditVm.Surname,
                JobTitle = UserForEditVm.JobTitle,
                PhoneNumber = UserForEditVm.PhoneNumber,
                Password = UserForEditVm.Password,
                ConfirmPassword = UserForEditVm.ConfirmPassword,
                AvatarUri = UserForEditVm.AvatarUri,
                IsAvatarAdded = UserForEditVm.IsAvatarAdded,
                IsSuperAdmin = UserForEditVm.IsSuperAdmin,
                IsSuspended = UserForEditVm.IsSuspended,
                MustSendActivationEmail = UserForEditVm.MustSendActivationEmail,
                SetRandomPassword = UserForEditVm.SetRandomPassword,
                NumberOfAttachments = UserForEditVm.NumberOfAttachments,
                AssignedRoleIds = AssignedUserRoles.Select(ar => ar.Id).ToList(),
                AttachmentIds = UserAttachmentList.Select(ai => ai.Id.ToString()).ToList(),
                AttachmentUris = AttachmentDictionary.Values.ToList()
            };

            var responseWrapper = await UsersClient.UpdateUser(UpdateUserCommand);

            if (responseWrapper.IsSuccessStatusCode)
            {
                Snackbar.Add(responseWrapper.Payload, Severity.Success);
                NavigationManager.NavigateTo("identity/users");
            }
            else
            {
                EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
                SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
            }
        }
    }

    #endregion Private Methods

    //private void TogglePasswordVisibility()
    //{
    //    if (PasswordVisibility)
    //    {
    //        PasswordVisibility = false;
    //        PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
    //        PasswordInput = InputType.Password;
    //    }
    //    else
    //    {
    //        PasswordVisibility = true;
    //        PasswordInputIcon = Icons.Material.Filled.Visibility;
    //        PasswordInput = InputType.Text;
    //    }
    //}
}