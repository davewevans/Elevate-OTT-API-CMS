using OttApiPlatform.Domain.Entities.Products;

namespace OttApiPlatform.Application.Common.Contracts.Repository
{
    public interface IProductFamilyRepository
    {
        //Task<PagedList<ProductFamilyModel>> GetProductFamiliesAsync(Guid? tenantId, ProductFamilyParameters productFamilyParameters, bool trackChanges);
        Task<ProductFamilyModel?> GetProductFamilyAsync(Guid productFamilyId, bool trackChanges);
        Task<ProductFamilyModel?> FindProductFamilyByConditionAsync(Expression<Func<ProductFamilyModel, bool>> expression, bool trackChanges);
        //void CreateProductFamily(Guid? tenantId, ProductFamilyModel productFamily);
        Task<IEnumerable<ProductFamilyModel>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        void DeleteProductFamily(ProductFamilyModel productFamily);
        Task<bool> ProductFamilyExistsAsync(Expression<Func<ProductFamilyModel, bool>> expression);
    }
}
