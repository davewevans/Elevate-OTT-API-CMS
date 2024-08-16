namespace OttApiPlatform.Application.Features.Identity.Users.Queries.GetUsers;

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

    #region Public Methods

    public static UserItem MapFromEntity(ApplicationUser user)
    {
        return new()
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            Name = user.Name,
            FullName = user.FullName,
            Surname = user.Surname,
            JobTitle = user.JobTitle,
            AvatarUri = user.AvatarUri,
            IsSuperAdmin = user.IsSuperAdmin,
            IsSuspended = user.IsSuspended,
            IsStatic = user.IsStatic,
            EmailConfirmed = user.EmailConfirmed,
            CreatedOn = user.CreatedOn,
            CreatedBy = user.CreatedBy,
            ModifiedOn = user.ModifiedOn,
            ModifiedBy = user.ModifiedBy
        };
    }

    #endregion Public Methods
}