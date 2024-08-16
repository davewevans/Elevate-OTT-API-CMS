namespace OttApiPlatform.Application.Features.Account.Manage.Commands.ResetAuthenticator;

public class ResetAuthenticatorCommand : IRequest<Envelope<ResetAuthenticatorResponse>>
{
    #region Public Classes

    public class ConfirmEmailCommandHandler : IRequestHandler<ResetAuthenticatorCommand, Envelope<ResetAuthenticatorResponse>>
    {
        #region Private Fields

        private readonly ApplicationUserManager _userManager;
        private readonly ITokenGeneratorService _tokenGeneratorService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion Private Fields

        #region Public Constructors

        public ConfirmEmailCommandHandler(ApplicationUserManager userManager,
                                          ITokenGeneratorService tokenGeneratorService,
                                          IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _tokenGeneratorService = tokenGeneratorService;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<ResetAuthenticatorResponse>> Handle(ResetAuthenticatorCommand request, CancellationToken cancellationToken)
        {
            // Get the current user ID from the HttpContext.
            var userId = _httpContextAccessor.GetUserId();

            // Check if the user ID is null or empty.
            if (string.IsNullOrEmpty(userId))
                return Envelope<ResetAuthenticatorResponse>.Result.BadRequest(Resource.Invalid_user_Id);

            // Find the user based on the user ID.
            var user = await _userManager.FindByIdAsync(userId);

            // If the user is not found, return an unauthorized error.
            if (user == null)
                return Envelope<ResetAuthenticatorResponse>.Result.Unauthorized(Resource.Unable_to_load_user);

            // Disable 2FA for the user.
            var setTwoFactorEnabledResult = await _userManager.SetTwoFactorEnabledAsync(user, false);

            // If disabling 2FA fails, return an error.
            if (!setTwoFactorEnabledResult.Succeeded)
                return Envelope<ResetAuthenticatorResponse>.Result.ServerError(string.Format(Resource.Unable_to_enable_2FA, user.Id));

            // Reset the authenticator key for the user.
            var resetAuthenticatorKeyResult = await _userManager.ResetAuthenticatorKeyAsync(user);

            // If resetting the authenticator key fails, return an error.
            if (!resetAuthenticatorKeyResult.Succeeded)
                return Envelope<ResetAuthenticatorResponse>.Result.ServerError(string.Format(Resource.Unable_to_reset_authenticator_keys, user.Id));

            // Generate a new authentication response for the user.
            var authResponse = await GenerateAuthResponseAsync(user);

            // Create a new response for resetting the authenticator key.
            var resetAuthenticatorResponse = new ResetAuthenticatorResponse
            {
                // Set the success message for the response.
                StatusMessage = Resource.Your_authenticator_app_key_has_been_reset,
                // Set the new authentication response for the user.
                AuthResponse = authResponse
            };

            return Envelope<ResetAuthenticatorResponse>.Result.Ok(resetAuthenticatorResponse);
        }

        #endregion Public Methods

        #region Private Methods

        private async Task<AuthResponse> GenerateAuthResponseAsync(ApplicationUser user)
        {
            // Generate an access token for the user.
            var accessToken = await _tokenGeneratorService.GenerateAccessTokenAsync(user);

            // Generate a refresh token for the user.
            var refreshToken = _tokenGeneratorService.GenerateRefreshToken();

            // Create a new AuthResponse object with the access token and refresh token.
            var authResponse = new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            return authResponse;
        }

        #endregion Private Methods
    }

    #endregion Public Classes
}