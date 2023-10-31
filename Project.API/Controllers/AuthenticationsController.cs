using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Project.API.Options;
using Project.Application.Dtos.Requests;
using Project.Application.Identities.Commands;

namespace Project.API.Controllers;

/// <summary>
/// Information of authentication controller
/// CreatedBy: ThiepTT(31/10/2023)
/// </summary>
[Route($"{ApiRoutes.BaseRoute}")]
[ApiController]
public class AuthenticationsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AuthenticationsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
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
        var command = _mapper.Map<RegisterCommand>(registerRequest);

        var response = await _mediator.Send(command, cancellationToken);

        if (response.IsError)
            return HandlerErrorResponse(response.Errors);

        return Ok(response);
    }
}