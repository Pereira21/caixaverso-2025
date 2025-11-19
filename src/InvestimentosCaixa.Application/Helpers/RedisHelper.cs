using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace InvestimentosCaixa.Application.Helpers
{
    public static class RedisHelper
    {
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            IncludeFields = true,
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        public static async Task<T> GetOrSetCacheAsync<T>(IDistributedCache distributedCache, string key, Func<Task<T>> loadFromDb)
        {
            var cachedBytes = await distributedCache.GetAsync(key);
            if (cachedBytes != null && cachedBytes.Length > 0)
            {
                try
                {
                    var fromCache = JsonSerializer.Deserialize<T>(cachedBytes, _jsonOptions);
                    if (fromCache != null) return fromCache;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Cache Deserialize Error] Key={key} Exception={ex.Message}");
                    await distributedCache.RemoveAsync(key);
                }
            }

            var data = await loadFromDb();

            try
            {
                var bytes = JsonSerializer.SerializeToUtf8Bytes(data, _jsonOptions);

                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12)
                };

                await distributedCache.SetAsync(key, bytes, options);

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Cache Set Error] Key={key} Exception={ex.Message}");
                return data;
            }
        }
    }

}
