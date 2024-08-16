namespace OttApiPlatform.Application.Features.Account.Manage.Commands.ChangeEmail;

public class ChangeEmailCommand : IRequest<Envelope<ChangeEmailResponse>>
{
    #region Public Properties

    public string NewEmail { get; set; }
    public bool DisplayConfirmAccountLink { get; set; } = true;

    #endregion Public Properties

    #region Public Classes

    public class ChangeEmailCommandHandler : IRequestHandler<ChangeEmailCommand, Envelope<ChangeEmailResponse>>
    {
        #region Private Fields

        private readonly ApplicationUserManager _userManager;
        private readonly ITokenGeneratorService _tokenGeneratorService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion Private Fields

        #region Public Constructors

        public ChangeEmailCommandHandler(ApplicationUserManager userManager,
                                         ITokenGeneratorService tokenGeneratorService,
                                         IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _tokenGeneratorService = tokenGeneratorService;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<ChangeEmailResponse>> Handle
            (ChangeEmailCommand request, CancellationToken cancellationToken)
        {
            // Get the current user ID from the HttpContext.
            var userId = _httpContextAccessor.GetUserId();

            // Return a bad request error if the user ID is null or empty.
            if (string.IsNullOrEmpty(userId))
                return Envelope<ChangeEmailResponse>.Result.BadRequest(Resource.Invalid_user_Id);

            // Find the user based on the retrieved ID.
            var user = await _userManager.FindByIdAsync(_httpContextAccessor.GetUserId());

            // If the user cannot be found, return an unauthorized error.
            if (user == null)
                return Envelope<ChangeEmailResponse>.Result.Unauthorized(Resource.Unable_to_load_user);

            // Get the current email address of the user.
            var email = await _userManager.GetEmailAsync(user);

            ChangeEmailResponse changeEmailResponse;

            // If the new email address is the same as the current email address, return a success
            // message with the current email address.
            if (request.NewEmail == email)
            {
                changeEmailResponse = new ChangeEmailResponse
                {
                    RequireConfirmedAccount = false,
                    EmailUnchanged = true,
                    EmailConfirmationUrl = null,
                    AuthResponse = null,
                    SuccessMessage = Resource.Your_email_is_unchanged
                };
                return Envelope<ChangeEmailResponse>.Result.Ok(changeEmailResponse);
            }

            // If confirmed account is required, generate a confirmation link and a token response
            // for authentication.
            if (_userManager.Options.SignIn.RequireConfirmedAccount)
            {
                var callbackUrl = await _userManager.SendActivationEmailAsync(user, request.NewEmail);

                var tokenResponse = await GenerateAuthResponseAsync(user);

                // If the token response is null, return a bad request error with a message.
                if (tokenResponse == null)
                    return Envelope<ChangeEmailResponse>.Result.BadRequest(string.Format(Resource.value_cannot_be_null,
                                                                               nameof(tokenResponse)));

                //If the request specifies to display the confirm account link, return the link along with the token response; otherwise, return only the link.
                if (request.DisplayConfirmAccountLink)
                {
                    changeEmailResponse = new ChangeEmailResponse
                    {
                        RequireConfirmedAccount = true,
                        DisplayConfirmAccountLink = true,
                        EmailConfirmationUrl = HttpUtility.UrlEncode(callbackUrl),
                        AuthResponse = tokenResponse,
                        SuccessMessage = Resource.Confirmation_link_to_change_email_has_been_sent
                    };
                    return Envelope<ChangeEmailResponse>.Result.Ok(changeEmailResponse);
                }

                changeEmailResponse = new ChangeEmailResponse
                {
                    RequireConfirmedAccount = true,
                    DisplayConfirmAccountLink = false,
                    EmailConfirmationUrl = HttpUtility.UrlEncode(callbackUrl),
                    SuccessMessage = Resource.Confirmation_link_to_change_email_has_been_sent,
                };

                return Envelope<ChangeEmailResponse>.Result.Ok(changeEmailResponse);
            }

            // If confirmed account is not required, simply update the user name and email and
            // return the result.
            return await UpdateUserNameAndEmail(user, request.NewEmail);
        }

        #endregion Public Methods

        #region Private Methods

        private async Task<Envelope<ChangeEmailResponse>> UpdateUserNameAndEmail(ApplicationUser user, string email)
        {
            // Update the user's email.
            user.Email = email;

            // Update the user's information in the database.
            var updateUserResult = await _userManager.UpdateAsync(user);

            // If the update was not successful, return an error.
            if (!updateUserResult.Succeeded)
                return Envelope<ChangeEmailResponse>.Result.AddErrors(updateUserResult.Errors.ToApplicationResult(),
                                                                      HttpStatusCode.InternalServerError);

            // In our UI, email and user name are one and the same, so when we update the email we
            // need to update the user name.
            var setUserNameResult = await _userManager.SetUserNameAsync(user, email);

            // If the update was not successful, return an error.
            if (!setUserNameResult.Succeeded)
                return Envelope<ChangeEmailResponse>.Result.ServerError(Resource.Error_changing_user_name);

            // Generate an authentication response for the user.
            var authResponse = await GenerateAuthResponseAsync(user);

            // Create a new ChangeEmailResponse object with the new email and authentication response.
            var changeEmailResponse = new ChangeEmailResponse
            {
                RequireConfirmedAccount = false,
                DisplayConfirmAccountLink = false,
                EmailConfirmationUrl = null,
                SuccessMessage = Resource.Your_email_has_been_successfully_changed,
                AuthResponse = authResponse
            };

            return Envelope<ChangeEmailResponse>.Result.Ok(changeEmailResponse);
        }

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