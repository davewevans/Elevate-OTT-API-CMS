using OttApiPlatform.Application.Common.Contracts.UseCases.Content;

namespace OttApiPlatform.Application.Features.ContentManagement.Authors.Commands.CreateAuthor;

public class CreateAuthorCommand : IRequest<Envelope<CreateAuthorResponse>>
{
    #region Public Properties

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
    public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, Envelope<CreateAuthorResponse>>
    {
        #region Private Fields

        private readonly IAuthorUseCase _authorUseCase;

        #endregion Private Fields

        #region Public Constructors
        public CreateAuthorCommandHandler(IAuthorUseCase authorUseCase)
        {
            _authorUseCase = authorUseCase;
        }
        #endregion Public Constructors

        #region Public Methods
        public async Task<Envelope<CreateAuthorResponse>> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
        {
            return await _authorUseCase.AddAuthor(request);
        }
        #endregion Public Methods
    }
    #endregion Public Classes
}
