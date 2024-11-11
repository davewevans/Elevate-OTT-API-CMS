namespace OttApiPlatform.Application.Features.ContentManagement.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    #region Public Constructors

    public DeleteCategoryCommandValidator()
    {
        RuleFor(v => v.Id).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Invalid Category Id");
    }

    #endregion Public Constructors
}
