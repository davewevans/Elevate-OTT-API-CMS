namespace OttApiPlatform.Application.Features.Account.Manage.Commands.UpdateUserProfile;

public class UpdateUserProfileCommandValidator : AbstractValidator<UpdateUserProfileCommand>
{
    #region Public Constructors

    public UpdateUserProfileCommandValidator()
    {
        RuleFor(v => v.Name).Cascade(CascadeMode.Stop)
                            .NotEmpty()
                            .WithMessage(Resource.First_name_is_required);

        RuleFor(v => v.Surname).Cascade(CascadeMode.Stop)
                               .NotEmpty()
                               .WithMessage(Resource.Surname_is_required);

        RuleFor(v => v.JobTitle).Cascade(CascadeMode.Stop)
                                .NotEmpty()
                                .WithMessage(Resource.Job_title_is_required);
    }

    #endregion Public Constructors
}