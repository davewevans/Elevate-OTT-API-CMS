namespace OttApiPlatform.Application.Features.MyTenant.Queries.GetMyTenant;

public class MyTenantForEditResponse : AuditableDto
{
    #region Public Properties

    public string Id { get; set; }
    public string Name { get; set; }

    #endregion Public Properties

    #region Public Methods

    public static MyTenantForEditResponse MapFromEntity(Tenant myTenant)
    {
        return new MyTenantForEditResponse
        {
            Id = myTenant.Id.ToString(),
            Name = myTenant.Name
        };
    }

    #endregion Public Methods
}