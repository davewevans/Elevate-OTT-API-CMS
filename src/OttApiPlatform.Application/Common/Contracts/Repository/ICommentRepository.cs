using OttApiPlatform.Domain.Entities.Content;

namespace OttApiPlatform.Application.Common.Contracts.Repository
{
    public interface ICommentRepository
    {
        //Task<PagedList<CommentModel>> GetCommentsAsync(Guid tenantId, CommentParameters commentParameters, bool trackChanges);
        Task<CommentModel?> GetCommentAsync(Guid commentId, bool trackChanges);
        Task<CommentModel?> FindCommentByConditionAsync(Expression<Func<CommentModel, bool>> expression, bool trackChanges);
        void CreateCommentForTenant(Guid tenantId, CommentModel comment);
        Task<IEnumerable<CommentModel>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        void DeleteComment(CommentModel comment);
        Task<bool> CommentExistsAsync(Expression<Func<CommentModel, bool>> expression);
    }
}
