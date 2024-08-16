namespace OttApiPlatform.Infrastructure.Services
{
    public class MemoryCacheService : ICacheService
    {
        #region Private Fields

        private readonly IMemoryCache _cache;

        #endregion Private Fields

        #region Public Constructors

        public MemoryCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        #endregion Public Constructors

        #region Public Methods

        public T Get<T>(string key)
        {
            return _cache.Get<T>(key);
        }

        public void Set<T>(string key, T value, TimeSpan absoluteExpirationRelativeToNow)
        {
            _cache.Set(key, value, absoluteExpirationRelativeToNow);
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        #endregion Public Methods
    }
}