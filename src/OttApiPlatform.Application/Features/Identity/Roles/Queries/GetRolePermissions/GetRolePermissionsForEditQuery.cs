namespace OttApiPlatform.Application.Features.Identity.Roles.Queries.GetRolePermissions;

public class GetRolePermissionsForEditQuery : IRequest<Envelope<RolePermissionsResponse>>
{
    #region Public Properties

    public string RoleId { get; set; }
    public Guid? PermissionId { get; set; }
    public bool LoadingOnDemand { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class GetRolePermissionsForEditQueryHandler : IRequestHandler<GetRolePermissionsForEditQuery, Envelope<RolePermissionsResponse>>
    {
        #region Private Fields

        private readonly IApplicationDbContext _dbContext;
        private readonly IPermissionService _permissionService;

        #endregion Private Fields

        #region Public Constructors

        public GetRolePermissionsForEditQueryHandler(IApplicationDbContext dbContext, IPermissionService permissionService)
        {
            _dbContext = dbContext;
            _permissionService = permissionService;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<RolePermissionsResponse>> Handle(GetRolePermissionsForEditQuery request, CancellationToken cancellationToken)
        {
            // Checks if the RoleId provided in the request is valid.
            if (string.IsNullOrWhiteSpace(request.RoleId))
                return Envelope<RolePermissionsResponse>.Result.BadRequest(Resource.Invalid_role_Id);

            // Retrieves the role from the database.
            var role = await _dbContext.Roles.Include(r => r.RoleClaims)
                                             .Where(r => r.Id == request.RoleId)
                                             .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // Checks if the role was found.
            if (role == null)
                return Envelope<RolePermissionsResponse>.Result.NotFound(Resource.Unable_to_load_role);

            // Retrieves all the application permissions from the database.
            var permissions = await _dbContext.ApplicationPermissions.ToListAsync(cancellationToken: cancellationToken);

            // Selects the permissions that belong to the role and maps them to PermissionItem objects.
            var selectedPermissions = role.RoleClaims.Join(permissions, rc => rc.ClaimValue, p => p.Name, (rc, p) => new PermissionItem
            {
                Id = p.Id,
                Name = rc.ClaimValue,
            }).ToList();

            List<PermissionItem> requestedPermissions;

            // Checks if the permissions are being loaded on demand or all at once.
            if (request.LoadingOnDemand)
            {
                // Retrieves the requested permissions and maps them to PermissionItem objects.
                var loadedOnDemandPermissions = await _permissionService.GetPermissionsOnDemand(new GetPermissionsQuery { Id = request.PermissionId });
                requestedPermissions = loadedOnDemandPermissions.Payload.Permissions;
            }
            else
            {
                // Retrieves all the application permissions and maps them to PermissionItem objects.
                var loadedOneShotPermissions = await _permissionService.GetAllPermissions();
                requestedPermissions = loadedOneShotPermissions.Payload.Permissions;
            }

            // Maps the role and the selected/requested permissions to a RolePermissionsResponse object.
            var rolePermissionsForEditResponse = RolePermissionsResponse.MapFromEntity(role, selectedPermissions, requestedPermissions);

            // Returns the mapped role and permissions in an Envelope object with the Ok status.
            return Envelope<RolePermissionsResponse>.Result.Ok(rolePermissionsForEditResponse);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}