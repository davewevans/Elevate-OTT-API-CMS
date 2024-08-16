namespace OttApiPlatform.CMS.Features.Identity.Roles.Commands.UpdateRole;

public class UpdateRoleCommand
{
    #region Public Properties

    public string Id { get; set; }
    public string Name { get; set; }
    public bool IsDefault { get; set; }
    public IList<Guid> SelectedPermissionIds { get; set; }

    #endregion Public Properties
}