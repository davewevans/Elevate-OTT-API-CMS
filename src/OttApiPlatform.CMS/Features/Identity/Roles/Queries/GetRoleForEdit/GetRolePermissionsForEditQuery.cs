namespace OttApiPlatform.CMS.Features.Identity.Roles.Queries.GetRoleForEdit;

public class GetRolePermissionsForEditQuery
{
    #region Public Properties

    public string RoleId { get; set; }
    public bool LoadingOnDemand { get; set; }
    public Guid? PermissionId { get; set; }

    #endregion Public Properties
}