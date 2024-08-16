namespace OttApiPlatform.Infrastructure.Identity.Validators;

/// <summary>
/// Custom implementation of IPasswordValidator that validates if the password contains the
/// username. or the word 'password'.
/// </summary>
/// <typeparam name="TUser">The type of user object.</typeparam>
public class CustomPasswordValidator<TUser> : IPasswordValidator<TUser> where TUser : class
{
    #region Public Methods

    /// <summary>
    /// Validates the password for the specified user.
    /// </summary>
    /// <param name="manager">The user manager.</param>
    /// <param name="user">The user object.</param>
    /// <param name="password">The password to validate.</param>
    /// <returns>An IdentityResult that represents the outcome of the password validation.</returns>
    public async Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user, string password)
    {
        var username = await manager.GetUserNameAsync(user);

        // Check if the password contains the username.
        if (username == password)
            return IdentityResult.Failed(new IdentityError
            {
                Code = "InvalidPassword",
                Description = Resource.Password_cannot_contain_username
            });

        // Check if the password contains the word 'password'.
        return password.Contains("password") ? IdentityResult.Failed(new IdentityError
        {
            Code = "InvalidPassword",
            Description = Resource.Password_cannot_contain_password
        }) : IdentityResult.Success;
    }

    #endregion Public Methods
}