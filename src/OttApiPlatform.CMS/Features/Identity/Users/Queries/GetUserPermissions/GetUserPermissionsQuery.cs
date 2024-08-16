namespace OttApiPlatform.CMS.Features.Identity.Users.Queries.GetUserPermissions;

public class GetUserPermissionsQuery
{
    #region Public Properties

    public string UserId { get; set; }
    public bool LoadingOnDemand { get; set; }

    #endregion Public Properties
}