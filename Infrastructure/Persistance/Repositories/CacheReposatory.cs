using DomainLayer.Contracts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories
{
    public class CacheReposatory(IConnectionMultiplexer connection) : ICacheReposatory
    {
        readonly IDatabase _database = connection.GetDatabase();
        public async Task<string?> GetAsync(string CacheKey)
        {
            var value = await _database.StringGetAsync(CacheKey);
            return value.IsNullOrEmpty? null:value.ToString();
        }

        public async Task SetAsync(string CacheKey, string CacheValue, TimeSpan TimeToLive)
        {
            await _database.StringSetAsync(CacheKey, CacheValue, TimeToLive);
        }
    }
}
