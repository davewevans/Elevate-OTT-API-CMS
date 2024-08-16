namespace OttApiPlatform.Application.Features.Account.Manage.Commands.DeletePersonalData;

public class DeletePersonalDataCommand : IRequest<Envelope<string>>
{
    #region Public Properties

    public string Password { get; set; }

    #endregion Public Properties

    public class DeletePersonalDataHandler : IRequestHandler<DeletePersonalDataCommand, Envelope<string>>
    {
        #region Private Fields

        private readonly ApplicationUserManager _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion Private Fields

        #region Public Constructors

        public DeletePersonalDataHandler(ApplicationUserManager userManager,
                                         SignInManager<ApplicationUser> signInManager,
                                         IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion Public Constructors

        #region Public Methods

        #region Public Methods

        public async Task<Envelope<string>> Handle(DeletePersonalDataCommand request, CancellationToken cancellationToken)
        {
            // Get the current user ID from the HttpContext.
            var userId = _httpContextAccessor.GetUserId();

            // If the user ID is null or empty, return a bad request result with an error message.
            if (string.IsNullOrEmpty(userId))
                return Envelope<string>.Result.BadRequest(Resource.Invalid_user_Id);

            // Find the user by ID.
            var user = await _userManager.FindByIdAsync(userId);

            // If the user is null, return an unauthorized result with an error message.
            if (user == null)
                return Envelope<string>.Result.Unauthorized(Resource.Unable_to_load_user);

            // Check if the user requires a password to delete their personal data.
            var requirePassword = await _userManager.HasPasswordAsync(user);

            // If a password is required, check if the provided password is correct.
            if (requirePassword)
                if (!await _userManager.CheckPasswordAsync(user, request.Password))
                    return Envelope<string>.Result.ServerError(Resource.Incorrect_password);

            // Delete the user.
            var identityResult = await _userManager.DeleteAsync(user);

            // If the delete operation failed, return a server error result with an error message.
            if (!identityResult.Succeeded)
                return Envelope<string>.Result.ServerError(string.Format(Resource.Unexpected_error_occurred_deleting_user_with_Id, user.Id));

            // Sign the user out.
            await _signInManager.SignOutAsync();

            // Return an OK result with a success message.
            return Envelope<string>.Result.Ok(string.Format(Resource.User_with_Id_deleted, userId));
        }

        #endregion Public Methods
    }

    #endregion Public Methods
}