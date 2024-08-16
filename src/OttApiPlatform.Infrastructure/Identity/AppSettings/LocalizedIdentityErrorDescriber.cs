namespace OttApiPlatform.Infrastructure.Identity.AppSettings;

public class LocalizedIdentityErrorDescriber : IdentityErrorDescriber
{
    #region Public Methods

    public override IdentityError DefaultError() =>
        new() { Code = nameof(DefaultError), Description = Resource.An_unknown_failure_has_occurred };

    public override IdentityError ConcurrencyFailure() =>
        new() { Code = nameof(ConcurrencyFailure), Description = Resource.Optimistic_concurrency_failure };

    public override IdentityError PasswordMismatch() =>
        new() { Code = nameof(PasswordMismatch), Description = Resource.Incorrect_password };

    public override IdentityError InvalidToken() =>
        new() { Code = nameof(InvalidToken), Description = Resource.Invalid_token };

    public override IdentityError RecoveryCodeRedemptionFailed() =>
        new() { Code = nameof(InvalidToken), Description = Resource.Recovery_Code_Redemption_Failed };

    public override IdentityError LoginAlreadyAssociated() =>
        new() { Code = nameof(LoginAlreadyAssociated), Description = Resource.A_user_with_this_login_already_exists };

    public override IdentityError InvalidUserName(string userName) =>
        new() { Code = nameof(InvalidUserName), Description = string.Format(Resource.Username_is_invalid, userName) };

    public override IdentityError InvalidEmail(string email) =>
        new() { Code = nameof(InvalidEmail), Description = string.Format(Resource.Email_is_invalid, email) };

    public override IdentityError DuplicateUserName(string userName) =>
        new() { Code = nameof(DuplicateUserName), Description = string.Format(Resource.Username_is_already_taken, userName) };

    public override IdentityError DuplicateEmail(string email) =>
        new() { Code = nameof(DuplicateEmail), Description = string.Format(Resource.Email_is_already_taken, email) };

    public override IdentityError InvalidRoleName(string role) =>
        new() { Code = nameof(InvalidRoleName), Description = string.Format(Resource.Role_name_is_invalid, role) };

    public override IdentityError DuplicateRoleName(string role) =>
        new() { Code = nameof(DuplicateRoleName), Description = string.Format(Resource.Role_name_is_already_taken, role) };

    public override IdentityError UserAlreadyHasPassword() =>
        new() { Code = nameof(UserAlreadyHasPassword), Description = Resource.User_already_has_a_password_set };

    public override IdentityError UserLockoutNotEnabled() =>
        new() { Code = nameof(UserLockoutNotEnabled), Description = Resource.Lockout_is_not_enabled_for_this_user };

    public override IdentityError UserAlreadyInRole(string role) =>
        new() { Code = nameof(UserAlreadyInRole), Description = string.Format(Resource.User_already_in_role, role) };

    public override IdentityError PasswordTooShort(int length) =>
        new() { Code = nameof(PasswordTooShort), Description = string.Format(Resource.Passwords_must_be_at_least_length_characters, length) };

    public override IdentityError PasswordRequiresNonAlphanumeric() =>
        new() { Code = nameof(PasswordRequiresNonAlphanumeric), Description = Resource.Passwords_must_have_at_least_one_non_alphanumeric_character };

    public override IdentityError PasswordRequiresDigit() =>
        new() { Code = nameof(PasswordRequiresDigit), Description = Resource.Passwords_must_have_at_least_one_digit };

    public override IdentityError PasswordRequiresLower() =>
        new() { Code = nameof(PasswordRequiresLower), Description = Resource.Passwords_must_have_at_least_one_lowercase };

    public override IdentityError PasswordRequiresUpper() =>
        new() { Code = nameof(PasswordRequiresUpper), Description = Resource.Passwords_must_have_at_least_one_uppercase };

    #endregion Public Methods
}