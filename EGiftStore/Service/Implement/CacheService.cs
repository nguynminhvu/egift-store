using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Service.Interface;
using StackExchange.Redis;

namespace Service.Implement
{
    public class CacheService : ICacheService
    {
        private IDistributedCache _distributedCache;
        private IConnectionMultiplexer _connectionMutiplexer;

        public CacheService(IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer)
        {
            _distributedCache = distributedCache;
            _connectionMutiplexer = connectionMultiplexer;
        }

        public async Task<string> GetCacheAsync(string cacheKey)
        {
            var cacheResponse = await _distributedCache.GetStringAsync(cacheKey);
            return !string.IsNullOrWhiteSpace(cacheResponse) ? cacheResponse! : null!;
        }

        public async Task SetCacheAsync(string cacheKey, object obj, TimeSpan timeSpan)
        {
            if (obj is null)
            {
                return;
            }
            var serializerResponse = JsonConvert.SerializeObject(obj, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            await _distributedCache.SetStringAsync(cacheKey, serializerResponse, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = timeSpan });
        }

        public async Task RemoveCacheAsync(string pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern))
            {
                throw new ArgumentException("Pattern cannot be null or empty");
            }
            foreach (var endPoint in _connectionMutiplexer.GetEndPoints())
            {
                var server = _connectionMutiplexer.GetServer(endPoint);
                await foreach (var key in server.KeysAsync(pattern: pattern))
                {
                    await _distributedCache.RemoveAsync(key.ToString());
                }
            }
        }
    }
}
