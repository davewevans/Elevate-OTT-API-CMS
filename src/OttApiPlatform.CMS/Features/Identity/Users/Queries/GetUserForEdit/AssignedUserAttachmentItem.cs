namespace OttApiPlatform.CMS.Features.Identity.Users.Queries.GetUserForEdit;

public class AssignedUserAttachmentItem
{
    #region Public Properties

    public Guid Id { get; set; }
    public string UserId { get; set; }
    public string FileName { get; set; }
    public string FileUri { get; set; }

    #endregion Public Properties
}