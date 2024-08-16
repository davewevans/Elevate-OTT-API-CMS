namespace OttApiPlatform.CMS.Features.Tenants.Queries.GetTenants;

public class TenantItem : AuditableDto
{
    #region Public Properties

    public string Id { get; set; }
    public string Name { get; set; }

    #endregion Public Properties
}