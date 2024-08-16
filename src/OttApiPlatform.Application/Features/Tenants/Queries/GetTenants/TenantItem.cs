namespace OttApiPlatform.Application.Features.Tenants.Queries.GetTenants;

public class TenantItem : AuditableDto
{
    #region Public Properties

    public string Id { get; set; }
    public string Name { get; set; }
    public string Subdomain { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidUntil { get; set; }

    #endregion Public Properties

    #region Public Methods

    public static TenantItem MapFromEntity(Tenant tenant)
    {
        return new()
        {
            Id = tenant.Id.ToString(),
            Name = tenant.Name,
        };
    }

    #endregion Public Methods
}