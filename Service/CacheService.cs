using DomainLayer.Contracts;
using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Service
{
    public class CacheService (ICacheReposatory _cacheReposatory): ICacheService
    {
        public async Task<string?> GetAsync(string CacheKey)
        => await _cacheReposatory.GetAsync(CacheKey);

        public async Task SetAsync(string CacheKey, object CacheValue, TimeSpan TimeToLive)
        {
            var val=JsonSerializer.Serialize(CacheValue);
             await _cacheReposatory.SetAsync(CacheKey, val, TimeToLive);
        }
    }
}
