namespace OttApiPlatform.CMS.Features.Tenants.Commands.UpdateTenant;

public class TenantForEditValidator : AbstractValidator<TenantForEdit>
{
    #region Public Constructors

    public TenantForEditValidator()
    {
        RuleFor(v => v.Name).Cascade(CascadeMode.Stop)
                            .NotEmpty()
                            .WithMessage(Resource.Tenant_name_is_required);
    }

    #endregion Public Constructors
}