namespace Project.Application.Models;

/// <summary>
/// Information of error code
/// CreatedBy: ThiepTT(31/10/2023)
/// </summary>
public enum ErrorCode
{
    /// <summary>
    /// Bad request
    /// </summary>
    BadRequest = 400,

    /// <summary>
    /// Unauthorized
    /// </summary>
    Unauthorized = 401,

    /// <summary>
    /// Forbidden
    /// </summary>
    Forbidden = 403,

    /// <summary>
    /// Not found
    /// </summary>
    NotFound = 404,

    /// <summary>
    /// Internal server error
    /// </summary>
    InternalServerError = 500,
}