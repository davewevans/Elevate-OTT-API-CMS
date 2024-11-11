using OttApiPlatform.Application.Common.Contracts.UseCases.Content;

namespace OttApiPlatform.Application.Features.ContentManagement.Authors.Queries.ExportAuthors;

public class ExportAuthorsQuery : IRequest<Envelope<ExportAuthorsResponse>>
{
    #region Public Classes

    public string SearchText { get; set; }
    public string SortBy { get; set; }

    public class CreateAuthorQueryHandler : IRequestHandler<ExportAuthorsQuery, Envelope<ExportAuthorsResponse>>
    {
        #region Private Fields

        private readonly IAuthorUseCase _authorUseCase;

        #endregion Private Fields

        #region Public Constructors

        public CreateAuthorQueryHandler(IAuthorUseCase authorUseCase)
        {
            _authorUseCase = authorUseCase;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<ExportAuthorsResponse>> Handle(ExportAuthorsQuery request, CancellationToken cancellationToken)
        {
            return await _authorUseCase.ExportAsPdf(request);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}
