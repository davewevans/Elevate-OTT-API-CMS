using OttApiPlatform.Application.Features.ContentManagement.Authors.Queries.GetAuthors;
using OttApiPlatform.Domain.Entities.Content;

namespace OttApiPlatform.Application.Common.Contracts.Repository
{
    public interface IAuthorRepository
    {
        Task<AuthorModel?> GetAuthorAsync(Guid tenantId, Guid authorId, bool trackChanges);
        IQueryable<AuthorModel>? GetAuthors(Guid tenantId, GetAuthorsQuery request, bool trackChanges);
        Task<AuthorModel?> FindAuthorByConditionAsync(Expression<Func<AuthorModel, bool>> expression, bool trackChanges);
        void CreateAuthor(AuthorModel author);
        Task<IEnumerable<AuthorModel>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        void DeleteAuthor(AuthorModel author);
        Task<bool> AuthorExistsAsync(Expression<Func<AuthorModel, bool>> expression);
    }
}
