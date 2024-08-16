namespace OttApiPlatform.Application.Features.Identity.Roles.Commands.UpdateRole;

public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    #region Public Constructors

    public UpdateRoleCommandValidator()
    {
        RuleFor(v => v.Id).Cascade(CascadeMode.Stop)
                          .NotEmpty()
                          .WithMessage(Resource.Invalid_role_Id);

        RuleFor(r => r.Name).Cascade(CascadeMode.Stop)
                            .NotEmpty()
                            .WithMessage(Resource.Role_name_is_required);
    }

    #endregion Public Constructors
}