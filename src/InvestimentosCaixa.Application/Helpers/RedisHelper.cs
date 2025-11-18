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
            // tenta ler como bytes (mais robusto)
            var cachedBytes = await distributedCache.GetAsync(key);
            if (cachedBytes != null && cachedBytes.Length > 0)
            {
                try
                {
                    var fromCache = JsonSerializer.Deserialize<T>(cachedBytes, _jsonOptions);
                    if (fromCache != null) return fromCache;
                    // se veio nulo, vamos prosseguir para recarregar (evita ficar com uma entrada "corrompida")
                }
                catch (Exception ex)
                {
                    // Logar o erro real no seu logger (ex: ILogger). Não lançar para não quebrar a aplicação.
                    Console.WriteLine($"[Cache Deserialize Error] Key={key} Exception={ex.Message}");
                    // opcional: distributedCache.Remove(key) para remover entrada corrompida
                    await distributedCache.RemoveAsync(key);
                }
            }

            // Cache miss ou desserialização falhou -> carregar do banco
            var data = await loadFromDb();

            try
            {
                var bytes = JsonSerializer.SerializeToUtf8Bytes(data, _jsonOptions);

                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12)
                };

                await distributedCache.SetAsync(key, bytes, options);

                // opcional: log do JSON para debug (cuidado em produção com dados sensíveis)
                // Console.WriteLine($"[Cache Set] Key={key} Size={bytes.Length} bytes");

                return data;
            }
            catch (Exception ex)
            {
                // se a serialização falhar, só retornamos os dados carregados (mas não cacheamos)
                Console.WriteLine($"[Cache Set Error] Key={key} Exception={ex.Message}");
                return data;
            }
        }
    }

}
