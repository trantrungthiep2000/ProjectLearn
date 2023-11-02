using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.API.Extenstions;
using Project.API.Options;
using Project.Application.Dtos.Requests;
using Project.Application.Models;
using Project.Application.Users.Commands;
using Project.Application.Users.Queries;
using Project.Domain.Aggregates;
using System.Threading;

namespace Project.API.Controllers;

/// <summary>
/// Information of user profile controller
/// CreatedBy: ThiepTT(02/11/2023)
/// </summary>
[Route($"{ApiRoutes.BaseRoute}")]
[ApiController]
[Authorize]
public class UserProfilesController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public UserProfilesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Get information of user profile
    /// </summary>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>IActionResult</returns>
    /// CreatedBy: ThiepTT(02/11/2023)
    [HttpGet]
    [Route($"{ApiRoutes.UserProfile.GetInformationOfUserProfile}")]
    public async Task<IActionResult> GetInformationOfUserProfile(CancellationToken cancellationToken)
    {
        Guid userProfileId = HttpContext.GetUserProfileId();

        GetUserProfileByIdQuery query = new GetUserProfileByIdQuery() { UserProfileId = userProfileId };

        OperationResult<UserProfile> response = await _mediator.Send(query, cancellationToken);

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        return Ok(response);
    }

    /// <summary>
    /// Update information of user profile
    /// </summary>
    /// <param name="userProfile">UserProfileRequest</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>IActionResult</returns>
    /// CreatedBy: ThiepTT(02/11/2023)
    [HttpPut]
    [Route($"{ApiRoutes.UserProfile.UpdateInformationOfUserProfile}")]
    public async Task<IActionResult> UpdateInformationOfUserProfile(UserProfileRequest userProfile, CancellationToken cancellationToken)
    {
        Guid userProfileId = HttpContext.GetUserProfileId();

        UpdateUserProfileCommand command = _mapper.Map<UpdateUserProfileCommand>(userProfile);
        command.UserProfileId = userProfileId;

        OperationResult<string> response = await _mediator.Send(command, cancellationToken);

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        return Ok(response);
    }

    /// <summary>
    /// Remove account
    /// </summary>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>IActionResult</returns>
    /// CreatedBy: ThiepTT(02/11/2023)
    [HttpDelete]
    [Route($"{ApiRoutes.UserProfile.RemoveAccount}")]
    public async Task<IActionResult> RemoveAccount(CancellationToken cancellationToken)
    {
        Guid userProfileId = HttpContext.GetUserProfileId();

        RemoveAccountCommand command = new RemoveAccountCommand() { UserProfileId = userProfileId };

        OperationResult<string> response = await _mediator.Send(command, cancellationToken);

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        return Ok(response);
    }
}