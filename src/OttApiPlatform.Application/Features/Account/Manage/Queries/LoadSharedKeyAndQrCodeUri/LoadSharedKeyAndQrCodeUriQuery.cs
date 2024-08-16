namespace OttApiPlatform.Application.Features.Account.Manage.Queries.LoadSharedKeyAndQrCodeUri;

public class LoadSharedKeyAndQrCodeUriQuery : IRequest<Envelope<LoadSharedKeyAndQrCodeUriResponse>>
{
    #region Public Classes

    public class LoadSharedKeyAndQrCodeUriQueryHandler : IRequestHandler<LoadSharedKeyAndQrCodeUriQuery, Envelope<LoadSharedKeyAndQrCodeUriResponse>>
    {
        #region Private Fields

        private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
        private readonly ApplicationUserManager _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UrlEncoder _urlEncoder;

        #endregion Private Fields

        #region Public Constructors

        public LoadSharedKeyAndQrCodeUriQueryHandler(ApplicationUserManager userManager,
                                                     UrlEncoder urlEncoder,
                                                     IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _urlEncoder = urlEncoder;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<Envelope<LoadSharedKeyAndQrCodeUriResponse>> Handle(LoadSharedKeyAndQrCodeUriQuery request, CancellationToken cancellationToken)
        {
            // Get the current user ID from the HttpContext.
            var userId = _httpContextAccessor.GetUserId();

            // Check if the user id is null or empty.
            if (string.IsNullOrEmpty(userId))
                return Envelope<LoadSharedKeyAndQrCodeUriResponse>.Result.BadRequest(Resource.Invalid_user_Id);

            // Find the user based on the user id.
            var user = await _userManager.FindByIdAsync(userId);

            // Check if the user is null.
            if (user == null)
                return Envelope<LoadSharedKeyAndQrCodeUriResponse>.Result.Unauthorized(Resource.Unable_to_load_user);

            // Load the authenticator key & QR code URI to display on the form.
            var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);

            // Check if the authenticator key is null or empty.
            if (string.IsNullOrEmpty(unformattedKey))
            {
                // Reset the authenticator key if it is null or empty.
                await _userManager.ResetAuthenticatorKeyAsync(user);
                unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            }

            // Format the authenticator key.
            var loadSharedKeyAndQrCodeUriResponse = new LoadSharedKeyAndQrCodeUriResponse
            {
                SharedKey = FormatKey(unformattedKey)
            };

            // Get the user email.
            var email = await _userManager.GetEmailAsync(user);

            // Generate the QR code URI based on the user email and authenticator key.
            loadSharedKeyAndQrCodeUriResponse.AuthenticatorUri = GenerateQrCodeUri(email, unformattedKey);

            // Return the result.
            return Envelope<LoadSharedKeyAndQrCodeUriResponse>.Result.Ok(loadSharedKeyAndQrCodeUriResponse);
        }

        #endregion Public Methods

        #region Private Methods

        private static string FormatKey(string unformattedKey)
        {
            var result = new StringBuilder();
            var currentPosition = 0;

            // Split the unformattedKey into groups of four characters separated by spaces.
            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
                currentPosition += 4;
            }

            // Append any remaining characters that are less than four.
            if (currentPosition < unformattedKey.Length)
                result.Append(unformattedKey.Substring(currentPosition));

            // Convert the result to lowercase and return it.
            return result.ToString().ToLowerInvariant();
        }

        private string GenerateQrCodeUri(string email, string unformattedKey)
        {
            // Format the authenticator URI using the provided parameters and URL-encode them.
            return string.Format(AuthenticatorUriFormat,
                                 _urlEncoder.Encode("OttApiPlatform"),
                                 _urlEncoder.Encode(email),
                                 unformattedKey);
        }

        #endregion Private Methods
    }

    #endregion Public Classes
}