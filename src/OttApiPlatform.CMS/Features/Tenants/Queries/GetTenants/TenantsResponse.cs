namespace OttApiPlatform.CMS.Features.Tenants.Queries.GetTenants;

public class TenantsResponse
{
    #region Public Properties

    public PagedList<TenantItem> Tenants { get; set; }

    #endregion Public Properties
}