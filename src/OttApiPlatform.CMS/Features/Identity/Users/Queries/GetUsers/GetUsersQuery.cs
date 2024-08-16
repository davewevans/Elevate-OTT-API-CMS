namespace OttApiPlatform.CMS.Features.Identity.Users.Queries.GetUsers;

public class GetUsersQuery : FilterableQuery
{
    #region Public Properties

    public IList<string> SelectedRoleIds { get; set; }

    #endregion Public Properties
}