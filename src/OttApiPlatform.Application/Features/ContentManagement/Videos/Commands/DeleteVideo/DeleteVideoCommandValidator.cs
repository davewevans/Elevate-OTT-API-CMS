namespace OttApiPlatform.Application.Features.ContentManagement.Videos.Commands.DeleteVideo;

public class DeleteVideoCommandValidator : AbstractValidator<DeleteVideoCommand>
{
    #region Public Constructors

    public DeleteVideoCommandValidator()
    {
        RuleFor(v => v.Id).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(Resource.Invalid_author_Id);
    }

    #endregion Public Constructors
}
