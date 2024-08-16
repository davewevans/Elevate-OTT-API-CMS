namespace OttApiPlatform.CMS.Features.Identity.Users.Queries.GetUsers;

public class UserItem : AuditableDto
{
    #region Public Properties

    public string Id { get; set; }

    public string Name { get; set; }
    public string Surname { get; set; }
    public string FullName { get; set; }
    public string JobTitle { get; set; }
    public string AvatarUri { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public bool EmailConfirmed { get; set; }
    public bool IsSuperAdmin { get; set; }
    public bool IsSuspended { get; set; }
    public bool IsStatic { get; set; }

    #endregion Public Properties
}