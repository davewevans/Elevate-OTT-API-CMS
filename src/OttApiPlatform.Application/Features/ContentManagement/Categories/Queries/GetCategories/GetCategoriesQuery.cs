using OttApiPlatform.Application.Common.Contracts.UseCases.Content;

namespace OttApiPlatform.Application.Features.ContentManagement.Categories.Queries.GetCategories;

public class GetCategoriesQuery : FilterableQuery, IRequest<Envelope<CategoriesResponse>>
{
    #region Public Classes
    public class GetCategorysQueryHandler : IRequestHandler<GetCategoriesQuery, Envelope<CategoriesResponse>>
    {
        #region Private Fields

        private readonly ICategoryUseCase _categoryUseCase;

        #endregion Private Fields

        #region Public Constructors

        public GetCategorysQueryHandler(ICategoryUseCase categoryUseCase)
        {
            _categoryUseCase = categoryUseCase;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<CategoriesResponse>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await _categoryUseCase.GetCategories(request);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}
