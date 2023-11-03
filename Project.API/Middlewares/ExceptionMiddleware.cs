﻿using Newtonsoft.Json;
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

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Invoke async
    /// </summary>
    /// <param name="context">HttpContext</param>
    /// <returns>Task</returns>
    /// CreatedBy: ThiepTT(02/11/2023)
    public async Task InvokeAsync(HttpContext context)
    {
        ErrorResponse errorResponse = new ErrorResponse();

        try
        {
            await _next(context);

            if (context.Response.StatusCode == (int)ErrorCode.Unauthorized)
            {
                errorResponse.StatusCode = (int)ErrorCode.Unauthorized;
                errorResponse.StatusPhrase = SystemConfig.Unauthorized;
                errorResponse.TimeStamp = DateTime.UtcNow;
                errorResponse.Errors.Add(SystemConfig.Unauthorized);
                context.Response.ContentType = $"{SystemConfig.ApplicationJson}";
                context.Response.StatusCode = (int)ErrorCode.Unauthorized;

                await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
            }

            if (context.Response.StatusCode == (int)ErrorCode.Forbidden)
            {
                errorResponse.StatusCode = (int)ErrorCode.Forbidden;
                errorResponse.StatusPhrase = SystemConfig.Forbidden;
                errorResponse.TimeStamp = DateTime.UtcNow;
                errorResponse.Errors.Add(SystemConfig.Forbidden);
                context.Response.ContentType = $"{SystemConfig.ApplicationJson}";
                context.Response.StatusCode = (int)ErrorCode.Forbidden;

                await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
            }
        }
        catch (Exception ex)
        {
            errorResponse.StatusCode = (int)ErrorCode.InternalServerError;
            errorResponse.StatusPhrase = SystemConfig.InternalServerError;
            errorResponse.TimeStamp = DateTime.UtcNow;
            errorResponse.Errors.Add(ex.Message);
            context.Response.ContentType = $"{SystemConfig.ApplicationJson}";
            context.Response.StatusCode = (int)ErrorCode.InternalServerError;

            await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
        }
    }
}