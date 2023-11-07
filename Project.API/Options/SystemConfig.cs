using Microsoft.AspNetCore.Routing;

namespace Project.API.Options;

/// <summary>
/// Information of system config
/// CreatedBy: ThiepTT(31/10/2023)
/// </summary>
public static class SystemConfig
{
    /// <summary>
    /// Bad request
    /// </summary>
    public const string BadRequest = "Bad request";

    /// <summary>
    /// Unauthorized
    /// </summary>
    public const string Unauthorized = "Unauthorized";

    /// <summary>
    /// Forbidden
    /// </summary>
    public const string Forbidden = "Forbidden";

    /// <summary>
    /// Not found
    /// </summary>
    public const string NotFound = "Not found";

    /// <summary>
    /// Internal server error
    /// </summary>
    public const string InternalServerError = "Internal server error";

    /// <summary>
    /// Id of user profile
    /// </summary>
    public const string UserProfileId = "UserProfileId";

    /// <summary>
    /// Application/json
    /// </summary>
    public const string ApplicationJson = "application/json";

    /// <summary>
    /// Controller
    /// </summary>
    public const string Controller = "Controller";

    /// <summary>
    /// Title
    /// </summary>
    public const string Title = "Project";

    /// <summary>
    /// Description
    /// </summary>
    public const string Description = "This API version has been deprecated";

    /// <summary>
    /// Generate pattern
    /// </summary>
    /// <param name="controllerName">Name of controller</param>
    /// <param name="baseRoute">Base route</param>
    /// <param name="action">Action</param>
    /// <returns>Pattern</returns>
    /// CreatedBy: ThiepTT(27/10/2023)
    public static string GeneratePattern(string controllerName, string baseRoute, string action)
    {
        if (controllerName.EndsWith($"{Controller}"))
        {
            controllerName = controllerName.Substring(0, controllerName.Length - $"{Controller}".Length);
        }
        var pattern = $"/{baseRoute}/{controllerName}/{action}";

        return pattern;
    }
}