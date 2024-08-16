namespace OttApiPlatform.CMS.Shared.FileUpload;

public partial class BpMultipleFileUpload
{
    #region Public Properties

    [Parameter] public string AllowedExtensions { get; set; }
    [Parameter] public long MaxFileSize { get; set; }
    [Parameter] public bool ShowRenameFileDialog { get; set; }
    [Parameter] public EventCallback<StreamContent> OnFileSelected { get; set; }
    [Parameter] public EventCallback<StreamContent> OnFileUnSelected { get; set; }
    [Parameter] public string UploadFileButtonName { get; set; }
    [Parameter] public string UploadFileButtonIcon { get; set; }

    #endregion Public Properties

    #region Private Properties

    private int CurrentCount { get; set; }
    private List<int> List { get; set; }

    #endregion Private Properties

    #region Public Methods

    public bool GetFileRenameAllowed()
    {
        return ShowRenameFileDialog;
    }

    #endregion Public Methods

    #region Protected Methods

    protected override void OnInitialized()
    {
        List = new List<int>();
    }

    protected void AddBpUploadFileComponent()
    {
        List.Add(CurrentCount++);
    }

    #endregion Protected Methods
}