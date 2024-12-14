using OttApiPlatform.Application.Features.ContentManagement.Assets.Queries.GetSasToken;

namespace OttApiPlatform.Application.Common.Contracts.Infrastructure.Storage;

public interface IAzureBlobStorageService : IFileStorageService
{
    SasTokenResponse GetSasTokenForVideoContainer(string fileName);
}
