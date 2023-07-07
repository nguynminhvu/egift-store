namespace Service.Interface
{
    public interface ICacheService
    {
        Task SetCacheAsync(string cacheKey, object obj, TimeSpan timeSpan);
        Task<string> GetCacheAsync(string cacheKey);
        Task RemoveCacheAsync(string pattern);
    }
}
