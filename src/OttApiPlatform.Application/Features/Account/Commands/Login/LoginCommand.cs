namespace OttApiPlatform.Application.Features.Account.Commands.Login;

public class LoginCommand : IRequest<Envelope<LoginResponse>>
{
    #region Public Properties

    public string Email { get; set; }
    public string Password { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class LoginCommandHandler : IRequestHandler<LoginCommand, Envelope<LoginResponse>>
    {
        #region Private Fields

        private readonly ApplicationUserManager _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAuthenticationService _authenticationService;

        #endregion Private Fields

        #region Public Constructors

        public LoginCommandHandler(ApplicationUserManager userManager,
                                   SignInManager<ApplicationUser> signInManager,
                                   IAuthenticationService authenticationService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authenticationService = authenticationService;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // Attempt to sign in the user with their email and password.
            var signInResult = await _signInManager.PasswordSignInAsync(request.Email,
                                                                        request.Password,
                                                                        isPersistent: false,
                                                                        lockoutOnFailure: true);
            
            // If the sign-in is successful.
            if (signInResult.Succeeded)
            {
                // Find the user by email.
                var user = await _userManager.FindByNameAsync(request.Email);

                // If user cannot be found, return a server error with an error message.
                if (user == null)
                    return Envelope<LoginResponse>.Result.ServerError(Resource.Invalid_login_attempt);

                // If user is suspended, return a server error with a deactivation message.
                if (user.IsSuspended)
                    return
                        Envelope<LoginResponse>.Result
                                               .ServerError(Resource.Your_account_is_deactivated_Please_contact_your_administrator,
                                                            rollbackDisabled: true);

                // Generate access and refresh tokens for the user.
                var (accessToken, refreshToken) = await _authenticationService.GenerateAccessAndRefreshTokens(user);

                // Create an authentication response with the access and refresh tokens.
                var authResponse = new AuthResponse { AccessToken = accessToken, RefreshToken = refreshToken };

                // Create a login response with the authentication response and a flag indicating
                // whether two-factor authentication is required.
                var loginResponse = new LoginResponse
                {
                    AuthResponse = authResponse,
                    RequiresTwoFactor = false,
                };

                // Return a successful result with the login response.
                return Envelope<LoginResponse>.Result.Ok(loginResponse);
            }

            // If the sign-in requires two-factor authentication, return a successful result
            // indicating this.
            if (signInResult.RequiresTwoFactor)
                return Envelope<LoginResponse>.Result.Ok(new LoginResponse { RequiresTwoFactor = true });

            // Otherwise, return an unsuccessful result with any sign-in errors.
            return Envelope<LoginResponse>.Result.AddErrors(signInResult.ToApplicationResult(),
                                                            HttpStatusCode.InternalServerError, rollbackDisabled: true);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}