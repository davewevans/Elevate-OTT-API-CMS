namespace OttApiPlatform.CMS.Features.Identity.Users.Commands.GrantOrRevokeUserPermissions;

public class GrantOrRevokeUserPermissionsCommand
{
    #region Public Properties

    public string UserId { get; set; }
    public IList<Guid> SelectedPermissionIds { get; set; }

    #endregion Public Properties
}