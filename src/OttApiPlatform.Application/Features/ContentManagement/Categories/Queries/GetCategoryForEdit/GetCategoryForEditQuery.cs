using OttApiPlatform.Application.Common.Contracts.UseCases.Content;

namespace OttApiPlatform.Application.Features.ContentManagement.Categories.Queries.GetCategoryForEdit;

public class GetCategoryForEditQuery : IRequest<Envelope<CategoryForEdit>>
{
    #region Public Properties

    public Guid? Id { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class GetCategoryForEditQueryHandler : IRequestHandler<GetCategoryForEditQuery, Envelope<CategoryForEdit>>
    {
        #region Private Fields

        private readonly ICategoryUseCase _categoryUseCase;

        #endregion Private Fields

        #region Public Constructors

        public GetCategoryForEditQueryHandler(ICategoryUseCase categoryUseCase)
        {
            _categoryUseCase = categoryUseCase;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<CategoryForEdit>> Handle(GetCategoryForEditQuery request, CancellationToken cancellationToken)
        {
            return await _categoryUseCase.GetCategory(request);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}
