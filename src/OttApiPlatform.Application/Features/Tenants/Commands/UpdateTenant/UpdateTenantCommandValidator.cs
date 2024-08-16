namespace OttApiPlatform.Application.Features.Tenants.Commands.UpdateTenant;

public class UpdateTenantCommandValidator : AbstractValidator<UpdateTenantCommand>
{
    #region Public Constructors

    public UpdateTenantCommandValidator()
    {
        RuleFor(v => v.Name).Cascade(CascadeMode.Stop)
                            .NotEmpty()
                            .WithMessage(Resource.Tenant_name_is_required);
    }

    #endregion Public Constructors
}