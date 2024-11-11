using OttApiPlatform.Application.Common.Contracts.UseCases.Content;

namespace OttApiPlatform.Application.Features.ContentManagement.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommand : IRequest<Envelope<string>>
{
    #region Public Properties

    public Guid Id { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Envelope<string>>
    {
        #region Private Fields

        private readonly ICategoryUseCase _categoryUseCase;

        #endregion Private Fields

        #region Public Constructors

        public DeleteCategoryCommandHandler(ICategoryUseCase categoryUseCase)
        {
            _categoryUseCase = categoryUseCase;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<string>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            return await _categoryUseCase.DeleteCategory(request);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}
