namespace OttApiPlatform.Application.Features.Tenants.Commands.CreateTenant;
public class CreateTenantResult
{
    public bool IsSuccess { get; set; }
    public List<string> Errors { get; set; } = new();
}

