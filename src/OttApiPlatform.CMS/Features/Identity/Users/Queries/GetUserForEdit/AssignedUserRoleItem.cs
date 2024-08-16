namespace OttApiPlatform.CMS.Features.Identity.Users.Queries.GetUserForEdit;

public class AssignedUserRoleItem
{
    #region Public Properties

    public string Id { get; set; }
    public string Name { get; set; }
    public bool Checked { get; set; }
    public bool IsDefault { get; set; }
    public bool IsStatic { get; set; }

    #endregion Public Properties
}