using OttApiPlatform.Application.Common.Contracts.UseCases.Content;

namespace OttApiPlatform.Application.Features.ContentManagement.Authors.Commands.UpdateAuthor;

public class UpdateAuthorCommand : IRequest<Envelope<string>>
{

    #region Public Properties

    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Bio { get; set; } 
    public string? ImageUrl { get; set; }
    public string? SeoTitle { get; set; } 
    public string? SeoDescription { get; set; } 
    public string? Slug { get; set; } 
    public IFormFile? ImageFile { get; set; }
    public bool IsImageAdded { get; set; }

    #endregion Public Properties


    #region Public Classes

    public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, Envelope<string>>
    {
        #region Private Fields

        private readonly IAuthorUseCase _authorUseCase;

        #endregion Private Fields

        #region Public Constructors

        public UpdateAuthorCommandHandler(IAuthorUseCase authorUseCase)
        {
            _authorUseCase = authorUseCase;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<string>> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            return await _authorUseCase.EditAuthor(request);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}
