using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.API.Options;
using Project.Application.Models;
using System.Security.Claims;

namespace Project.API.Controllers;

/// <summary>
/// Information of base controller
/// CreatedBy: ThiepTT(31/10/2023)
/// </summary>
public class BaseController : ControllerBase
{
    /// <summary>
    /// id of user profile
    /// </summary>
    protected Guid UserProfileId = Guid.Empty;

    /// <summary>
    /// Full name
    /// </summary>
    protected string FullName = string.Empty;

    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Information of base controller
    /// CreatedBy: ThiepTT(24/11/2023)
    /// </summary>
    public BaseController()
    {
        _httpContextAccessor = new HttpContextAccessor();

        if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User.Identity!.IsAuthenticated)
        {
            UserProfileId = GetUserProfileId(_httpContextAccessor.HttpContext!);
            FullName = GetFullName(_httpContextAccessor.HttpContext!);
        }
    }

    /// <summary>
    /// Get id of user profile
    /// </summary>
    /// <param name="httpContext">HttpContext</param>
    /// <returns>Id of user profile</returns>
    /// CreatedBy: ThiepTT(02/11/2023)
    protected Guid GetUserProfileId(HttpContext httpContext)
    {
        return Guid.Parse(GetDataClaimValue($"{SystemConfig.UserProfileId}", httpContext));
    }

    /// <summary>
    /// Get full name 
    /// </summary>
    /// <param name="httpContext">HttpContext</param>
    /// <returns>Full name</returns>
    /// CreatedBy: ThiepTT(08/11/2023)
    protected string GetFullName(HttpContext httpContext)
    {
        return GetDataClaimValue($"{SystemConfig.FullName}", httpContext).ToString();
    }

    /// <summary>
    /// Handler error response
    /// </summary>
    /// <param name="errors">List of error</param>
    /// <returns>IActionResult</returns>
    /// CreatedBy: ThiepTT(31/10/2023)
    protected IActionResult HandlerErrorResponse(List<Error> errors)
    {
        var errorResponse = new ErrorResponse();

        if (errors.Any(error => error.Code == ErrorCode.BadRequest))
        {
            errorResponse = ConfigErrorResponse(errorResponse, errors, (int)ErrorCode.BadRequest, SystemConfig.BadRequest);

            return BadRequest(errorResponse);
        }

        if (errors.Any(error => error.Code == ErrorCode.NotFound))
        {
            errorResponse = ConfigErrorResponse(errorResponse, errors, (int)ErrorCode.NotFound, SystemConfig.NotFound);

            return NotFound(errorResponse);
        }

        errorResponse = ConfigErrorResponse(errorResponse, errors, (int)ErrorCode.InternalServerError, SystemConfig.InternalServerError);

        return StatusCode((int)ErrorCode.InternalServerError, errorResponse);
    }

    /// <summary>
    /// Config error response
    /// </summary>
    /// <param name="errorResponse">Error response</param>
    /// <param name="errors">List of error</param>
    /// <param name="statusCode">Status code</param>
    /// <param name="statusPhrase">Status phrase</param>
    /// <returns></returns>
    private static ErrorResponse ConfigErrorResponse(ErrorResponse errorResponse, List<Error> errors, int statusCode, string statusPhrase)
    {
        errorResponse.StatusCode = statusCode;
        errorResponse.StatusPhrase = statusPhrase;
        errorResponse.TimeStamp = DateTime.UtcNow;

        foreach (var error in errors)
        {
            errorResponse.Errors.Add(error.Message);
        }

        return errorResponse;
    }

    /// <summary>
    /// Get data claim value
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="context">HttpContext</param>
    /// <returns>Data</returns>
    /// CreatedBy: ThiepTT(02/11/2023)
    private string GetDataClaimValue(string key, HttpContext context)
    {
        ClaimsIdentity? identity = context.User.Identity as ClaimsIdentity;

        string data = string.Empty;

        if (identity is not null)
        {
            if (identity.FindFirst(key) is not null)
            {
                data = identity.FindFirst(key)!.Value;
            }
        }

        return data;
    }
}