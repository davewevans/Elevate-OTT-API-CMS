namespace OttApiPlatform.Application.Features.Tenants.Commands.CreateTenant;

public class CreateTenantResponse
{
    #region Public Properties

    public Guid Id { get; internal set; }
    public string SuccessMessage { get; internal set; }

    #endregion Public Properties
}