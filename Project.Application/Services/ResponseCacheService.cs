using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Project.Application.Services.IServices;
using StackExchange.Redis;

namespace Project.Application.Services;

/// <summary>
/// Information of response cache service
/// CreatedBy: ThiepTT(06/11/2023)
/// </summary>
public class ResponseCacheService : IResponseCacheService
{
    private readonly IDistributedCache _distributedCache;
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public ResponseCacheService(IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer)
    {
        _distributedCache = distributedCache;
        _connectionMultiplexer = connectionMultiplexer;
    }

    /// <summary>
    /// Get cache response async
    /// </summary>
    /// <param name="cacheKey">Cache key</param>
    /// <returns>string</returns>
    /// CreatedBy: ThiepTT(06/11/2023)
    public async Task<string> GetCacheResponseAsync(string cacheKey)
    {
        var cacheResponse = await _distributedCache.GetStringAsync(cacheKey);

        return string.IsNullOrWhiteSpace(cacheResponse) ? string.Empty : cacheResponse;
    }

    /// <summary>
    /// Set cache response async
    /// </summary>
    /// <param name="cacheKey">Cache key</param>
    /// <param name="response">Response</param>
    /// <param name="timeOut">Time out</param>
    /// <returns>Task</returns>
    /// CreatedBy: ThiepTT(06/11/2023)
    public async Task SetCacheResponseAsync(string cacheKey, object response, TimeSpan timeOut)
    {
        if (response is null)
            return;

        var serializerResponse = JsonConvert.SerializeObject(response, new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        });

        await _distributedCache.SetStringAsync(cacheKey, serializerResponse, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = timeOut
        });
    }

    /// <summary>
    /// Remove cache response async
    /// </summary>
    /// <param name="pattern">Pattern</param>
    /// <returns>Task</returns>
    /// CreatedBy: ThiepTT(06/11/2023)
    public async Task RemoveCacheResponseAsync(string pattern)
    {
        await foreach (var key in GetKeyAsync(pattern + "*"))
        {
            await _distributedCache.RemoveAsync(key);
        }
    }

    /// <summary>
    /// Get key async 
    /// </summary>
    /// <param name="pattern">Pattern</param>
    /// <returns>string</returns>
    /// <exception cref="ArgumentException">ArgumentException</exception>
    /// CreatedBy: ThiepTT(06/11/2023)
    private async IAsyncEnumerable<string> GetKeyAsync(string pattern)
    {
        if (string.IsNullOrWhiteSpace(pattern))
            throw new ArgumentException("Value cannot be null or whitespace");

        foreach (var endPoint in _connectionMultiplexer.GetEndPoints())
        {
            var server = _connectionMultiplexer.GetServer(endPoint);

            await foreach (var key in server.KeysAsync(pattern: pattern))
            {
                yield return key.ToString();
            }
        }
    }
}