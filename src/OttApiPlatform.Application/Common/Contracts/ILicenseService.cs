namespace OttApiPlatform.Application.Common.Contracts;
public interface ILicenseService
{
    string GenerateLicenseForTenant(Guid tenantId);
}
