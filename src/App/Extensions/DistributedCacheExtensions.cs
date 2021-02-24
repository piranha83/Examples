using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace App.Extensions
{
    public static class DistributedCacheExtensions
    {   
        public static async Task<TEntity> GetOrCreateAsync<TEntity>(this IDistributedCache distributedCache, string key, Func<Task<TEntity>> init, TimeSpan timeout)
        where TEntity: class
        {
            var cache = await distributedCache.GetStringAsync(key);
            if(cache != null) return JsonConvert.DeserializeObject<TEntity>(cache);
            
            TEntity val = await init();
            var options = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = timeout };
            cache = JsonConvert.SerializeObject(val);
            await distributedCache.SetStringAsync(key, cache, options, CancellationToken.None);

            return val;
        }
    }
}