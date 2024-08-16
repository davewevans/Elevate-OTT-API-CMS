namespace OttApiPlatform.Application.Common.Contracts.Infrastructure
{
    public interface ICacheService
    {
        #region Public Methods

        T Get<T>(string key);

        void Set<T>(string key, T value, TimeSpan absoluteExpirationRelativeToNow);

        void Remove(string key);

        #endregion Public Methods
    }
}