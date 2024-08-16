namespace OttApiPlatform.Application.Features.Identity.Users.Queries.GetUserPermissions;

public class UserPermissionsResponse
{
    #region Public Constructors

    public UserPermissionsResponse()
    {
        RequestedPermissions = new List<PermissionItem>();
        SelectedPermissions = new List<PermissionItem>();
    }

    #endregion Public Constructors

    #region Public Properties

    public IReadOnlyList<PermissionItem> SelectedPermissions { get; set; }
    public IReadOnlyList<PermissionItem> RequestedPermissions { get; set; }

    #endregion Public Properties
}