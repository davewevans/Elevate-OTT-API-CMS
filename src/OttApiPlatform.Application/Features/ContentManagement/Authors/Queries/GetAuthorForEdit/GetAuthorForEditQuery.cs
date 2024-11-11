using OttApiPlatform.Application.Common.Contracts.UseCases.Content;

namespace OttApiPlatform.Application.Features.ContentManagement.Authors.Queries.GetAuthorForEdit;

public class GetAuthorForEditQuery : IRequest<Envelope<AuthorForEdit>>
{
    #region Public Properties

    public Guid? Id { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class GetAuthorForEditQueryHandler : IRequestHandler<GetAuthorForEditQuery, Envelope<AuthorForEdit>>
    {
        #region Private Fields

        private readonly IAuthorUseCase _authorUseCase;

        #endregion Private Fields

        #region Public Constructors

        public GetAuthorForEditQueryHandler(IAuthorUseCase authorUseCase)
        {
            _authorUseCase = authorUseCase;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<AuthorForEdit>> Handle(GetAuthorForEditQuery request, CancellationToken cancellationToken)
        {
            return await _authorUseCase.GetAuthor(request);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}
