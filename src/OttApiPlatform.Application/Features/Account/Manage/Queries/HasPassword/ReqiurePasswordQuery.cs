namespace OttApiPlatform.Application.Features.Account.Manage.Queries.HasPassword;

public class RequirePasswordQuery : IRequest<Envelope<bool>>
{
    #region Public Classes

    public class RequirePasswordQueryHandler : IRequestHandler<RequirePasswordQuery, Envelope<bool>>
    {
        #region Private Fields

        private readonly ApplicationUserManager _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion Private Fields

        #region Public Constructors

        public RequirePasswordQueryHandler(ApplicationUserManager userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<bool>> Handle(RequirePasswordQuery request, CancellationToken cancellationToken)
        {
            // Get the current user ID from the HttpContext.
            var userId = _httpContextAccessor.GetUserId();

            // If the user ID is null or empty, return a bad request result with a message
            // indicating an invalid user ID.
            if (string.IsNullOrEmpty(userId))
                return Envelope<bool>.Result.BadRequest(Resource.Invalid_user_Id);

            // Find the user by their ID.
            var user = await _userManager.FindByIdAsync(userId);

            // If the user cannot be found, return an unauthorized result with a message indicating
            // that the user cannot be loaded.
            return user == null
                ? Envelope<bool>.Result.Unauthorized(Resource.Unable_to_load_user)
                : Envelope<bool>.Result.Ok(await _userManager.HasPasswordAsync(user));
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}