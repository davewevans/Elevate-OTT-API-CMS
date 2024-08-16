namespace OttApiPlatform.Application.Features.Identity.Users.Queries.GetUserPermissions;

public class GetUserPermissionsQuery : IRequest<Envelope<UserPermissionsResponse>>
{
    #region Public Properties

    public string UserId { get; set; }
    public bool LoadingOnDemand { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class GetUserPermissionsQueryHandler : IRequestHandler<GetUserPermissionsQuery, Envelope<UserPermissionsResponse>>
    {
        #region Private Fields

        private readonly ApplicationUserManager _userManager;
        private readonly IPermissionService _permissionService;

        #endregion Private Fields

        #region Public Constructors

        public GetUserPermissionsQueryHandler(ApplicationUserManager userManager, IPermissionService permissionService)
        {
            _userManager = userManager;
            _permissionService = permissionService;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<UserPermissionsResponse>> Handle(GetUserPermissionsQuery request, CancellationToken cancellationToken)
        {
            // Find the user by their ID using the user manager and include their claims.
            var user = await _userManager.Users.Include(u => u.Claims)
                                               .Where(u => u.Id == request.UserId)
                                               .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // If the user is not found, return a NotFound response with an error message.
            if (user == null)
                return Envelope<UserPermissionsResponse>.Result.NotFound(Resource.Unable_to_load_user);

            // Get the selected non-excluded permissions for the user.
            var selectedNonExcludedPermissions = await _permissionService.GetUserNonExcludedPermissions(user);

            // Declare a list of all permissions.
            IReadOnlyList<PermissionItem> allPermissions;

            // If loading on demand is requested, get the permissions on demand and set them to allPermissions.
            if (request.LoadingOnDemand)
            {
                var loadedOnDemandPermissions = await _permissionService.GetPermissionsOnDemand(new GetPermissionsQuery());
                allPermissions = loadedOnDemandPermissions.Payload.Permissions;
            }
            // If not, get all permissions at once and set them to allPermissions.
            else
            {
                var loadedOneShotPermissions = await _permissionService.GetAllPermissions();
                allPermissions = loadedOneShotPermissions.Payload.Permissions;
            }

            // Create a new UserPermissionsResponse with the selected and requested permissions, and
            // return an OK response with it.
            var userPermissionsResponse = new UserPermissionsResponse
            {
                SelectedPermissions = selectedNonExcludedPermissions,
                RequestedPermissions = allPermissions
            };
            // Return the user non-excluded permissions.
            return Envelope<UserPermissionsResponse>.Result.Ok(userPermissionsResponse);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}