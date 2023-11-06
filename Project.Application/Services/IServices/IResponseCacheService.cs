namespace Project.Application.Services.IServices;

/// <summary>
/// Information of interface response cache service
/// CreatedBy: ThiepTT(06/11/2023)
/// </summary>
public interface IResponseCacheService
{
    /// <summary>
    /// Set cache response async
    /// </summary>
    /// <param name="cacheKey">Cache key</param>
    /// <param name="response">Response</param>
    /// <param name="timeOut">Time out</param>
    /// <returns>Task</returns>
    /// CreatedBy: ThiepTT(06/11/2023)
    public Task SetCacheResponseAsync(string cacheKey, object response, TimeSpan timeOut);

    /// <summary>
    /// Get cache response async
    /// </summary>
    /// <param name="cacheKey">Cache key</param>
    /// <returns>string</returns>
    /// CreatedBy: ThiepTT(06/11/2023)
    public Task<string> GetCacheResponseAsync(string cacheKey);

    /// <summary>
    /// Remove cache response async
    /// </summary>
    /// <param name="pattern">Pattern</param>
    /// <returns>Task</returns>
    /// CreatedBy: ThiepTT(06/11/2023)
    public Task RemoveCacheResponseAsync(string pattern);
}