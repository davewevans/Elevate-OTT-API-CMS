namespace OttApiPlatform.Application.Features.Tenants.Commands.CreateTenant;

public class CreateTenantCommandValidator : AbstractValidator<CreateTenantCommand>
{
    #region Public Constructors

    public CreateTenantCommandValidator()
    {
        //RuleFor(v => v.Name).Cascade(CascadeMode.Stop)
        //                    .NotEmpty()
        //                    .WithMessage(Resource.Tenant_name_is_required);
    }

    #endregion Public Constructors
}