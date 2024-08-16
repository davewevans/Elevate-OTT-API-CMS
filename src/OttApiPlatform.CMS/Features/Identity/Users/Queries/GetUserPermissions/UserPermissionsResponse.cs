namespace OttApiPlatform.CMS.Features.Identity.Users.Queries.GetUserPermissions;

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

    public IList<PermissionItem> SelectedPermissions { get; set; }
    public IList<PermissionItem> RequestedPermissions { get; set; }

    #endregion Public Properties
}