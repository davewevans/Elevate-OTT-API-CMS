namespace OttApiPlatform.Application.Common.Extensions;

public static class SignInResultExtensions
{
    #region Public Methods

    /// <summary>
    /// Converts the SignInResult to a dictionary of key value pairs where the key is the name of.
    /// the property and the value is a localized error message.
    /// </summary>
    /// <param name="signInResult">The result of a sign-in attempt.</param>
    /// <returns>
    /// A dictionary of key value pairs where the key is the name of the property and the value is
    /// a. localized error message.
    /// </returns>
    public static Dictionary<string, string> ToApplicationResult(this SignInResult signInResult)
    {
        var keyValuePairs = new Dictionary<string, string>();

        if (signInResult.IsLockedOut)
            keyValuePairs.Add(nameof(signInResult.IsLockedOut), Resource.You_are_locked_out);

        if (signInResult.IsNotAllowed)
            keyValuePairs.Add(nameof(signInResult.IsNotAllowed), Resource.Please_confirm_your_email);

        if (signInResult.RequiresTwoFactor)
            keyValuePairs.Add(nameof(signInResult.RequiresTwoFactor), Resource.Two_factor_authentication_required);

        if (!signInResult.Succeeded)
            keyValuePairs.Add("Failed", Resource.Invalid_login_attempt);
        return keyValuePairs;
    }

    /// <summary>
    /// Converts the SignInResult to a serialized error message where each error message is.
    /// separated by "||".
    /// </summary>
    /// <param name="signInResult">The result of a sign-in attempt.</param>
    /// <returns>A serialized error message where each error message is separated by "||".</returns>
    public static string ToSerializedResult(this SignInResult signInResult)
    {
        var errorBuilder = new StringBuilder();

        if (signInResult.IsLockedOut)
        {
            errorBuilder.AppendLine(Resource.You_are_locked_out);

            errorBuilder.AppendLine("||");
        }

        if (signInResult.IsNotAllowed)
        {
            errorBuilder.AppendLine(Resource.Please_confirm_your_email);

            errorBuilder.AppendLine("||");
        }

        if (signInResult.RequiresTwoFactor)
        {
            errorBuilder.AppendLine(Resource.Two_factor_authentication_required);

            errorBuilder.AppendLine("||");
        }

        if (signInResult.Succeeded)
            return errorBuilder.ToString();

        errorBuilder.AppendLine(Resource.Invalid_login_attempt);

        errorBuilder.AppendLine("||");

        return errorBuilder.ToString();
    }

    #endregion Public Methods
}