namespace OttApiPlatform.Application.Features.Identity.Roles.Commands.CreateRole;

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    #region Public Constructors

    public CreateRoleCommandValidator()
    {
        RuleFor(v => v.Name).Cascade(CascadeMode.Stop)
                            .NotEmpty()
                            .WithMessage(Resource.Role_name_is_required);
    }

    #endregion Public Constructors
}