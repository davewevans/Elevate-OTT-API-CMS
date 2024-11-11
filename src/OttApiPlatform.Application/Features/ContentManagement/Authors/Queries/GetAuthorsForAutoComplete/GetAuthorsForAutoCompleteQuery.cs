using OttApiPlatform.Application.Common.Contracts.UseCases.Content;
using OttApiPlatform.Application.Features.ContentManagement.Authors.Queries.GetAuthors;

namespace OttApiPlatform.Application.Features.ContentManagement.Authors.Queries.GetAuthorsForAutoComplete;

public class GetAuthorsForAutoCompleteQuery : FilterableQuery, IRequest<Envelope<AuthorsForAutoCompleteResponse>>
{
    #region Public Classes
    public class GetAuthorsForAutoCompleteQueryHandler : IRequestHandler<GetAuthorsForAutoCompleteQuery, Envelope<AuthorsForAutoCompleteResponse>>
    {
        #region Private Fields

        private readonly IAuthorUseCase _authorUseCase;

        #endregion Private Fields

        #region Public Constructors

        public GetAuthorsForAutoCompleteQueryHandler(IAuthorUseCase authorUseCase)
        {
            _authorUseCase = authorUseCase;
        }

        #endregion Public Constructors

        #region Public Methods
        
        public async Task<Envelope<AuthorsForAutoCompleteResponse>> Handle(GetAuthorsForAutoCompleteQuery request, CancellationToken cancellationToken)
        {
            var response = await _authorUseCase.GetAuthors(new GetAuthorsQuery
            {
                RowsPerPage = request.RowsPerPage,
                SearchText = request.SearchText
            });

            if (response?.Payload?.Authors?.Items is null)
            {
                return Envelope<AuthorsForAutoCompleteResponse>.Result.NotFound();
            }

            var responseForAutoComplete = new AuthorsForAutoCompleteResponse
            {
                Authors = new PagedList<AuthorItemForAutoComplete>()
            };

            foreach (var author in response.Payload.Authors.Items)
            {
                //responseForAutoComplete.Authors.Items.Add(new AuthorItemForAutoComplete
                //{
                //    Id = author.Id,
                //    Name = author.Name,
                //    ImageUrl = author.ImageUrl ?? string.Empty
                //});
            }

            return Envelope<AuthorsForAutoCompleteResponse>.Result.Ok(responseForAutoComplete);
        }

        #endregion Public Methods

    }

    #endregion Public Classes
}
