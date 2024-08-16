namespace OttApiPlatform.Application.Features.Account.Manage.Commands.UpdateUserAvatar;

public class UpdateUserAvatarCommandValidator : AbstractValidator<UpdateUserAvatarCommand>
{
    #region Public Constructors

    public UpdateUserAvatarCommandValidator()
    {
        //RuleFor(v => v.AvatarUri).Cascade(CascadeMode.Stop)
        //    .NotEmpty().WithMessage(Resource.Profile_picture_is_required)
        //    .When(v => v.IsAvatarAdded == false);
    }

    #endregion Public Constructors
}