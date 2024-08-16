namespace OttApiPlatform.Application.Features.Account.Commands.LoginWithRecoveryCode;

public class LoginWithRecoveryCodeCommand : IRequest<Envelope<LoginWithRecoveryCodeResponse>>
{
    #region Public Properties

    public string RecoveryCode { get; set; }
    public string UserName { get; set; }
    public string ReturnUrl { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class LoginWithRecoveryCodeCommandHandler : IRequestHandler<LoginWithRecoveryCodeCommand, Envelope<LoginWithRecoveryCodeResponse>>
    {
        #region Private Fields

        private readonly ApplicationUserManager _userManager;
        private readonly IAuthenticationService _authenticationService;

        #endregion Private Fields

        #region Public Constructors

        public LoginWithRecoveryCodeCommandHandler(ApplicationUserManager userManager, IAuthenticationService authenticationService)
        {
            _userManager = userManager;
            _authenticationService = authenticationService;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<LoginWithRecoveryCodeResponse>> Handle(LoginWithRecoveryCodeCommand request, CancellationToken cancellationToken)
        {
            // Find the user with email.
            var user = await _userManager.FindByEmailAsync(request.UserName);

            // If no user found, return a NotFound response.
            if (user == null)
                return Envelope<LoginWithRecoveryCodeResponse>.Result.NotFound(Resource.Unable_to_load_user);

            // Remove any spaces from the recovery code.
            var recoveryCode = request.RecoveryCode.Replace(" ", string.Empty);

            // Redeem the two-factor recovery code for the user.
            var identityResult = await _userManager.RedeemTwoFactorRecoveryCodeAsync(user, recoveryCode);

            // If the redemption of the recovery code is successful.
            if (identityResult.Succeeded)
            {
                // Generate access and refresh tokens.
                var (accessToken, refreshToken) = await _authenticationService.GenerateAccessAndRefreshTokens(user);

                // Create an authorization response containing the tokens.
                var authResponse = new AuthResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                };

                // Create a response containing the authorization response.
                var loginWithRecoveryCodeResponse = new LoginWithRecoveryCodeResponse { AuthResponse = authResponse };

                return Envelope<LoginWithRecoveryCodeResponse>.Result.Ok(loginWithRecoveryCodeResponse);
            }

            // If the redemption of the recovery code is not successful, return a BadRequest response.
            return Envelope<LoginWithRecoveryCodeResponse>.Result.BadRequest(Resource.Invalid_recovery_code_entered);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}