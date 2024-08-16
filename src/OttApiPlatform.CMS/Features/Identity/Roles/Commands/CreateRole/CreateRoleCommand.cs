namespace OttApiPlatform.CMS.Features.Identity.Roles.Commands.CreateRole;

public class CreateRoleCommand
{
    #region Public Properties

    public string Name { get; set; }
    public bool IsDefault { get; set; }
    public IList<Guid> SelectedPermissionIds { get; set; }

    #endregion Public Properties
}