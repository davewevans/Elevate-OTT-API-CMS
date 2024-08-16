namespace OttApiPlatform.Application.Features.Account.Manage.Commands.Disable2fa;

public class Disable2FaCommand : IRequest<Envelope<string>>
{
    #region Public Classes

    public class Disable2FaCommandHandler : IRequestHandler<Disable2FaCommand, Envelope<string>>
    {
        #region Private Fields

        private readonly ApplicationUserManager _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion Private Fields

        #region Public Constructors

        public Disable2FaCommandHandler(ApplicationUserManager userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<string>> Handle(Disable2FaCommand request, CancellationToken cancellationToken)
        {
            // Get the current user ID from the HttpContext.
            var userId = _httpContextAccessor.GetUserId();

            // If the user ID is missing or invalid, return a bad request response.
            if (string.IsNullOrEmpty(userId))
                return Envelope<string>.Result.BadRequest(Resource.Invalid_user_Id);

            // Find the user with the specified ID.
            var user = await _userManager.FindByIdAsync(userId);

            // If the user cannot be found, return an unauthorized response.
            if (user == null)
                return Envelope<string>.Result.Unauthorized(Resource.Unable_to_load_user);

            // Disable two-factor authentication for the user.
            var identityResult = await _userManager.SetTwoFactorEnabledAsync(user, false);

            // If an error occurred while disabling 2FA, return a server error response.
            if (!identityResult.Succeeded)
                return Envelope<string>.Result.ServerError(string.Format(Resource.Unexpected_error_occurred_disabling_2FA, user.Id));

            // Return a success response.
            return Envelope<string>.Result.Ok(Resource.Two_factor_authentication_has_been_disabled);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}