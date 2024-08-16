namespace OttApiPlatform.Application.Features.Tenants.Queries.GetTenantForEdit;

public class TenantForEditResponse : AuditableDto
{
    #region Public Properties

    public string Id { get; set; }
    public string Name { get; set; }

    #endregion Public Properties

    #region Public Methods

    public static TenantForEditResponse MapFromEntity(Tenant tenant)
    {
        return new TenantForEditResponse
        {
            Id = tenant.Id.ToString(),
            Name = tenant.Name
        };
    }

    #endregion Public Methods
}