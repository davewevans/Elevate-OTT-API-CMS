using OttApiPlatform.Application.Features.Tenants.Commands.CreateTenant;

namespace OttApiPlatform.Application.Common.Contracts.UseCases.Identity;

public interface ITenantUseCase
{
    #region Public Methods

    Task<Envelope<CreateTenantResponse>> AddTenant(CreateTenantCommand request);

    string GetTenantStorageNamePrefix();

    Task AddTenantStorageNamePrefixIfNotExists();

    #endregion Public Methods
}
