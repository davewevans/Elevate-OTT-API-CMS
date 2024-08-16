namespace OttApiPlatform.CMS.Models;

public class FileUploadMetaData
{
    #region Public Constructors

    public FileUploadMetaData()
    {
        Content = new MultipartFormDataContent();
    }

    #endregion Public Constructors

    #region Public Properties

    public string Name { get; set; }
    public string Type { get; set; }
    public string Size { get; set; }
    public MultipartFormDataContent Content { get; set; }

    #endregion Public Properties
}