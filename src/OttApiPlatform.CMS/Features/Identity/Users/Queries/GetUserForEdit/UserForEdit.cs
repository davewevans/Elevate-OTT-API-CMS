namespace OttApiPlatform.CMS.Features.Identity.Users.Queries.GetUserForEdit;

public class UserForEdit
{
    #region Public Constructors

    public UserForEdit()
    {
        AssignedRoles = new List<AssignedUserRoleItem>();
        AssignedAttachments = new List<AssignedUserAttachmentItem>();
    }

    #endregion Public Constructors

    #region Public Properties

    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string JobTitle { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string AvatarUri { get; set; }
    public bool IsAvatarAdded { get; set; }
    public int NumberOfAttachments { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string PhoneNumber { get; set; }
    public bool SetRandomPassword { get; set; }
    public bool MustSendActivationEmail { get; set; }
    public bool IsSuperAdmin { get; set; }
    public bool IsSuspended { get; set; }
    public bool IsStatic { get; set; }
    public bool EmailConfirmed { get; set; }
    public DateTime CreatedOn { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public string ModifiedBy { get; set; }
    public List<AssignedUserRoleItem> AssignedRoles { get; set; }
    public List<AssignedUserAttachmentItem> AssignedAttachments { get; set; }

    #endregion Public Properties
}