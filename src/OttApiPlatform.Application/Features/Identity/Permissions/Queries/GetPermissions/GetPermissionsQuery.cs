namespace OttApiPlatform.Application.Features.Identity.Permissions.Queries.GetPermissions;

public class GetPermissionsQuery : IRequest<Envelope<PermissionsResponse>>
{
    #region Public Properties

    public Guid? Id { get; set; }
    public bool LoadingOnDemand { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class GetPermissionsQueryHandler : IRequestHandler<GetPermissionsQuery, Envelope<PermissionsResponse>>
    {
        #region Private Fields

        private readonly IPermissionService _permissionService;

        #endregion Private Fields

        #region Public Constructors

        public GetPermissionsQueryHandler(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<PermissionsResponse>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
        {
            if (request.LoadingOnDemand)
                return await _permissionService.GetPermissionsOnDemand(request);

            return await _permissionService.GetAllPermissions();
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}