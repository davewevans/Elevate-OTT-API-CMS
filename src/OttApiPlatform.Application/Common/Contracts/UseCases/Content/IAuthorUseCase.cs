using OttApiPlatform.Application.Features.ContentManagement.Authors.Commands.CreateAuthor;
using OttApiPlatform.Application.Features.ContentManagement.Authors.Commands.DeleteAuthor;
using OttApiPlatform.Application.Features.ContentManagement.Authors.Commands.UpdateAuthor;
using OttApiPlatform.Application.Features.ContentManagement.Authors.Queries.ExportAuthors;
using OttApiPlatform.Application.Features.ContentManagement.Authors.Queries.GetAuthorForEdit;
using OttApiPlatform.Application.Features.ContentManagement.Authors.Queries.GetAuthors;

namespace OttApiPlatform.Application.Common.Contracts.UseCases.Content;
public interface IAuthorUseCase
{
    #region Public Methods

    Task<Envelope<AuthorForEdit>> GetAuthor(GetAuthorForEditQuery request);
    Task<Envelope<AuthorsResponse>> GetAuthors(GetAuthorsQuery request);
    Task<Envelope<CreateAuthorResponse>> AddAuthor(CreateAuthorCommand request);
    Task<Envelope<string>> EditAuthor(UpdateAuthorCommand request);
    Task<Envelope<string>> DeleteAuthor(DeleteAuthorCommand request);
    Task<Envelope<ExportAuthorsResponse>> ExportAsPdf(ExportAuthorsQuery request);
    
    #endregion Public Methods
}
