namespace OttApiPlatform.Application.Features.Account.Manage.Queries.GetUserAvatar;

public class GetUserAvatarForEditQuery : IRequest<Envelope<UserAvatarForEditResponse>>
{
    #region Public Classes

    public class GetUserAvatarQueryHandler : IRequestHandler<GetUserAvatarForEditQuery, Envelope<UserAvatarForEditResponse>>
    {
        #region Private Fields

        private readonly ApplicationUserManager _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion Private Fields

        #region Public Constructors

        public GetUserAvatarQueryHandler(ApplicationUserManager userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<UserAvatarForEditResponse>> Handle(GetUserAvatarForEditQuery request, CancellationToken cancellationToken)
        {
            // Get the current user ID from the HttpContext.
            var userId = _httpContextAccessor.GetUserId();

            // Return BadRequest if user ID is null or empty.
            if (string.IsNullOrEmpty(userId))
                return Envelope<UserAvatarForEditResponse>.Result.BadRequest(Resource.Invalid_user_Id);

            // Find the user by their ID.
            var user = await _userManager.FindByIdAsync(userId);

            // Return Unauthorized if user is null.
            if (user == null)
                return Envelope<UserAvatarForEditResponse>.Result.Unauthorized(Resource.Unable_to_load_user);

            // Create a UserAvatarForEditResponse object with the user's avatar URI.
            var payload = new UserAvatarForEditResponse
            {
                AvatarUri = user.AvatarUri
            };

            // Return Ok with the UserAvatarForEditResponse object.
            return Envelope<UserAvatarForEditResponse>.Result.Ok(payload);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}