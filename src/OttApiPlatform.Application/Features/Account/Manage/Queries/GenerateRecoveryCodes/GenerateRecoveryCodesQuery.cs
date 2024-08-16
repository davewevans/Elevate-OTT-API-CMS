namespace OttApiPlatform.Application.Features.Account.Manage.Queries.GenerateRecoveryCodes;

public class GenerateRecoveryCodesQuery : IRequest<Envelope<GenerateRecoveryCodesResponse>>
{
    #region Public Classes

    public class GenerateRecoveryCodesQueryHandler : IRequestHandler<GenerateRecoveryCodesQuery, Envelope<GenerateRecoveryCodesResponse>>
    {
        #region Private Fields

        private readonly ApplicationUserManager _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion Private Fields

        #region Public Constructors

        public GenerateRecoveryCodesQueryHandler(ApplicationUserManager userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<GenerateRecoveryCodesResponse>> Handle(GenerateRecoveryCodesQuery request, CancellationToken cancellationToken)
        {
            // Get the current user ID from the HttpContext.
            var userId = _httpContextAccessor.GetUserId();

            // Check if the user ID is null or empty.
            if (string.IsNullOrEmpty(userId))
                return Envelope<GenerateRecoveryCodesResponse>.Result.BadRequest(Resource.Invalid_user_Id);

            // Find the user based on the user ID.
            var user = await _userManager.FindByIdAsync(userId);

            // If the user is not found, return an unauthorized error.
            if (user == null)
                return Envelope<GenerateRecoveryCodesResponse>.Result.Unauthorized(Resource.Unable_to_load_user);

            // Check if the user has 2FA enabled.
            var isTwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user);

            // If 2FA is not enabled, return an error.
            if (!isTwoFactorEnabled)
                return Envelope<GenerateRecoveryCodesResponse>.Result.ServerError(string.Format(Resource.Cannot_generate_recovery_codes, user.UserName));

            // Generate new recovery codes for the user.
            var generateRecoveryCodesResponse = new GenerateRecoveryCodesResponse
            {
                RecoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10)
            };

            // Convert the recovery codes to an array.
            generateRecoveryCodesResponse.RecoveryCodes = generateRecoveryCodesResponse.RecoveryCodes?.ToArray();

            // Set the success message for the response.
            generateRecoveryCodesResponse.StatusMessage = Resource.You_have_generated_new_recovery_codes;

            // Return the response wrapped in an Envelope.
            return Envelope<GenerateRecoveryCodesResponse>.Result.Ok(generateRecoveryCodesResponse);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}