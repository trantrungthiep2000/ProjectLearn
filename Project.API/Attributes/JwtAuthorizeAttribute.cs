using Microsoft.AspNetCore.Mvc;
using Project.API.Filters;

namespace Project.API.Attributes;

/// <summary>
/// Information of json web token authorize attribute
/// CreatedBy: ThiepTT(06/11/2023)
/// </summary>
public class JwtAuthorizeAttribute : TypeFilterAttribute
{
    /// <summary>
    /// Role name
    /// </summary>
    public string RoleName { get; set; }

    /// <summary>
    /// Json web token authorize attribute
    /// </summary>
    /// <param name="roleName">Role name</param>
    /// CreatedBy: ThiepTT(06/11/2023)
    public JwtAuthorizeAttribute(string roleName) : base(typeof(JwtAuthorizeFilter))
    {
        RoleName = roleName;
        Arguments = new object[] { RoleName };
    }
}