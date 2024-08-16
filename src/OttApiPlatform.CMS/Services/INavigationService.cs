namespace OttApiPlatform.CMS.Services;

public interface INavigationService
{
    #region Public Methods

    Task NavigateToUrlAsync(string url, bool openInNewTab);

    Task NavigateToTenantPortalAsync(string tenantName, bool redirectToRegistration, bool openInNewTab);

    #endregion Public Methods
}