namespace OttApiPlatform.CMS.Features.Identity.Users.Commands.CreateUser;

public class CreateUserCommand
{
    #region Public Properties

    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }

    public string AvatarUri { get; set; }

    public string Name { get; set; }
    public string Surname { get; set; }
    public string JobTitle { get; set; }
    public string PhoneNumber { get; set; }
    public int NumberOfAttachments { get; set; }
    public bool IsAvatarAdded { get; set; }
    public bool SetRandomPassword { get; set; }
    public bool MustSendActivationEmail { get; set; }
    public bool IsSuperAdmin { get; set; }
    public bool IsSuspended { get; set; }
    public bool IsStatic { get; set; }
    public IList<string> AssignedRoleIds { get; set; }
    public IList<string> AttachmentUris { get; set; }

    #endregion Public Properties
}