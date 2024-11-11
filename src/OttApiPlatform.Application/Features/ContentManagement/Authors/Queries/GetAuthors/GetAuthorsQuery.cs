using OttApiPlatform.Application.Common.Contracts.UseCases.Content;

namespace OttApiPlatform.Application.Features.ContentManagement.Authors.Queries.GetAuthors;

public class GetAuthorsQuery : FilterableQuery, IRequest<Envelope<AuthorsResponse>>
{
    #region Public Classes
    public class GetAuthorsQueryHandler : IRequestHandler<GetAuthorsQuery, Envelope<AuthorsResponse>>
    {
        #region Private Fields

        private readonly IAuthorUseCase _authorUseCase;

        #endregion Private Fields

        #region Public Constructors

        public GetAuthorsQueryHandler(IAuthorUseCase authorUseCase)
        {
            _authorUseCase = authorUseCase;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<AuthorsResponse>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
        {
            return await _authorUseCase.GetAuthors(request);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}
