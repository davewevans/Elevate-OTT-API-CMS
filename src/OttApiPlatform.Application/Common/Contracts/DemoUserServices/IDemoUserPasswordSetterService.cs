namespace OttApiPlatform.Application.Common.Contracts.DemoUserServices;

public interface IDemoUserPasswordSetterService
{
    #region Public Methods

    Task ResetDefaultPassword(string currentPassword, ApplicationUser user);

    #endregion Public Methods
}