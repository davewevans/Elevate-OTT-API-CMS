using OttApiPlatform.Application.Common.Contracts.UseCases.Identity;

namespace OttApiPlatform.Application.Features.ContentManagement.Videos.Queries.GetNewStorageName;
public class GetNewStorageNameQuery : IRequest<Envelope<NewStorageNameResponse>>
{
    #region Public Classes

    public class GetNewStorageNameQueryHandler : IRequestHandler<GetNewStorageNameQuery, Envelope<NewStorageNameResponse>>
    {
        #region Private Fields

        private readonly ITenantResolver _tenantResolver;

        public GetNewStorageNameQueryHandler(ITenantResolver tenantResolver)
        {
            _tenantResolver = tenantResolver;
        }

        #endregion Private Fields

        #region Public Methods

        public async Task<Envelope<NewStorageNameResponse>> Handle(GetNewStorageNameQuery request, CancellationToken cancellationToken)
        {
            var tenantId = _tenantResolver.GetTenantId();
            if (tenantId == null)
                return Envelope<NewStorageNameResponse>.Result.BadRequest("Tenant ID is null");

            var storageNamePrefix = tenantId.Value.GetStorageFileNamePrefix();
            var storageName = $"{storageNamePrefix}/{ Guid.NewGuid().ToString().Replace("-", "") }";
            var response = new NewStorageNameResponse { Name = storageName };

            return Envelope<NewStorageNameResponse>.Result.Ok(response);
        }

        #endregion Public Methods
    }

    #endregion Public Classes   
}
