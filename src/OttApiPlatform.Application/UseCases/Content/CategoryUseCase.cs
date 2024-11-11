using AutoMapper;
using OttApiPlatform.Application.Common.Contracts.Reports;
using OttApiPlatform.Application.Common.Contracts.Repository;
using OttApiPlatform.Application.Common.Contracts.UseCases.Content;
using OttApiPlatform.Application.Features.ContentManagement.Categories.Commands.CreateCategory;
using OttApiPlatform.Application.Features.ContentManagement.Categories.Commands.DeleteCategory;
using OttApiPlatform.Application.Features.ContentManagement.Categories.Commands.UpdateCategory;
using OttApiPlatform.Application.Features.ContentManagement.Categories.Queries.GetCategories;
using OttApiPlatform.Application.Features.ContentManagement.Categories.Queries.GetCategoryForEdit;
using OttApiPlatform.Domain.Entities.Content;

namespace OttApiPlatform.Application.UseCases.Content;

public class CategoryUseCase : ICategoryUseCase
{

    // TODO create resources for error messages

    #region Private Fields

    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    public readonly IRepositoryManager _repositoryManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IReportingService _reportingService;
    private readonly ITenantResolver _tenantResolver;
    private readonly IStorageProvider _storageProvider;
    private readonly IConfigReaderService _configReaderService;

    #endregion Private Fields

    #region Public Constructors

    public CategoryUseCase(IApplicationDbContext dbContext,
                            IHttpContextAccessor httpContextAccessor,
                            IReportingService reportingService,
                            IMapper mapper,
                            IRepositoryManager repositoryManager,
                            ITenantResolver tenantResolver,
                            IStorageProvider storageProvider,
                            IConfigReaderService configReaderService)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
        _reportingService = reportingService;
        _mapper = mapper;
        _repositoryManager = repositoryManager;
        _tenantResolver = tenantResolver;
        _storageProvider = storageProvider;
        _configReaderService = configReaderService;
    }

    #endregion Public Constructors

    #region Public Methods

    public async Task<Envelope<CategoryForEdit>> GetCategory(GetCategoryForEditQuery request)
    {
        if (request.Id is null)
        {
            return Envelope<CategoryForEdit>.Result.BadRequest("Invalid category id");
        }

        var tenantId = _tenantResolver.GetTenantId();

        if (tenantId is null)
        {
            return Envelope<CategoryForEdit>.Result.BadRequest(Resource.Invalid_tenant_Id);
        }

        var category = await _repositoryManager.Category.GetCategoryAsync(tenantId.Value, request.Id.Value, false);

        if (category == null)
            return Envelope<CategoryForEdit>.Result.NotFound("Unable to load category");

        var categoryForEdit = _mapper.Map<CategoryForEdit>(category);

        return Envelope<CategoryForEdit>.Result.Ok(categoryForEdit);
    }

    public async Task<Envelope<CategoriesResponse>> GetCategories(GetCategoriesQuery request)
    {
        var tenantId = _tenantResolver.GetTenantId();

        if (tenantId is null)
        {
            return Envelope<CategoriesResponse>.Result.BadRequest(Resource.Invalid_tenant_Id);
        }

        var query = _repositoryManager.Category.GetCategories(tenantId.Value, request, false);

        var categoryItems = query is not null
            ? await query.Select(category => _mapper.Map<CategoryItem>(category))
                .ToPagedListAsync(request.PageNumber, request.RowsPerPage)
            : null;

        var categorysResponse = new CategoriesResponse
        {
            Categories = categoryItems
        };

        return Envelope<CategoriesResponse>.Result.Ok(categorysResponse);
    }

    public async Task<Envelope<CreateCategoryResponse>> AddCategory(CreateCategoryCommand request)
    {
        var tenantId = _tenantResolver.GetTenantId();

        if (tenantId is null)
        {
            return Envelope<CreateCategoryResponse>.Result.BadRequest(Resource.Invalid_tenant_Id);
        }

        string fileNamePrefix = tenantId.Value.GetStorageFileNamePrefix();

        var category = _mapper.Map<CategoryModel>(request);

        if (request.IsImageAdded && request.ImageFile is not null)
        {
            string imageUrl = await StoreImageAsync(request.ImageFile, fileNamePrefix);
            category.ImageUrl = imageUrl;
        }

        _repositoryManager.Category.CreateCategory(category);
        await _repositoryManager.SaveAsync();

        var createCategoryResponse = new CreateCategoryResponse
        {
            Id = category.Id,
          
            SuccessMessage = "Category has been created successfully."
        };

        return Envelope<CreateCategoryResponse>.Result.Ok(createCategoryResponse);
    }

    public async Task<Envelope<string>> EditCategory(UpdateCategoryCommand request)
    {
        var tenantId = _tenantResolver.GetTenantId();

        if (tenantId is null)
        {
            return Envelope<string>.Result.BadRequest(Resource.Invalid_tenant_Id);
        }
        // TODO uncomment
        string fileNamePrefix = tenantId.Value.GetStorageFileNamePrefix();

        var categoryEntity = await _repositoryManager.Category.GetCategoryAsync(tenantId.Value, request.Id, true);

        if (categoryEntity == null)
            return Envelope<string>.Result.NotFound("Unable_to_load_category");

        _mapper.Map(request, categoryEntity);

        if (request.IsImageAdded && request.ImageFile is not null)
        {
            await UpdateCategoryWithImageAsync(categoryEntity, request.ImageFile, fileNamePrefix);
        }

        await _repositoryManager.SaveAsync();

        return Envelope<string>.Result.Ok("Category_has_been_updated_successfully");
    }

    public async Task<Envelope<string>> DeleteCategory(DeleteCategoryCommand request)
    {
        var tenantId = _tenantResolver.GetTenantId();

        if (tenantId is null)
        {
            return Envelope<string>.Result.BadRequest(Resource.Invalid_tenant_Id);
        }

        var categoryEntity = await _repositoryManager.Category.GetCategoryAsync(tenantId.Value, request.Id, true);
        await _repositoryManager.SaveAsync();

        if (categoryEntity == null)
            return Envelope<string>.Result.NotFound("The_category_is_not_found");

        _repositoryManager.Category.DeleteCategory(categoryEntity);
        await _repositoryManager.SaveAsync();

        return Envelope<string>.Result.Ok("Category_has_been_deleted_successfully");
    }

    #endregion Public Methods

    #region Private Methods
    private async Task UpdateCategoryWithImageAsync(CategoryModel category, IFormFile? image, string fileNamePrefix)
    {
        var storageService = _storageProvider.InvokeInstanceForAzureStorage();

        switch (storageService.GetFileState(image, category.ImageUrl))
        {
            case FileStatus.Unchanged:
                break;

            case FileStatus.Modified:
                category.ImageUrl = await UpdateImageAsync(image, fileNamePrefix, category.ImageUrl ?? string.Empty, storageService);
                break;

            case FileStatus.Deleted:
                var blobOptions = _configReaderService.GetBlobOptions();
                await storageService.DeleteFileIfExists(category.ImageUrl ?? string.Empty, blobOptions.ImageBlobContainerName);
                category.ImageUrl = null;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private async Task<string> StoreImageAsync(IFormFile? image, string fileNamePrefix)
    {
        var blobOptions = _configReaderService.GetBlobOptions();
        var storageService = _storageProvider.InvokeInstanceForAzureStorage();

        if (image == null) return string.Empty;

        //var imageUri = await storageService.UploadFile(image, blobOptions.ImageBlobContainerName, fileNamePrefix);

        //return imageUri.FileUri;

        return "";
    }

    private async Task<string> UpdateImageAsync(IFormFile? image, string fileNamePrefix, string oldFileUri, IFileStorageService storageService)
    {
        var blobOptions = _configReaderService.GetBlobOptions();

        if (image == null) return string.Empty;

        var newImageUri = await storageService.EditFile(image, blobOptions.ImageBlobContainerName, fileNamePrefix, oldFileUri);

        return newImageUri.FileUri;
    }
    #endregion Private Methods
}
