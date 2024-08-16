namespace OttApiPlatform.Application.Features.Identity.Roles.Queries.GetRoleForEdit;

public class RoleForEditResponse : AuditableDto
{
    #region Public Properties

    public string Id { get; set; }
    public bool IsDefault { get; set; }
    public string Name { get; set; }

    #endregion Public Properties

    #region Public Methods

    public static RoleForEditResponse MapFromEntity(ApplicationRole role)
    {
        return new RoleForEditResponse
        {
            Id = role.Id,
            Name = role.Name,
            IsDefault = role.IsDefault,
            CreatedOn = role.CreatedOn,
            CreatedBy = role.CreatedBy,
            ModifiedOn = role.ModifiedOn,
            ModifiedBy = role.ModifiedBy
        };
    }

    #endregion Public Methods
}