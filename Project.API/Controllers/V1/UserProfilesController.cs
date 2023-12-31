﻿using Asp.Versioning;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.API.Attributes;
using Project.API.Options;
using Project.Application.Dtos.Requests;
using Project.Application.Models;
using Project.Application.Users.Commands;
using Project.Application.Users.Queries;
using Project.Domain.Aggregates;
using Project.Domain.Interfaces.IServices;

namespace Project.API.Controllers.V1;

/// <summary>
/// Information of user profile controller
/// CreatedBy: ThiepTT(02/11/2023)
/// </summary>
[ApiVersion($"{ApiRoutes.Version.V1}")]
[Route($"{ApiRoutes.BaseRouter}")]
[ApiController]
[Authorize]
public class UserProfilesController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IResponseCacheService _responseCacheService;

    /// <summary>
    /// User profiles controller
    /// </summary>
    /// <param name="mediator">IMediator</param>
    /// <param name="mapper">IMapper</param>
    /// <param name="responseCacheService">IResponseCacheService</param>
    /// CreatedBy: ThiepTT(02/11/2023)
    public UserProfilesController(IMediator mediator, IMapper mapper, IResponseCacheService responseCacheService)
    {
        _mediator = mediator;
        _mapper = mapper;
        _responseCacheService = responseCacheService;
    }

    /// <summary>
    /// Get all user profiles
    /// </summary>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>IActionResult</returns>
    /// CreatedBy: ThiepTT(06/11/2023)
    [HttpGet]
    [Route($"{ApiRoutes.UserProfiles.GetAllUserProfiles}")]
    [JwtAuthorize($"{ApiRoutes.Role.Admin}")]
    [Cache(ApiRoutes.TimeToLive)]
    public async Task<IActionResult> GetAllUserProfiles(CancellationToken cancellationToken)
    {
        GetAllUserProfilesQuery query = new GetAllUserProfilesQuery();

        OperationResult<IEnumerable<UserProfile>> response = await _mediator.Send(query, cancellationToken);

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        return Ok(response);
    }

    /// <summary>
    /// Get all user profiles entity framework
    /// </summary>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>IActionResult</returns>
    /// CreatedBy: ThiepTT(06/11/2023)
    [HttpGet]
    [Route($"{ApiRoutes.UserProfiles.GetAllUserProfilesEF}")]
    [JwtAuthorize($"{ApiRoutes.Role.Admin}")]
    [Cache(ApiRoutes.TimeToLive)]
    public async Task<IActionResult> GetAllUserProfilesEF(CancellationToken cancellationToken)
    {
        GetAllUserProfilesEFQuery query = new GetAllUserProfilesEFQuery();

        OperationResult<IEnumerable<UserProfile>> response = await _mediator.Send(query, cancellationToken);

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        return Ok(response);
    }

    /// <summary>
    /// Get all user profiles entity framework as no tracking
    /// </summary>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>IActionResult</returns>
    /// CreatedBy: ThiepTT(06/11/2023)
    [HttpGet]
    [Route($"{ApiRoutes.UserProfiles.GetAllUserProfilesEFAsNoTracking}")]
    [JwtAuthorize($"{ApiRoutes.Role.Admin}")]
    [Cache(ApiRoutes.TimeToLive)]
    public async Task<IActionResult> GetAllUserProfilesEFAsNoTracking(CancellationToken cancellationToken)
    {
        GetAllUserProfilesEFAsNoTrackingQuery query = new GetAllUserProfilesEFAsNoTrackingQuery();

        OperationResult<IEnumerable<UserProfile>> response = await _mediator.Send(query, cancellationToken);

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        return Ok(response);
    }

    /// <summary>
    /// Get user profile by id
    /// </summary>
    /// <param name="userProfileId">Id of user profile</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>IActionResult</returns>
    /// CreatedBy: ThiepTT(03/11/2023)
    [HttpGet]
    [Route($"{ApiRoutes.UserProfiles.GetUserProfileById}")]
    [JwtAuthorize($"{ApiRoutes.Role.Admin}")]
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
    [Route($"{ApiRoutes.UserProfiles.UpdateUserProfileById}")]
    [JwtAuthorize($"{ApiRoutes.Role.Admin}")]
    [ValidateGuid("userProfileId")]
    public async Task<IActionResult> UpdateUserProfileById(string? userProfileId, UserProfileRequest userProfile, CancellationToken cancellationToken)
    {
        Guid.TryParse(userProfileId, out Guid userId);

        UpdateUserProfileCommand command = _mapper.Map<UpdateUserProfileCommand>(userProfile);
        command.UserProfileId = userId;

        OperationResult<string> response = await _mediator.Send(command, cancellationToken);

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        string pattern = SystemConfig.GeneratePattern(
            $"{nameof(ApiRoutes.UserProfiles)}",
            $"{ApiRoutes.Api}",
            $"{ApiRoutes.UserProfiles.GetAllUserProfiles}");

        await _responseCacheService.RemoveCacheResponseAsync(pattern);

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
    [Route($"{ApiRoutes.UserProfiles.RemoveUserProfileById}")]
    [JwtAuthorize($"{ApiRoutes.Role.Admin}")]
    [ValidateGuid("userProfileId")]
    public async Task<IActionResult> RemoveUserProfileById(string? userProfileId, CancellationToken cancellationToken)
    {
        Guid.TryParse(userProfileId, out Guid userId);

        RemoveAccountCommand command = new RemoveAccountCommand() { UserProfileId = userId };

        OperationResult<string> response = await _mediator.Send(command, cancellationToken);

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        string pattern = SystemConfig.GeneratePattern(
            $"{nameof(ApiRoutes.UserProfiles)}",
            $"{ApiRoutes.Api}",
            $"{ApiRoutes.UserProfiles.GetAllUserProfiles}");

        await _responseCacheService.RemoveCacheResponseAsync(pattern);

        return Ok(response);
    }

    /// <summary>
    /// Get information of user profile
    /// </summary>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>IActionResult</returns>
    /// CreatedBy: ThiepTT(02/11/2023)
    [HttpGet]
    [Route($"{ApiRoutes.UserProfiles.GetInformationOfUserProfile}")]
    public async Task<IActionResult> GetInformationOfUserProfile(CancellationToken cancellationToken)
    {
        GetUserProfileByIdQuery query = new GetUserProfileByIdQuery() { UserProfileId = UserProfileId };

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
    [Route($"{ApiRoutes.UserProfiles.UpdateInformationOfUserProfile}")]
    public async Task<IActionResult> UpdateInformationOfUserProfile(UserProfileRequest userProfile, CancellationToken cancellationToken)
    {
        UpdateUserProfileCommand command = _mapper.Map<UpdateUserProfileCommand>(userProfile);
        command.UserProfileId = UserProfileId;

        OperationResult<string> response = await _mediator.Send(command, cancellationToken);

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        string pattern = SystemConfig.GeneratePattern(
             $"{nameof(ApiRoutes.UserProfiles)}",
             $"{ApiRoutes.Api}",
             $"{ApiRoutes.UserProfiles.GetAllUserProfiles}");

        await _responseCacheService.RemoveCacheResponseAsync(pattern);

        return Ok(response);
    }

    /// <summary>
    /// Remove account
    /// </summary>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>IActionResult</returns>
    /// CreatedBy: ThiepTT(02/11/2023)
    [HttpDelete]
    [Route($"{ApiRoutes.UserProfiles.RemoveAccount}")]
    public async Task<IActionResult> RemoveAccount(CancellationToken cancellationToken)
    {
        RemoveAccountCommand command = new RemoveAccountCommand() { UserProfileId = UserProfileId };

        OperationResult<string> response = await _mediator.Send(command, cancellationToken);

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        string pattern = SystemConfig.GeneratePattern(
            $"{nameof(ApiRoutes.UserProfiles)}",
            $"{ApiRoutes.Api}",
            $"{ApiRoutes.UserProfiles.GetAllUserProfiles}");

        await _responseCacheService.RemoveCacheResponseAsync(pattern);

        return Ok(response);
    }
}