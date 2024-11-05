namespace OttApiPlatform.CMS.Shared.FileUpload;

public partial class BpFileUpload
{
    #region Public Properties

    [Parameter] public bool ShowRenameFileDialog { get; set; }
    [Parameter] public bool ShowRemoveFileUploadComponentButton { get; set; }
    [Parameter] public bool ShowClearFileButton { get; set; } = true;
    [Parameter] public string AllowedExtensions { get; set; }
    [Parameter] public string ButtonId { get; set; }
    [Parameter] public string UploadFileButtonName { get; set; }
    [Parameter] public string UploadFileButtonIcon { get; set; }
    [Parameter] public long AllowedFileSize { get; set; } = 1073741824; // 1073741824 Bytes = 1 Gigabyte
    [Parameter] public EventCallback<string> OnImageSrcChanged { get; set; }
    [Parameter] public EventCallback<StreamContent> OnFileSelected { get; set; }
    [Parameter] public EventCallback<StreamContent> OnFileUnSelected { get; set; }

    #endregion Public Properties

    #region Private Properties

    private bool Hidden { get; set; }
    private bool FileUploadCancelled { get; set; }
    private bool CancelAllowed { get; set; }
    private bool ClearAllowed { get; set; }
    private bool FileInfoBoxHidden { get; set; } = true;
    private string ImageSrc { get; set; }
    private string NewFileName { get; set; }
    private string FileExtension { get; set; }
    private long MinProgressValue { get; set; }
    private long ProgressValue { get; set; }
    private FileUploadMetaData FileUploadMetaData { get; set; }
    private StreamContent StreamContent { get; set; }
    private IList<IBrowserFile> BrowserFiles { get; } = new List<IBrowserFile>();
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private IAppStateManager AppStateManager { get; set; }
    [Inject] private IDialogService DialogService { get; set; }

    #endregion Private Properties

    #region Public Methods

    public async Task ClearFile()
    {
        await Reinitialize();

        if (StreamContent != null)
            await OnFileUnSelected.InvokeAsync(StreamContent);

        ClearAllowed = false;
    }

    public async Task CancelFileUpload()
    {
        await Reinitialize();
    }

    public async Task HideFileUploadComponent()
    {
        Hidden = !Hidden;

        await Reinitialize();

        if (StreamContent != null)
            await OnFileUnSelected.InvokeAsync(StreamContent);
    }

    public async Task Reinitialize()
    {
        BrowserFiles.Clear();
        await ResetValues();
        await InvokeAsync(StateHasChanged);
    }

    public async Task ResetValues()
    {
        FileUploadMetaData = new FileUploadMetaData();
        MinProgressValue = 0;
        ProgressValue = 0;
        FileInfoBoxHidden = true;
        FileUploadCancelled = false;
        CancelAllowed = false;
        ClearAllowed = false;
        ImageSrc = null;
        await OnImageSrcChanged.InvokeAsync(ImageSrc);
        await InvokeAsync(StateHasChanged);
    }

    public async Task<bool> RenameFile(string safeFileName)
    {
        var dialogParameters = new DialogParameters { ["FileName"] = safeFileName };

        var dialog = await DialogService.ShowAsync<BpFileUploadDialog>(Resource.Please_provide_new_name_for_the_file_or_leave_it_as_is, dialogParameters);

        var dialogResult = await dialog.Result;

        if (dialogResult.Canceled)
            return false;

        if (string.IsNullOrWhiteSpace(dialogResult.Data.ToString()))
        {
            Snackbar.Add(Resource.Invalid_file_name, Severity.Error);
            FileUploadCancelled = true;
            return false;
        }

        safeFileName = Path.GetFileNameWithoutExtension(dialogResult.Data.ToString());

        if (safeFileName != null)
        {
            safeFileName = safeFileName.Replace(".", "_");

            NewFileName = safeFileName;
        }

        NewFileName += FileExtension;

        return true;
    }

    public bool GetFileRenameAllowed()
    {
        return ShowRenameFileDialog;
    }

    #endregion Public Methods

    #region Protected Methods

    protected override void OnInitialized()
    {
        AppStateManager.TokenSourceChanged += async (obj, nav) =>
        {
            await CancelFileUpload();
        };
        var random = new Random();
        ButtonId += random.Next(1111111, 9999999).ToString();
    }

    #endregion Protected Methods

    #region Private Methods

    private async Task ConvertToStream()
    {
        if (!FileUploadCancelled)
        {
            MinProgressValue = BrowserFiles[0].Size;

            CancelAllowed = true;
            ClearAllowed = false;

            await InvokeAsync(StateHasChanged);

            FileUploadMetaData.Name = NewFileName;

            FileUploadMetaData.Type = BrowserFiles[0].ContentType;

            FileUploadMetaData.Size = $"{BrowserFiles[0].Size} bytes";

            StreamContent = new StreamContent(BrowserFiles[0].OpenReadStream(AllowedFileSize));

            StreamContent.Headers.Add("FileName", NewFileName);

            await OnFileSelected.InvokeAsync(StreamContent);

            ProgressValue = MinProgressValue;

            FileInfoBoxHidden = false;

            ClearAllowed = true;

            CancelAllowed = false;

            ClearAllowed = true;

            await InvokeAsync(StateHasChanged);
        }
        else
        {
            await Reinitialize();
        }
    }

    private async Task UploadFiles(InputFileChangeEventArgs fileChangeEventArgs)
    {
        BrowserFiles.Clear();

        foreach (var file in fileChangeEventArgs.GetMultipleFiles())
            BrowserFiles.Add(file);

        if (await ValidateFile())
        {
            await HandleFileSelection();

            await GetBase64StringImageUrl(fileChangeEventArgs);
        }
    }

    private async Task<string> GetBase64StringImageUrl(InputFileChangeEventArgs eventArgs)
    {
        var imageFile = eventArgs.GetMultipleFiles(999999999).FirstOrDefault();

        if (imageFile != null && !imageFile.ContentType.Contains("image"))
            return string.Empty;

        if (imageFile != null)
        {
            var format = imageFile.ContentType;

            var resizedImageFile = await imageFile.RequestImageFileAsync(format, 300, 300);

            var buffer = new byte[resizedImageFile.Size];

            await resizedImageFile.OpenReadStream().ReadAsync(buffer).ConfigureAwait(false);

            ImageSrc = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
        }

        await OnImageSrcChanged.InvokeAsync(ImageSrc);

        return ImageSrc;
    }

    private async Task HandleFileSelection()
    {
        await ResetValues();

        FileInfoBoxHidden = true;

        try
        {
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(BrowserFiles[0].Name);

            FileExtension = Path.GetExtension(BrowserFiles[0].Name);

            var safeFileName = fileNameWithoutExtension.Replace(".", "_");

            NewFileName = safeFileName; //Initialize File Name.

            if (ShowRenameFileDialog)
            {
                if (await RenameFile(safeFileName))
                    await ConvertToStream();
                else
                    await Reinitialize();
            }
            else
            {
                NewFileName += FileExtension;

                await ConvertToStream();
            }
        }
        catch (OperationCanceledException)
        {
            await InvokeAsync(StateHasChanged);

            await Reinitialize();
        }
    }

    private async Task<bool> ValidateFile()
    {
        var success = true;
        if (AllowedFileSize != 0 && BrowserFiles[0].Size > AllowedFileSize)
        {
            Snackbar.Add(Resource.The_selected_file_exceeds_the_allowed_maximum_size_limit, Severity.Error);
            success = false;
        }

        var fileExtension = $".{BrowserFiles[0].Name.Split(".").Last()}";
        if (!IsAllowFileExtension(fileExtension))
        {
            Snackbar.Add(Resource.The_selected_file_extension_is_not_allowed, Severity.Error);
            success = false;
        }

        if (BrowserFiles[0].Size == 0)
        {
            Snackbar.Add(Resource.Unable_to_upload_an_empty_file, Severity.Error);
            success = false;
        }

        if (!success)
            await Reinitialize();

        return success;
    }

    private bool IsAllowFileExtension(string fileExtension)
    {
        if (!string.IsNullOrWhiteSpace(AllowedExtensions))
            return !string.IsNullOrWhiteSpace(fileExtension) && AllowedExtensions.ToUpper().Split(",").ToArray().Contains(fileExtension.ToUpper());

        return true; //Which means allowed for all extensions.
    }

    #endregion Private Methods
}