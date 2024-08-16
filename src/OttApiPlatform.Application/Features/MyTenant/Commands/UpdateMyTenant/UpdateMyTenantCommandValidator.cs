namespace OttApiPlatform.Application.Features.MyTenant.Commands.UpdateMyTenant;

public class UpdateMyTenantCommandValidator : AbstractValidator<UpdateMyTenantCommand>
{
    #region Public Constructors

    public UpdateMyTenantCommandValidator()
    {
        RuleFor(v => v.Name).Cascade(CascadeMode.Stop)
                            .NotEmpty()
                            .WithMessage(Resource.Tenant_name_is_required);
    }

    #endregion Public Constructors
}