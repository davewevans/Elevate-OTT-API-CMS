namespace OttApiPlatform.Application.Common.Extensions;

public static class IdentityResultExtensions
{
    /// <summary>
    /// Converts a collection of <see cref="IdentityError"/> instances to a <see
    /// cref="Dictionary{TKey,TValue}"/> instance.
    /// </summary>
    /// <param name="identityErrors">
    /// The collection of <see cref="IdentityError"/> instances to convert.
    /// </param>
    /// <returns>
    /// A <see cref="Dictionary{TKey,TValue}"/> instance representing the <paramref name="identityErrors"/>.
    /// </returns>

    #region Public Methods

    public static Dictionary<string, string> ToApplicationResult(this IEnumerable<IdentityError> identityErrors)
    {
        return identityErrors.ToDictionary(identityError => identityError.Code, identityError => identityError.Description);
    }

    /// <summary>
    /// Converts a <see cref="SignInResult"/> instance to a serialized string.
    /// </summary>
    /// <param name="signInResult">The <see cref="SignInResult"/> instance to convert.</param>
    /// <returns>A serialized string representation of the <paramref name="signInResult"/>.</returns>

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