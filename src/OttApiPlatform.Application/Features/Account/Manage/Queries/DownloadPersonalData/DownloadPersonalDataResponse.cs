namespace OttApiPlatform.Application.Features.Account.Manage.Queries.DownloadPersonalData;

public class DownloadPersonalDataResponse
{
    #region Public Properties

    public byte[] PersonalData { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }

    #endregion Public Properties
}