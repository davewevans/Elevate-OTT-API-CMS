namespace OttApiPlatform.CMS.Models.Videos;

public class UploadFileModel
{
    public UploadProgressModel UploadProgress { get; set; }
    public IBrowserFile BrowserFile { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string StorageName { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public long MaxSizeAllowed { get; set; } = 53687091200; // 50GB default
    public string ContentType { get; set; } = string.Empty;
    public bool UploadComplete { get; set; }
    public string UploadProgressPercent
    {
        get
        {
            return UploadProgress is not null ? $"{UploadProgress.Value}%" : "0%";
        }
    }
}
