using Microsoft.AspNetCore.Mvc;
using Project.API.Options;
using Project.Application.Models;

namespace Project.API.Controllers;

/// <summary>
/// Information of base controller
/// CreatedBy: ThiepTT(31/10/2023)
/// </summary>
public class BaseController : ControllerBase
{
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
}