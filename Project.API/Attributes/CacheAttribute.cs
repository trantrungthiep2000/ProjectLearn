using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Project.API.Options;
using Project.Application.Models;
using Project.Domain.Interfaces.IServices;
using System.Text;

namespace Project.API.Attributes;

/// <summary>
/// Information of cache attribute
/// CreatedBy: ThiepTT(06/11/2023)
/// </summary>
public class CacheAttribute : Attribute, IAsyncActionFilter
{
    private readonly int _timeToLiveSeconds;

    /// <summary>
    /// Cache attribute
    /// </summary>
    /// <param name="timeToLiveSeconds">Time to live seconds</param>
    /// CreatedBy: ThiepTT(06/11/2023)
    public CacheAttribute(int timeToLiveSeconds = 1000)
    {
        _timeToLiveSeconds = timeToLiveSeconds;
    }

    /// <summary>
    /// On action execution async
    /// </summary>
    /// <param name="context">ActionExecutingContext</param>
    /// <param name="next">ActionExecutionDelegate</param>
    /// <returns>Task</returns>
    /// CreatedBy: ThiepTT(06/11/2023)
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var cacheConfiguration = context.HttpContext.RequestServices.GetRequiredService<RedisConfiguration>();

        if (!cacheConfiguration.Enabled)
        {
            await next();
            return;
        }

        var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();

        var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

        var cacheResponse = await cacheService.GetCacheResponseAsync(cacheKey);

        // check cache is not null
        if (!string.IsNullOrWhiteSpace(cacheResponse))
        {
            var contentResult = new ContentResult
            {
                Content = cacheResponse,
                ContentType = $"{SystemConfig.ApplicationJson}",
                StatusCode = 200
            };

            context.Result = contentResult;
            return;
        }

        var executedContext = await next();

        if (executedContext.Result is OkObjectResult objectResult)
            await cacheService.SetCacheResponseAsync(cacheKey, objectResult.Value!, TimeSpan.FromSeconds(_timeToLiveSeconds));
    }

    /// <summary>
    /// Generate cache key from request
    /// </summary>
    /// <param name="request">HttpRequest</param>
    /// <returns>string</returns>
    /// CreatedBy: ThiepTT(06/11/2023)
    private static string GenerateCacheKeyFromRequest(HttpRequest request)
    {
        var keyBuilder = new StringBuilder();
        keyBuilder.Append($"{request.Path}");

        foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
        {
            keyBuilder.Append($"|{key}-{value}");
        }

        return keyBuilder.ToString();
    }
}