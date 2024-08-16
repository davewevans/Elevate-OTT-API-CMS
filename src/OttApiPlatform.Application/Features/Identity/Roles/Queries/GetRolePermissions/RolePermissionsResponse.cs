namespace OttApiPlatform.Application.Features.Identity.Roles.Queries.GetRolePermissions;

public class RolePermissionsResponse : AuditableDto
{
    #region Public Constructors

    public RolePermissionsResponse()
    {
        SelectedPermissions = new List<PermissionItem>();
        RequestedPermissions = new List<PermissionItem>();
    }

    #endregion Public Constructors

    #region Public Properties

    public string RoleId { get; set; }
    public IReadOnlyList<PermissionItem> RequestedPermissions { get; set; }
    public IReadOnlyList<PermissionItem> SelectedPermissions { get; set; }

    #endregion Public Properties

    #region Public Methods

    public static RolePermissionsResponse MapFromEntity(ApplicationRole role, List<PermissionItem> selectedPermissions, List<PermissionItem> requestedPermissions)
    {
        return new()
        {
            RoleId = role.Id,
            SelectedPermissions = selectedPermissions,
            RequestedPermissions = requestedPermissions,
        };
    }

    #endregion Public Methods
}