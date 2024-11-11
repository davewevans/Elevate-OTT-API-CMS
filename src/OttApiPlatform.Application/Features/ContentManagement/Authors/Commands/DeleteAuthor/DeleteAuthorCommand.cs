using OttApiPlatform.Application.Common.Contracts.UseCases.Content;

namespace OttApiPlatform.Application.Features.ContentManagement.Authors.Commands.DeleteAuthor;

public class DeleteAuthorCommand : IRequest<Envelope<string>>
{
    #region Public Properties

    public Guid Id { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, Envelope<string>>
    {
        #region Private Fields

        private readonly IAuthorUseCase _authorUseCase;

        #endregion Private Fields

        #region Public Constructors

        public DeleteAuthorCommandHandler(IAuthorUseCase authorUseCase)
        {
            _authorUseCase = authorUseCase;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<string>> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            return await _authorUseCase.DeleteAuthor(request);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}
