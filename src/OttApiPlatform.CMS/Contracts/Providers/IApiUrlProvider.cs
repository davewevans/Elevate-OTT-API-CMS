namespace OttApiPlatform.CMS.Contracts.Providers;

public interface IApiUrlProvider
{
    #region Public Properties

    string BaseUrl { get; }
    string BaseHubUrl { get; }

    #endregion Public Properties
}