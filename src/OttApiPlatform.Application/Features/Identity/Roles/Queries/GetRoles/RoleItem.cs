namespace OttApiPlatform.Application.Features.Identity.Roles.Queries.GetRoles;

public class RoleItem : AuditableDto
{
    #region Public Properties

    public string Id { get; set; }
    public string Name { get; set; }
    public bool Checked { get; set; }
    public bool IsDefault { get; set; }
    public bool IsStatic { get; set; }

    #endregion Public Properties

    #region Public Methods

    public static RoleItem MapFromEntity(ApplicationRole applicationRole)
    {
        return new()
        {
            Id = applicationRole.Id,
            Name = applicationRole.Name,
            IsDefault = applicationRole.IsDefault,
            IsStatic = applicationRole.IsStatic,
            CreatedOn = applicationRole.CreatedOn,
            CreatedBy = applicationRole.CreatedBy,
            ModifiedOn = applicationRole.ModifiedOn,
            ModifiedBy = applicationRole.ModifiedBy
        };
    }

    #endregion Public Methods
}