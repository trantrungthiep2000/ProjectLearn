using Newtonsoft.Json;
using Project.API.Options;
using Project.Application.Models;

namespace Project.API.Middlewares;

/// <summary>
/// Information of exception middleware
/// CreatedBy: ThiepTT(02/11/2023)
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    /// <summary>
    /// Exception middleware
    /// </summary>
    /// <param name="next">RequestDelegate</param>
    /// <param name="logger">ILogger</param>
    /// CreatedBy: ThiepTT(02/11/2023)
    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Invoke async
    /// </summary>
    /// <param name="context">HttpContext</param>
    /// <returns>Task</returns>
    /// CreatedBy: ThiepTT(02/11/2023)
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);

            if (context.Response.StatusCode == (int)ErrorCode.Unauthorized)
            {
                await HandleExceptionAsync(context, SystemConfig.Unauthorized, (int)ErrorCode.Unauthorized, SystemConfig.Unauthorized);
            }

            if (context.Response.StatusCode == (int)ErrorCode.Forbidden)
            {
                await HandleExceptionAsync(context, SystemConfig.Forbidden, (int)ErrorCode.Forbidden, SystemConfig.Forbidden);
            }
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"\nError: \"{ex}\"\n");
            File.AppendAllText("log_error.txt", $"\nError: \"{ex}\"\n");

            await HandleExceptionAsync(context, ex.Message, (int)ErrorCode.InternalServerError, SystemConfig.InternalServerError);
        }
    }

    /// <summary>
    /// Handle exception async
    /// </summary>
    /// <param name="context">HttpContext</param>
    /// <param name="errorMessage">Error message</param>
    /// <param name="statusCode">Status code</param>
    /// <param name="statusPhrase">Status phrase</param>
    /// <returns>Task</returns>
    /// CreatedBy: ThiepTT(08/11/2023)
    private async Task HandleExceptionAsync(HttpContext context, string errorMessage, int statusCode, string statusPhrase)
    {
        ErrorResponse errorResponse = new ErrorResponse();
        errorResponse.StatusCode = statusCode;
        errorResponse.StatusPhrase = statusPhrase;
        errorResponse.TimeStamp = DateTime.UtcNow;
        errorResponse.Errors.Add(errorMessage);

        context.Response.ContentType = $"{SystemConfig.ApplicationJson}";
        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
    }
}