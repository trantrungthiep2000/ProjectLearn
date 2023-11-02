using Project.API.Options;
using System.Security.Claims;

namespace Project.API.Extenstions;

/// <summary>
/// Information of http context extention
/// CreatedBy: ThiepTT(02/11/2023)
/// </summary>
public static class HttpContextExtention
{
    /// <summary>
    /// Get id of user profile
    /// </summary>
    /// <param name="context">HttpContext</param>
    /// <returns>Id of user profile</returns>
    /// CreatedBy: ThiepTT(02/11/2023)
    public static Guid GetUserProfileId(this HttpContext context)
    {
        return Guid.Parse(GetDataClaimValue($"{SystemConfig.UserProfileId}", context));
    }

    /// <summary>
    /// Get data claim value
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="context">HttpContext</param>
    /// <returns>Data</returns>
    /// CreatedBy: ThiepTT(02/11/2023)
    private static string GetDataClaimValue(string key, HttpContext context)
    {
        ClaimsIdentity? identity = context.User.Identity as ClaimsIdentity;

        string data = string.Empty;

        if (identity is not null)
        {
            data = identity.FindFirst(key)!.Value;
        }    

        return data;
    }
}