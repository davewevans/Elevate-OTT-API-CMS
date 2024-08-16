namespace OttApiPlatform.CMS.Contracts.Services;

public interface IAuthenticationService
{
    #region Public Methods

    Task Login(AuthResponse authResponse);

    Task ReAuthenticate(AuthResponse authResponse);

    Task Logout();

    #endregion Public Methods
}