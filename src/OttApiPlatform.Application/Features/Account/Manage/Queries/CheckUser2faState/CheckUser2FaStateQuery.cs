namespace OttApiPlatform.Application.Features.Account.Manage.Queries.CheckUser2faState;

public class CheckUser2FaStateQuery : IRequest<Envelope<User2FaStateResponse>>
{
    #region Public Classes

    public class CheckUser2FaStateQueryHandler : IRequestHandler<CheckUser2FaStateQuery, Envelope<User2FaStateResponse>>
    {
        #region Private Fields

        private readonly ApplicationUserManager _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion Private Fields

        #region Public Constructors

        public CheckUser2FaStateQueryHandler(ApplicationUserManager userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<User2FaStateResponse>> Handle(CheckUser2FaStateQuery request, CancellationToken cancellationToken)
        {
            // Get the current user ID from the HttpContext.
            var userId = _httpContextAccessor.GetUserId();

            // Validate user id.
            if (string.IsNullOrEmpty(userId))
                return Envelope<User2FaStateResponse>.Result.BadRequest(Resource.Invalid_user_Id);

            // Retrieve user from UserManager.
            var user = await _userManager.FindByIdAsync(userId);

            // Check if user exists.
            if (user == null)
                return Envelope<User2FaStateResponse>.Result.Unauthorized(Resource.Unable_to_load_user);

            // Check if two-factor authentication is enabled for the user.
            var isTwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user);

            // Create the response object.
            var user2FaStateResponse = new User2FaStateResponse
            {
                IsTwoFactorEnabled = isTwoFactorEnabled,
                StatusMessage = !isTwoFactorEnabled ? string.Format(Resource.Cannot_generate_recovery_codes, user.UserName) : string.Empty,
            };

            // Return the response object.
            return Envelope<User2FaStateResponse>.Result.Ok(user2FaStateResponse);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}