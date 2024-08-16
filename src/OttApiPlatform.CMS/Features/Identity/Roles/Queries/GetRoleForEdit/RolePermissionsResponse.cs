namespace OttApiPlatform.CMS.Features.Identity.Roles.Queries.GetRoleForEdit;

public class RolePermissionsResponse
{
    #region Public Properties

    public bool IsDefault { get; set; }
    public string Name { get; set; }
    public IList<PermissionItem> RequestedPermissions { get; set; }
    public string RoleId { get; set; }
    public IList<PermissionItem> SelectedPermissions { get; set; }

    #endregion Public Properties
}