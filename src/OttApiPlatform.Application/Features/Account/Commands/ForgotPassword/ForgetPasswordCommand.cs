namespace OttApiPlatform.Application.Features.Account.Commands.ForgotPassword;

public class ForgetPasswordCommand : IRequest<Envelope<ForgetPasswordResponse>>
{
    #region Public Properties

    public string Email { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class ForgotPasswordCommandHandler : IRequestHandler<ForgetPasswordCommand, Envelope<ForgetPasswordResponse>>
    {
        #region Private Fields

        private readonly ApplicationUserManager _userManager;

        #endregion Private Fields

        #region Public Constructors

        public ForgotPasswordCommandHandler(ApplicationUserManager userManager)
        {
            _userManager = userManager;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<ForgetPasswordResponse>> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
        {
            // Find the user associated with the email address provided by the user.
            var user = await _userManager.FindByEmailAsync(request.Email);

            // If no user is found, return a success message without revealing the reason for failure.
            if (user == null || !user.EmailConfirmed)
                return Envelope<ForgetPasswordResponse>.Result.Ok(new ForgetPasswordResponse
                {
                    Code = null,
                    SuccessMessage = Resource.Password_reset_link_was_sent
                });

            // If the user exists, send a password reset code to their email address.
            var code = await _userManager.SendResetPasswordAsync(user);

            // Create a response object that includes the password reset code and a success message.
            var forgetPasswordResponse = new ForgetPasswordResponse
            {
                Code = code,
                DisplayConfirmPasswordLink = true,
                SuccessMessage = Resource.Password_reset_link_was_sent,
            };

            // Return the response object in an envelope with a success status.
            return Envelope<ForgetPasswordResponse>.Result.Ok(forgetPasswordResponse);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}