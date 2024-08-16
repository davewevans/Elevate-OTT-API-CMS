namespace OttApiPlatform.Application.Features.Account.Commands.ResendEmailConfirmation;

public class ResendEmailConfirmationCommand : IRequest<Envelope<ResendEmailConfirmationResponse>>
{
    #region Public Properties

    public string Email { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class ResendEmailConfirmationCommandHandler : IRequestHandler<ResendEmailConfirmationCommand, Envelope<ResendEmailConfirmationResponse>>
    {
        #region Private Fields

        private readonly ApplicationUserManager _userManager;

        #endregion Private Fields

        #region Public Constructors

        public ResendEmailConfirmationCommandHandler(ApplicationUserManager userManager)
        {
            _userManager = userManager;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<ResendEmailConfirmationResponse>> Handle(ResendEmailConfirmationCommand request, CancellationToken cancellationToken)
        {
            ResendEmailConfirmationResponse resendEmailConfirmationResponse;

            // Find the user by email address.
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                // Don't reveal that the user does not exist.
                resendEmailConfirmationResponse = new ResendEmailConfirmationResponse
                {
                    RequireConfirmedAccount = true,
                    DisplayConfirmAccountLink = true,
                    SuccessMessage = Resource.Verification_email_has_been_sent
                };
                return Envelope<ResendEmailConfirmationResponse>.Result.Ok(resendEmailConfirmationResponse);
            }

            // Send the activation email and get the callback URL.
            var callbackUrl = await _userManager.SendActivationEmailAsync(user);

            // Return a successful response with the confirmation URL.
            resendEmailConfirmationResponse = new ResendEmailConfirmationResponse
            {
                RequireConfirmedAccount = true,
                DisplayConfirmAccountLink = true,
                EmailConfirmationUrl = HttpUtility.UrlEncode(callbackUrl),
                SuccessMessage = Resource.Verification_email_has_been_sent
            };

            return Envelope<ResendEmailConfirmationResponse>.Result.Ok(resendEmailConfirmationResponse);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}