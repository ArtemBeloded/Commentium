using Microsoft.Extensions.Caching.Distributed;

namespace Commentium.Persistence.Caching
{
    public static class CacheOption
    {
        public static DistributedCacheEntryOptions DefaultExpiration =>
            new() { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60) };
    }
}
