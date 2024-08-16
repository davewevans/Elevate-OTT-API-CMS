namespace OttApiPlatform.Application.Features.Account.Manage.Commands.ChangePassword;

public class ChangePasswordCommand : IRequest<Envelope<ChangePasswordResponse>>
{
    #region Public Properties

    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmPassword { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Envelope<ChangePasswordResponse>>
    {
        #region Private Fields

        private readonly ApplicationUserManager _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenGeneratorService _tokenGeneratorService;

        #endregion Private Fields

        #region Public Constructors

        public ChangePasswordCommandHandler(SignInManager<ApplicationUser> signInManager,
                                            ApplicationUserManager userManager,
                                            ITokenGeneratorService tokenGeneratorService,
                                            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenGeneratorService = tokenGeneratorService;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<ChangePasswordResponse>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            // Get the current user ID from the HttpContext.
            var userId = _httpContextAccessor.GetUserId();

            // If the user ID is null or empty, return a bad request result with a message
            // indicating an invalid user ID.
            if (string.IsNullOrEmpty(userId))
                return Envelope<ChangePasswordResponse>.Result.BadRequest(Resource.Invalid_user_Id);

            // Find the user by their ID.
            var user = await _userManager.FindByIdAsync(userId);

            // If the user cannot be found, return an unauthorized result with a message indicating
            // that. the user cannot be loaded.
            if (user == null)
                return Envelope<ChangePasswordResponse>.Result.Unauthorized(Resource.Unable_to_load_user);

            // Change the user's password and get the result.
            var identityResult = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);

            // If the password change was unsuccessful, return a result with the errors that
            // occurred during the password change process.
            if (!identityResult.Succeeded)
                return Envelope<ChangePasswordResponse>.Result.AddErrors(identityResult.Errors.ToApplicationResult(), HttpStatusCode.InternalServerError);

            // Refresh the user's sign-in cookie.
            await _signInManager.RefreshSignInAsync(user);

            // Generate a new auth response for the user.
            var authResponse = await GenerateAuthResponseAsync(user);

            // Create a new ChangePasswordResponse object with a success message and the newly
            // generated auth response.
            var changePasswordResponse = new ChangePasswordResponse
            {
                SuccessMessage = Resource.Your_password_has_been_changed,
                AuthResponse = authResponse
            };

            // Return an OK result with the changePasswordResponse object.
            return Envelope<ChangePasswordResponse>.Result.Ok(changePasswordResponse);
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