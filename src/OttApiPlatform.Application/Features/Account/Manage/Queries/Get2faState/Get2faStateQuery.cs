namespace OttApiPlatform.Application.Features.Account.Manage.Queries.Get2faState;

public class Get2FaStateQuery : IRequest<Envelope<TwoFactorAuthenticationStateResponse>>
{
    #region Public Classes

    public class Get2FaStateQueryHandler : IRequestHandler<Get2FaStateQuery, Envelope<TwoFactorAuthenticationStateResponse>>
    {
        #region Private Fields

        private readonly ApplicationUserManager _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion Private Fields

        #region Public Constructors

        public Get2FaStateQueryHandler(ApplicationUserManager userManager,
                                       SignInManager<ApplicationUser> signInManager,
                                       IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<TwoFactorAuthenticationStateResponse>> Handle(Get2FaStateQuery request, CancellationToken cancellationToken)
        {
            // Get the current user ID from the HttpContext.
            var userId = _httpContextAccessor.GetUserId();

            // Return a bad request result if the user ID is invalid.
            if (string.IsNullOrEmpty(userId))
                return Envelope<TwoFactorAuthenticationStateResponse>.Result.BadRequest(Resource.Invalid_user_Id);

            // Find the user based on the ID.
            var user = await _userManager.FindByIdAsync(userId);

            // Return an unauthorized result if the user is not found.
            if (user == null)
                return Envelope<TwoFactorAuthenticationStateResponse>.Result.Unauthorized(Resource.Unable_to_load_user);

            // Create the response object with the user's two-factor authentication state.
            var twoFactorAuthenticationStateResponse = new TwoFactorAuthenticationStateResponse
            {
                HasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user) != null,
                Is2FaEnabled = await _userManager.GetTwoFactorEnabledAsync(user),
                IsMachineRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user),
                RecoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user)
            };

            // Return the response object with a successful result.
            return Envelope<TwoFactorAuthenticationStateResponse>.Result.Ok(twoFactorAuthenticationStateResponse);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}