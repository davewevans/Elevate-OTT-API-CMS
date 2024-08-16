namespace OttApiPlatform.Application.Features.Account.Commands.ResetPassword;

public class ResetPasswordCommand : IRequest<Envelope<string>>
{
    #region Public Properties

    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string Code { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Envelope<string>>
    {
        #region Private Fields

        private readonly ApplicationUserManager _userManager;

        #endregion Private Fields

        #region Public Constructors

        public ResetPasswordCommandHandler(ApplicationUserManager userManager)
        {
            _userManager = userManager;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            // If user doesn't exist, return a success envelope indicating that the password has
            // been reset (without revealing that the user doesn't exist).
            if (user == null)
                return Envelope<string>.Result.Ok(Resource.Your_password_has_been_reset);

            // Reset the user's password.
            var identityResult = await _userManager.ResetPasswordAsync(user,
                                                                       Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code)),
                                                                       request.Password);

            // Return an error envelope if password reset failed and a success envelope if password
            // reset succeeded.
            return !identityResult.Succeeded
                ? Envelope<string>.Result.AddErrors(identityResult.Errors.ToApplicationResult(), HttpStatusCode.InternalServerError, rollbackDisabled: true)
                : Envelope<string>.Result.Ok(Resource.Your_password_has_been_reset);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}