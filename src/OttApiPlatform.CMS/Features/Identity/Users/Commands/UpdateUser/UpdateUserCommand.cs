namespace OttApiPlatform.CMS.Features.Identity.Users.Commands.UpdateUser;

public class UpdateUserCommand
{
    #region Public Properties

    public string Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string AvatarUri { get; set; }
    public bool IsAvatarAdded { get; set; }
    public int NumberOfAttachments { get; set; } = 0;
    public string Name { get; set; }
    public string Surname { get; set; }
    public string JobTitle { get; set; }
    public string PhoneNumber { get; set; }
    public bool SetRandomPassword { get; set; }
    public bool MustSendActivationEmail { get; set; }
    public bool IsSuperAdmin { get; set; }
    public bool IsSuspended { get; set; }

    public IList<string> AssignedRoleIds { get; set; }
    public IList<string> AttachmentIds { get; set; }
    public IList<string> AttachmentUris { get; set; }

    #endregion Public Properties
}