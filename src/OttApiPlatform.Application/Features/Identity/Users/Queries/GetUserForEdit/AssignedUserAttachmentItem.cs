namespace OttApiPlatform.Application.Features.Identity.Users.Queries.GetUserForEdit;

public class AssignedUserAttachmentItem
{
    #region Public Properties

    public Guid Id { get; set; }
    public string UserId { get; set; }
    public string FileName { get; set; }
    public string FileUri { get; set; }

    #endregion Public Properties

    #region Public Methods

    public static AssignedUserAttachmentItem MapFromEntity(ApplicationUserAttachment userAttachment)
    {
        return new()
        {
            Id = userAttachment.Id,
            UserId = userAttachment.UserId,
            FileName = userAttachment.FileName,
            FileUri = userAttachment.FileUri
        };
    }

    #endregion Public Methods
}