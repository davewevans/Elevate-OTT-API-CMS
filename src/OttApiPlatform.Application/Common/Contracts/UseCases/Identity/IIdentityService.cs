namespace OttApiPlatform.Application.Common.Contracts.UseCases.Identity;

public interface IIdentityService
{
    #region Public Methods

    Task<string> GetUserNameAsync(string userId);

    #endregion Public Methods
}