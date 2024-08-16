namespace OttApiPlatform.CMS.Features.Identity.Manage.Queries.GetUser;

public class UserAttachmentItem
{
    #region Public Properties

    public Guid Id { get; set; }
    public string UserId { get; set; }
    public string FileName { get; set; }
    public string FileUri { get; set; }

    #endregion Public Properties
}