using OttApiPlatform.Domain.Entities.Content;

namespace OttApiPlatform.Application.Common.Contracts.Repository
{
    public interface ISubtitleRepository
    {
        //Task<PagedList<SubtitleModel>> GetSubtitlesAsync(Guid tenantId, SubtitleParameters subtitleParameters, bool trackChanges);
        Task<SubtitleModel?> GetSubtitleAsync(Guid subtitleId, bool trackChanges);
        Task<SubtitleModel?> FindSubtitleByConditionAsync(Expression<Func<SubtitleModel, bool>> expression, bool trackChanges);
        void CreateSubtitleForTenant(Guid tenantId, SubtitleModel subtitle);
        Task<IEnumerable<SubtitleModel>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        void DeleteSubtitle(SubtitleModel subtitle);
        Task<bool> SubtitleExistsAsync(Expression<Func<SubtitleModel, bool>> expression);
    }
}
