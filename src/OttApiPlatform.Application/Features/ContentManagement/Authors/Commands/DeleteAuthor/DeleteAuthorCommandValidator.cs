namespace OttApiPlatform.Application.Features.ContentManagement.Authors.Commands.DeleteAuthor;

public class DeleteAuthorCommandValidator : AbstractValidator<DeleteAuthorCommand>
{
    #region Public Constructors

    public DeleteAuthorCommandValidator()
    {
        RuleFor(v => v.Id).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(Resource.Invalid_author_Id);
    }

    #endregion Public Constructors
}
