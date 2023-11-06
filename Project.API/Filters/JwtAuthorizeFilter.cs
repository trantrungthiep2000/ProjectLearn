using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Project.API.Filters;

/// <summary>
/// Information of json web token authorize filter
/// CreatedBy: ThiepTT(06/11/2023)
/// </summary>
public class JwtAuthorizeFilter : IAuthorizationFilter
{
    public string RoleName { get; set; }

    public JwtAuthorizeFilter(string roleName)
    {
        RoleName = roleName;
    }

    /// <summary>
    /// On authorization
    /// </summary>
    /// <param name="context">AuthorizationFilterContext</param>
    /// CreatedBy: ThiepTT(06/11/2023)
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!CanAccessToAction(context.HttpContext))
            context.Result = new ForbidResult();
    }

    /// <summary>
    /// Can access to action
    /// </summary>
    /// <param name="httpContext">HttpContext</param>
    /// <returns>True | False</returns>
    /// CreatedBy: ThiepTT(06/11/2023)
    private bool CanAccessToAction(HttpContext httpContext)
    {
        var roles = httpContext.User.FindFirstValue(ClaimTypes.Role);

        if (roles!.Equals(RoleName))
            return true;

        return false;
    }
}