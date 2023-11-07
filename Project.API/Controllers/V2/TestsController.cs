using Microsoft.AspNetCore.Mvc;
using Project.API.Options;

namespace Project.API.Controllers.V2;

/// <summary>
/// Information of tests controller
/// CreatedBy: ThiepT(07/11/2023)
/// </summary>
[ApiVersion($"{ApiRoutes.Version.V2}")]
[Route($"{ApiRoutes.BaseRouter}")]
[ApiController]
public class TestsController : BaseController
{
    /// <summary>
    /// Test v2
    /// </summary>
    /// <returns>IActionResult</returns>
    /// CreatedBy: ThiepT(07/11/2023)
    [HttpGet]
    [Route($"{ApiRoutes.Test.TestV2}")]
    public IActionResult TestV2()
    {
        return Ok("test api v2");
    }
}