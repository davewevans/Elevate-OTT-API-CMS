namespace OttApiPlatform.Application.Features.Identity.Users.Commands.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    #region Public Constructors

    public UpdateUserCommandValidator()
    {
        RuleFor(v => v.Id).Cascade(CascadeMode.Stop)
                          .NotEmpty()
                          .WithMessage(Resource.Invalid_user_Id);

        RuleFor(v => v.Name).Cascade(CascadeMode.Stop)
                            .NotEmpty()
                            .WithMessage(Resource.First_name_is_required);

        //RuleFor(v => v.AvatarUri).Cascade(CascadeMode.Stop)
        //    .NotEmpty().WithMessage(Resource.Profile_picture_is_required)
        //    .When(v => v.IsAvatarAdded == false);

        //RuleFor(v => v.NumberOfAttachments).Cascade(CascadeMode.Stop)
        //    .LessThanOrEqualTo(3).WithMessage(Resource.Maximum_allowed_number_of_attachments_must_not_exceed_3)
        //    .GreaterThanOrEqualTo(1).WithMessage(Resource.Minimum_allowed_number_of_attachments_must_be_at_least_1);

        RuleFor(v => v.Email).Cascade(CascadeMode.Stop)
                             .EmailAddress()
                             .WithMessage(v => string.Format(Resource.Username_is_invalid, v.Email))
                             .MaximumLength(100)
                             .WithMessage(Resource.Username_must_not_exceed_200_characters)
                             .MinimumLength(6)
                             .WithMessage(Resource.Username_must_be_at_least_6_characters)
                             .When(x => !string.IsNullOrEmpty(x.Email));

        //RuleFor(v => v.Password).Cascade(CascadeMode.Stop)
        //    .NotEmpty().WithMessage(Resource.Password_is_required)
        //    .MaximumLength(100).WithMessage(Resource.Password_must_not_exceed_200_characters)
        //    .MinimumLength(6).WithMessage(Resource.Password_must_be_at_least_6_characters)
        //    .When(v => v.SetRandomPassword == false && !string.IsNullOrWhiteSpace(v.Password));

        //RuleFor(v => v.ConfirmPassword).Cascade(CascadeMode.Stop)
        //    .NotEmpty().WithMessage(Resource.Confirm_password_is_required)
        //    .Equal(v => v.Password).WithMessage(Resource.The_password_and_confirmation_password_do_not_match)
        //    .When(v => v.SetRandomPassword == false && string.IsNullOrWhiteSpace(v.Password));
    }

    #endregion Public Constructors
}