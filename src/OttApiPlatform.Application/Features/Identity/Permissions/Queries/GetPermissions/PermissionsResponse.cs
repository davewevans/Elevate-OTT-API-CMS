namespace OttApiPlatform.Application.Features.Identity.Permissions.Queries.GetPermissions;

public class PermissionsResponse
{
    #region Public Constructors

    public PermissionsResponse()
    {
        Permissions = new List<PermissionItem>();
    }

    #endregion Public Constructors

    #region Public Properties

    public List<PermissionItem> Permissions { get; set; }

    #endregion Public Properties
}