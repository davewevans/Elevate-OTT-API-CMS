using AutoMapper;
using OttApiPlatform.Application.Common.Contracts.Repository;
using OttApiPlatform.Application.Common.Contracts.UseCases.Content;
using OttApiPlatform.Application.Features.ContentManagement.Authors.Commands.CreateAuthor;
using OttApiPlatform.Application.Features.ContentManagement.Authors.Commands.DeleteAuthor;
using OttApiPlatform.Application.Features.ContentManagement.Authors.Commands.UpdateAuthor;
using OttApiPlatform.Application.Features.ContentManagement.Authors.Queries.ExportAuthors;
using OttApiPlatform.Application.Features.ContentManagement.Authors.Queries.GetAuthorForEdit;
using OttApiPlatform.Application.Features.ContentManagement.Authors.Queries.GetAuthors;
using OttApiPlatform.Domain.Entities.Content;

namespace OttApiPlatform.Application.UseCases.Content;
public class AuthorUseCase : IAuthorUseCase
{
    #region Private Fields

    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    public readonly IRepositoryManager _repositoryManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITenantResolver _tenantResolver;
    private readonly IStorageProvider _storageProvider;
    private readonly IConfigReaderService _configReaderService;

    #endregion Private Fields

    #region Public Constructors

    public AuthorUseCase(IApplicationDbContext dbContext,
                            IHttpContextAccessor httpContextAccessor,
                            IMapper mapper, 
                            IRepositoryManager repositoryManager,
                            ITenantResolver tenantResolver, 
                            IStorageProvider storageProvider, 
                            IConfigReaderService configReaderService)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _repositoryManager = repositoryManager;
        _tenantResolver = tenantResolver;
        _storageProvider = storageProvider;
        _configReaderService = configReaderService;
    }

    #endregion Public Constructors

    #region Public Methods

    public async Task<Envelope<AuthorForEdit>> GetAuthor(GetAuthorForEditQuery request)
    {
        if (request.Id is null)
        {
            return Envelope<AuthorForEdit>.Result.BadRequest(Resource.Invalid_author_Id);
        }

        var tenantId = _tenantResolver.GetTenantId();

        if (tenantId is null)
        {
            return Envelope<AuthorForEdit>.Result.BadRequest(Resource.Invalid_tenant_Id);
        }

        var author = await _repositoryManager.Author.GetAuthorAsync(tenantId.Value, request.Id.Value, false);

        if (author == null)
            return Envelope<AuthorForEdit>.Result.NotFound(Resource.Unable_to_load_author);

        var authorForEdit = _mapper.Map<AuthorForEdit>(author);

        return Envelope<AuthorForEdit>.Result.Ok(authorForEdit);
    }

    public async Task<Envelope<AuthorsResponse>> GetAuthors(GetAuthorsQuery request)
    {
        var tenantId = _tenantResolver.GetTenantId();

        if (tenantId is null)
        {
            return Envelope<AuthorsResponse>.Result.BadRequest(Resource.Invalid_tenant_Id);
        }

        var query = _repositoryManager.Author.GetAuthors(tenantId.Value, request, false);
        
        var authorItems = query is not null
            ? await query.Select(author => _mapper.Map<AuthorItem>(author))
                .ToPagedListAsync(request.PageNumber, request.RowsPerPage)
            : null;

        var authorsResponse = new AuthorsResponse
        {
            Authors = authorItems
        };

        return Envelope<AuthorsResponse>.Result.Ok(authorsResponse);
    }

    public async Task<Envelope<CreateAuthorResponse>> AddAuthor(CreateAuthorCommand request)
    {
        var tenantId = _tenantResolver.GetTenantId();

        if (tenantId is null)
        {
            return Envelope<CreateAuthorResponse>.Result.BadRequest(Resource.Invalid_tenant_Id);
        }
    
        string fileNamePrefix = tenantId.Value.GetStorageFileNamePrefix();

        var author = _mapper.Map<AuthorModel>(request);

        if (request.IsImageAdded && request.ImageFile is not null)
        {
            //string imageUrl = await StoreImageAsync(request.ImageFile, fileNamePrefix);
            //author.ImageUrl = imageUrl;
        }

        _repositoryManager.Author.CreateAuthor(author);
        await _repositoryManager.SaveAsync();

        var createAuthorResponse = new CreateAuthorResponse
        {
            Id = author.Id,
            SuccessMessage = Resource.Author_has_been_created_successfully
        };

        return Envelope<CreateAuthorResponse>.Result.Ok(createAuthorResponse);
    }

    public async Task<Envelope<string>> EditAuthor(UpdateAuthorCommand request)
    {
        var tenantId = _tenantResolver.GetTenantId();

        if (tenantId is null)
        {
            return Envelope<string>.Result.BadRequest(Resource.Invalid_tenant_Id);
        }

        // TODO uncomment
        string fileNamePrefix = tenantId.Value.GetStorageFileNamePrefix();

        var authorEntity = await _repositoryManager.Author.GetAuthorAsync(tenantId.Value, request.Id, true);

        if (authorEntity == null)
            return Envelope<string>.Result.NotFound(Resource.Unable_to_load_author);

        _mapper.Map(request, authorEntity);

        if (request.IsImageAdded && request.ImageFile is not null)
        {
            await UpdateAuthorWithImageAsync(authorEntity, request.ImageFile, fileNamePrefix);
        }

        await _repositoryManager.SaveAsync();

        return Envelope<string>.Result.Ok(Resource.Author_has_been_updated_successfully);
    }

    public async Task<Envelope<string>> DeleteAuthor(DeleteAuthorCommand request)
    {
        var tenantId = _tenantResolver.GetTenantId();

        if (tenantId is null)
        {
            return Envelope<string>.Result.BadRequest(Resource.Invalid_tenant_Id);
        }

        var authorEntity = await _repositoryManager.Author.GetAuthorAsync(tenantId.Value, request.Id, true);
        await _repositoryManager.SaveAsync();

        if (authorEntity == null)
            return Envelope<string>.Result.NotFound(Resource.The_author_is_not_found);

        _repositoryManager.Author.DeleteAuthor(authorEntity);
        await _repositoryManager.SaveAsync();

        return Envelope<string>.Result.Ok(Resource.Author_has_been_deleted_successfully);
    }

    public Task<Envelope<ExportAuthorsResponse>> ExportAsPdf(ExportAuthorsQuery request)
    {
        throw new NotImplementedException();
    }

    #endregion Public Methods

    #region Private Methods

    private async Task UpdateAuthorWithImageAsync(AuthorModel author, IFormFile? image, string fileNamePrefix)
    {
        var storageService = _storageProvider.InvokeInstanceForAzureStorage();

        switch (storageService.GetFileState(image, author.ImageUrl))
        {
            case FileStatus.Unchanged:
                break;

            case FileStatus.Modified:
                //author.ImageUrl = await UpdateImageAsync(image, fileNamePrefix, author.ImageUrl ?? string.Empty, storageService);
                break;

            case FileStatus.Deleted:
                var blobOptions = _configReaderService.GetBlobOptions();
                await storageService.DeleteFileIfExists(author.ImageUrl ?? string.Empty, blobOptions.ImageBlobContainerName);
                author.ImageUrl = null;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    //private async Task<string> StoreImageAsync(IFormFile? image, string fileNamePrefix)
    //{
    //    var blobOptions = _configReaderService.GetBlobOptions();
    //    var storageService = _storageProvider.InvokeInstanceForAzureStorage();

    //    if (image == null) return string.Empty;

    //    var imageUri = await storageService.UploadFile(image, blobOptions.ImageBlobContainerName, fileNamePrefix);

    //    return imageUri;
    //}

    //private async Task<string> UpdateImageAsync(IFormFile? image, string fileNamePrefix, string oldFileUri, IFileStorageService storageService)
    //{
    //    var blobOptions = _configReaderService.GetBlobOptions();

    //    if (image == null) return string.Empty;

    //    var newImageUri = await storageService.EditFile(image, blobOptions.ImageBlobContainerName, fileNamePrefix, oldFileUri);

    //    return newImageUri;
    //}
    #endregion Private Methods
}
