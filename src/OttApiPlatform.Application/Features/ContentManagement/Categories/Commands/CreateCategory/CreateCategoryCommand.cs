using OttApiPlatform.Application.Common.Contracts.UseCases.Content;

namespace OttApiPlatform.Application.Features.ContentManagement.Categories.Commands.CreateCategory;

public class CreateCategoryCommand : IRequest<Envelope<CreateCategoryResponse>>
{
    #region Public Properties

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
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Envelope<CreateCategoryResponse>>
    {
        #region Private Fields

        private readonly ICategoryUseCase _categoryUseCase;

        #endregion Private Fields

        #region Public Constructors
        public CreateCategoryCommandHandler(ICategoryUseCase categoryUseCase)
        {
            _categoryUseCase = categoryUseCase;
        }
        #endregion Public Constructors

        #region Public Methods
        public async Task<Envelope<CreateCategoryResponse>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            return await _categoryUseCase.AddCategory(request);
        }
        #endregion Public Methods
    }
    #endregion Public Classes
}
