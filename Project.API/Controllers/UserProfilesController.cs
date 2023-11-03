using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.API.Attributes;
using Project.API.Extenstions;
using Project.API.Options;
using Project.Application.Dtos.Requests;
using Project.Application.Models;
using Project.Application.Users.Commands;
using Project.Application.Users.Queries;
using Project.Domain.Aggregates;

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
    /// Get user profile by id
    /// </summary>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>IActionResult</returns>
    /// CreatedBy: ThiepTT(03/11/2023)
    [HttpGet]
    [Route($"{ApiRoutes.UserProfile.GetUserProfileById}")]
    [ValidateGuid("userProfileId")]
    public async Task<IActionResult> GetUserProfileById(string? userProfileId, CancellationToken cancellationToken)
    {
        Guid.TryParse(userProfileId, out Guid userId);

        GetUserProfileByIdQuery query = new GetUserProfileByIdQuery() { UserProfileId = userId };

        OperationResult<UserProfile> response = await _mediator.Send(query, cancellationToken);

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        return Ok(response);
    }

    /// <summary>
    /// Update user profile by id
    /// </summary>
    /// <param name="userProfileId">Id of user profile</param>
    /// <param name="userProfile">UserProfileRequest</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>IActionResult</returns>
    /// CreatedBy: ThiepTT(02/11/2023)
    [HttpPut]
    [Route($"{ApiRoutes.UserProfile.UpdateUserProfileById}")]
    [ValidateGuid("userProfileId")]
    public async Task<IActionResult> UpdateUserProfileById(string? userProfileId, UserProfileRequest userProfile, CancellationToken cancellationToken)
    {
        Guid.TryParse(userProfileId, out Guid userId);

        UpdateUserProfileCommand command = _mapper.Map<UpdateUserProfileCommand>(userProfile);
        command.UserProfileId = userId;

        OperationResult<string> response = await _mediator.Send(command, cancellationToken);

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        return Ok(response);
    }

    /// <summary>
    /// Remove user profile by id
    /// </summary>
    /// <param name="userProfileId">Id of user profile</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>IActionResult</returns>
    /// CreatedBy: ThiepTT(02/11/2023)
    [HttpDelete]
    [Route($"{ApiRoutes.UserProfile.RemoveUserProfileById}")]
    [ValidateGuid("userProfileId")]
    public async Task<IActionResult> RemoveUserProfileById(string? userProfileId, CancellationToken cancellationToken)
    {
        Guid.TryParse(userProfileId, out Guid userId);

        RemoveAccountCommand command = new RemoveAccountCommand() { UserProfileId = userId };

        OperationResult<string> response = await _mediator.Send(command, cancellationToken);

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        return Ok(response);
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