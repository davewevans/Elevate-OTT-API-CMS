namespace OttApiPlatform.Application.Features.Account.Manage.Commands.SetPassword;

public partial class SetPasswordCommand : IRequest<Envelope<SetPasswordResponse>>
{
    #region Public Properties

    public string NewPassword { get; set; }
    public string ConfirmPassword { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class SetPasswordCommandHandler : IRequestHandler<SetPasswordCommand, Envelope<SetPasswordResponse>>
    {
        #region Private Fields

        private readonly ApplicationUserManager _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthenticationService _authenticationService;

        #endregion Private Fields

        #region Public Constructors

        public SetPasswordCommandHandler(ApplicationUserManager userManager,
                                         IHttpContextAccessor httpContextAccessor,
                                         IAuthenticationService authenticationService)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _authenticationService = authenticationService;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<SetPasswordResponse>> Handle(SetPasswordCommand request, CancellationToken cancellationToken)
        {
            // Get the current user ID from the HttpContext.
            var userId = _httpContextAccessor.GetUserId();

            // Return a bad request result if the user ID is invalid.
            if (string.IsNullOrEmpty(userId))
                return Envelope<SetPasswordResponse>.Result.BadRequest(Resource.Invalid_user_Id);

            // Find the user based on the ID.
            var user = await _userManager.FindByIdAsync(userId);

            // Return an unauthorized result if the user is not found.
            if (user == null)
                return Envelope<SetPasswordResponse>.Result.Unauthorized(Resource.Unable_to_load_user);

            // Add the new password to the user's account.
            var identityResult = await _userManager.AddPasswordAsync(user, request.NewPassword);

            // Return a server error result if the password cannot be added.
            if (!identityResult.Succeeded)
                return Envelope<SetPasswordResponse>.Result.AddErrors(identityResult.Errors.ToApplicationResult(), HttpStatusCode.InternalServerError);

            // Log the user in with the new password.
            var loginResponse = await Login(new LoginCommand
            {
                Email = user.Email,
                Password = request.NewPassword
            });

            // Create the response object with the new access token.
            var setPasswordResponse = new SetPasswordResponse
            {
                NewAccessToken = loginResponse.Payload.AuthResponse.AccessToken,
                StatusMessage = Resource.Your_password_has_been_set
            };

            // Return the response object with a successful result.
            return Envelope<SetPasswordResponse>.Result.Ok(setPasswordResponse);
        }

        #endregion Public Methods

        #region Private Methods

        private async Task<Envelope<LoginResponse>> Login(LoginCommand request)
        {
            return await _authenticationService.Login(request);
        }

        #endregion Private Methods
    }

    #endregion Public Classes
}