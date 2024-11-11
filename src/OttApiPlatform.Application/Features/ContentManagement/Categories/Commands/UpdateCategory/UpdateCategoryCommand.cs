using OttApiPlatform.Application.Common.Contracts.UseCases.Content;

namespace OttApiPlatform.Application.Features.ContentManagement.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommand : IRequest<Envelope<string>>
{
    #region Public Properties

    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int Position { get; set; }

    public string? ImageUrl { get; set; }

    public string? SeoTitle { get; set; }

    public string? SeoDescription { get; set; }

    public string? Slug { get; set; }

    public bool IsImageAdded { get; set; }

    public IFormFile? ImageFile { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Envelope<string>>
    {
        #region Private Fields

        private readonly ICategoryUseCase _categoryUseCase;

        #endregion Private Fields

        #region Public Constructors

        public UpdateCategoryCommandHandler(ICategoryUseCase categoryUseCase)
        {
            _categoryUseCase = categoryUseCase;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<string>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            return await _categoryUseCase.EditCategory(request);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}
