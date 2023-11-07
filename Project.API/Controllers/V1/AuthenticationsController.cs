using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Project.API.Options;
using Project.Application.Dtos.Requests;
using Project.Application.Identities.Commands;
using Project.Application.Models;
using Project.Application.Services.IServices;

namespace Project.API.Controllers.V1;

/// <summary>
/// Information of authentication controller
/// CreatedBy: ThiepTT(31/10/2023)
/// </summary>
[ApiVersion($"{ApiRoutes.Version.V1}")]
[Route($"{ApiRoutes.BaseRouter}")]
[ApiController]
public class AuthenticationsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IResponseCacheService _responseCacheService;

    /// <summary>
    /// Authentications controller
    /// </summary>
    /// <param name="mediator">IMediator</param>
    /// <param name="mapper">IMapper</param>
    /// <param name="responseCacheService">IResponseCacheService</param>
    /// CreatedBy: ThiepTT(31/10/2023)
    public AuthenticationsController(IMediator mediator, IMapper mapper, IResponseCacheService responseCacheService)
    {
        _mediator = mediator;
        _mapper = mapper;
        _responseCacheService = responseCacheService;
    }

    /// <summary>
    /// Register
    /// </summary>
    /// <param name="registerRequest">RegisterRequest</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>IActionResult</returns>
    /// CreatedBy: ThiepTT(31/10/2023)
    [HttpPost]
    [Route($"{ApiRoutes.Authentication.Register}")]
    public async Task<IActionResult> Register(RegisterRequest registerRequest, CancellationToken cancellationToken)
    {
        RegisterCommand command = _mapper.Map<RegisterCommand>(registerRequest);

        OperationResult<string> response = await _mediator.Send(command, cancellationToken);

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        string pattern = SystemConfig.GeneratePattern(
            ControllerContext.ActionDescriptor.ControllerTypeInfo.Name,
            $"{ApiRoutes.Api}",
            $"{ApiRoutes.UserProfile.GetAllUserProfiles}");

        await _responseCacheService.RemoveCacheResponseAsync(pattern);

        return Ok(response);
    }

    /// <summary>
    /// Login
    /// </summary>
    /// <param name="loginRequest">LoginRequest</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>IActionResult</returns>
    /// CreatedBy: ThiepTT(01/11/2023)
    [HttpPost]
    [Route($"{ApiRoutes.Authentication.Login}")]
    public async Task<IActionResult> Login(LoginRequest loginRequest, CancellationToken cancellationToken)
    {
        LoginCommand command = _mapper.Map<LoginCommand>(loginRequest);

        OperationResult<string> response = await _mediator.Send(command, cancellationToken);

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        return Ok(response);
    }
}