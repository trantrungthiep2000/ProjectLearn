using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Project.API.Options;
using Project.Application.Models;

namespace Project.API.Attributes;

/// <summary>
/// Information of validate guid attribute
/// CreatedBy: ThiepTT(03/11/2023)
/// </summary>
public class ValidateGuidAttribute : Attribute, IAsyncActionFilter
{
    private readonly List<string> _keys;

    public ValidateGuidAttribute(string key)
    {
        _keys = new List<string>();
        _keys.Add(key); 
    }

    /// <summary>
    /// On action execution async
    /// </summary>
    /// <param name="context">ActionExecutingContext</param>
    /// <param name="next">ActionExecutionDelegate</param>
    /// <returns>Task</returns>
    /// CreatedBy: ThiepTT(03/11/2023)
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        bool isCheck = false;
        ErrorResponse errorResponse = new ErrorResponse();

        foreach (string key in _keys)
        {
            if (!context.ActionArguments.TryGetValue(key, out object? value)
                || !Guid.TryParse(value?.ToString(), out Guid guid))
            {
                isCheck = true;
                errorResponse.Errors.Add($"The identity for {key} is not correct Guid format");
            }
        }

        if (isCheck)
        {
            errorResponse.StatusCode = (int)ErrorCode.BadRequest;
            errorResponse.StatusPhrase = SystemConfig.BadRequest;
            errorResponse.TimeStamp = DateTime.UtcNow;
            context.HttpContext.Response.ContentType = $"{SystemConfig.ApplicationJson}";
            context.HttpContext.Response.StatusCode = (int)ErrorCode.BadRequest;

            await context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
            return;
        }

        await next();
        return;
    }
}
