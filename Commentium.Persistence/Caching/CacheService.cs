using Commentium.Application.Abstractions.Caching;
using Commentium.Persistence.Helpers;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace Commentium.Persistence.Caching
{
    public class CacheService : ICacheService
    {
        private static readonly ConcurrentDictionary<string, bool> CacheKeys = new();
        private readonly IDistributedCache _distributedCache;
        private readonly PrivateResolver _privateResolver = new();

        public CacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
            where T : class
        {
            string? cachedValue = await _distributedCache.GetStringAsync(
                key,
                cancellationToken);

            if (cachedValue is null)
            {
                return null;
            }

            T? value = JsonConvert.DeserializeObject<T>(
                cachedValue,
                new JsonSerializerSettings
                {
                    ConstructorHandling =
                        ConstructorHandling.AllowNonPublicDefaultConstructor,
                    ContractResolver = _privateResolver
                });

            return value;
        }

        public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default)
            where T : class
        {
            string cacheValue = JsonConvert.SerializeObject(value);

            await _distributedCache.SetStringAsync(key, cacheValue, CacheOption.DefaultExpiration, cancellationToken);

            CacheKeys.TryAdd(key, false);
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            await _distributedCache.RemoveAsync(key, cancellationToken);

            CacheKeys.TryRemove(key, out bool _);
        }

        public async Task RemoveByPrefixAsync(string prefixKey, CancellationToken cancellationToken = default)
        {
            foreach (string key in CacheKeys.Keys)
            {
                if (key.StartsWith(prefixKey))
                {
                    await RemoveAsync(key, cancellationToken);
                }
            }
        }
    }
}
