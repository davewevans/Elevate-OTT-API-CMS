namespace OttApiPlatform.Application.Features.Account.Commands.ConfirmEmail;

public class ConfirmEmailCommand : IRequest<Envelope<ConfirmEmailResponse>>
{
    #region Public Properties

    public string UserId { get; set; }
    public string Code { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, Envelope<ConfirmEmailResponse>>
    {
        #region Private Fields

        private readonly ApplicationUserManager _userManager;

        private readonly IAuthenticationService _authenticationService;

        #endregion Private Fields

        #region Public Constructors

        public ConfirmEmailCommandHandler(ApplicationUserManager userManager, IAuthenticationService authenticationService)
        {
            _userManager = userManager;
            _authenticationService = authenticationService;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<ConfirmEmailResponse>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            //Check if the user ID is valid.
            if (string.IsNullOrWhiteSpace(request.UserId))
                return Envelope<ConfirmEmailResponse>.Result.BadRequest(Resource.Invalid_user_Id);

            // Find the user by their ID.
            var user = await _userManager.FindByIdAsync(request.UserId);

            // If user is not found, return success message to avoid revealing that the user does
            // not exist.
            if (user == null)
                return Envelope<ConfirmEmailResponse>.Result.Ok(new ConfirmEmailResponse
                {
                    SuccessMessage = Resource.User_email_has_been_confirmed_successfully
                });

            // Decode the confirmation code.
            request.Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));

            // Confirm the user's email address.
            var identityResult = await _userManager.ConfirmEmailAsync(user, request.Code);

            var authResponse = await _authenticationService.LoginUserAfterConfirmEmail(user);

            // If confirmation fails, return errors.
            return !identityResult.Succeeded
                ? Envelope<ConfirmEmailResponse>.Result.AddErrors(identityResult.Errors.ToApplicationResult(),
                    HttpStatusCode.InternalServerError, rollbackDisabled: true)
                : Envelope<ConfirmEmailResponse>.Result.Ok(new ConfirmEmailResponse
                {
                    SuccessMessage = Resource.User_email_has_been_confirmed_successfully,
                    AuthResponse = authResponse
                });
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}