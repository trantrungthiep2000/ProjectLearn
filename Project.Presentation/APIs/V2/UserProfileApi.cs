using AutoMapper;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Project.Application.Dtos.Requests;
using Project.Application.Models;
using Project.Application.Services.IServices;
using Project.Application.Users.Commands;
using Project.Application.Users.Queries;
using Project.Domain.Aggregates;
using Project.Presentation.Options;

namespace Project.Presentation.APIs.V2;

/// <summary>
/// Information of user profile api
/// CreatedBy: ThiepTT(15/12/2023)
/// </summary>
public class UserProfileApi : BaseApi, ICarterModule
{
    public const string BaseUrl = "api/v{version:apiversion}/UserProfiles";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.NewVersionedApi("UserProfiles").HasApiVersion(2, 0).MapGroup(BaseUrl).RequireAuthorization();

        group.MapGet($"{ApiRoutes.UserProfiles.GetAllUserProfiles}", GetAllUserProfiles);
        group.MapGet($"{ApiRoutes.UserProfiles.GetAllUserProfilesEF}", GetAllUserProfilesEF);
        group.MapGet($"{ApiRoutes.UserProfiles.GetAllUserProfilesEFAsNoTracking}", GetAllUserProfilesEFAsNoTracking);
        group.MapGet($"{ApiRoutes.UserProfiles.GetUserProfileById}", GetUserProfileById);
        group.MapPut($"{ApiRoutes.UserProfiles.UpdateUserProfileById}", UpdateUserProfileById);
        group.MapDelete($"{ApiRoutes.UserProfiles.RemoveUserProfileById}", RemoveUserProfileById);
        group.MapGet($"{ApiRoutes.UserProfiles.GetInformationOfUserProfile}", GetInformationOfUserProfile);
        group.MapPut($"{ApiRoutes.UserProfiles.UpdateInformationOfUserProfile}", UpdateInformationOfUserProfile);
        group.MapDelete($"{ApiRoutes.UserProfiles.RemoveAccount}", RemoveAccount);
    }

    /// <summary>
    /// Get all user profiles
    /// </summary>
    /// <param name="mediator">IMediator</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>IResult</returns>
    /// CreatedBy: ThiepTT(18/12/2023)
    public async Task<IResult> GetAllUserProfiles(IMediator mediator, CancellationToken cancellationToken)
    {
        GetAllUserProfilesQuery query = new GetAllUserProfilesQuery();

        OperationResult<IEnumerable<UserProfile>> response = await mediator.Send(query, cancellationToken);

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        return Results.Ok(response);
    }

    /// <summary>
    /// Get all user profile entity framework
    /// </summary>
    /// <param name="mediator">IMediator</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>IResult</returns>
    /// CreatedBy: ThiepTT(18/12/2023)
    public async Task<IResult> GetAllUserProfilesEF(IMediator mediator, CancellationToken cancellationToken)
    {
        GetAllUserProfilesEFQuery query = new GetAllUserProfilesEFQuery();

        OperationResult<IEnumerable<UserProfile>> response = await mediator.Send(query, cancellationToken);

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        return Results.Ok(response);
    }

    /// <summary>
    /// Get all user profiles entity framework as no tracking
    /// </summary>
    /// <param name="mediator">IMediator</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>IResult</returns>
    /// CreatedBy: ThiepTT(18/12/2023)
    public async Task<IResult> GetAllUserProfilesEFAsNoTracking(IMediator mediator, CancellationToken cancellationToken)
    {
        GetAllUserProfilesEFAsNoTrackingQuery query = new GetAllUserProfilesEFAsNoTrackingQuery();

        OperationResult<IEnumerable<UserProfile>> response = await mediator.Send(query, cancellationToken);

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        return Results.Ok(response);
    }

    /// <summary>
    /// Get user profile by id
    /// </summary>
    /// <param name="userProfileId">Id of user profile</param>
    /// <param name="mediator">IMediator</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>IResult</returns>
    /// CreatedBy: ThiepTT(18/12/2023)
    public async Task<IResult> GetUserProfileById(string? userProfileId, IMediator mediator, CancellationToken cancellationToken)
    {
        Guid.TryParse(userProfileId, out Guid userId);

        GetUserProfileByIdQuery query = new GetUserProfileByIdQuery() { UserProfileId = userId };

        OperationResult<UserProfile> response = await mediator.Send(query, cancellationToken);

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        return Results.Ok(response);
    }
    
    /// <summary>
    /// Update user profile by id
    /// </summary>
    /// <param name="userProfileId">Id of user profile</param>
    /// <param name="userProfile">UserProfileRequest</param>
    /// <param name="mapper">IMapper</param>
    /// <param name="mediator">IMediator</param>
    /// <param name="responseCacheService">IResponseCacheService</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>IResult</returns>
    /// CreatedBu: ThiepTT(18/12/2023)
    public async Task<IResult> UpdateUserProfileById(string? userProfileId, UserProfileRequest userProfile, IMapper mapper, IMediator mediator, IResponseCacheService responseCacheService,
        CancellationToken cancellationToken)
    {
        Guid.TryParse(userProfileId, out Guid userId);

        UpdateUserProfileCommand command = mapper.Map<UpdateUserProfileCommand>(userProfile);
        command.UserProfileId = userId;

        OperationResult<string> response = await mediator.Send(command, cancellationToken);

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        string pattern = SystemConfig.GeneratePattern(
            $"{nameof(ApiRoutes.UserProfiles)}",
            $"{ApiRoutes.Api}",
            $"{ApiRoutes.UserProfiles.GetAllUserProfiles}");

        await responseCacheService.RemoveCacheResponseAsync(pattern);

        return Results.Ok(response);
    }

    /// <summary>
    /// Remove user profile by id
    /// </summary>
    /// <param name="userProfileId">Id of user profile</param>
    /// <param name="mediator">IMediator</param>
    /// <param name="responseCacheService">IResponseCacheService</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>IResult</returns>
    /// CreatedBy: ThiepTT(18/12/2023)
    public async Task<IResult> RemoveUserProfileById(string? userProfileId, IMediator mediator, IResponseCacheService responseCacheService, CancellationToken cancellationToken)
    {
        Guid.TryParse(userProfileId, out Guid userId);

        RemoveAccountCommand command = new RemoveAccountCommand() { UserProfileId = userId };

        OperationResult<string> response = await mediator.Send(command, cancellationToken);

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        string pattern = SystemConfig.GeneratePattern(
            $"{nameof(ApiRoutes.UserProfiles)}",
            $"{ApiRoutes.Api}",
            $"{ApiRoutes.UserProfiles.GetAllUserProfiles}");

        await responseCacheService.RemoveCacheResponseAsync(pattern);

        return Results.Ok(response);
    }

    /// <summary>
    /// Get information of user profile
    /// </summary>
    /// <param name="mediator">IMediator</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>IResult</returns>
    /// CreatedBy: ThiepTT(18/12/2023)
    public async Task<IResult> GetInformationOfUserProfile(IMediator mediator, CancellationToken cancellationToken)
    {
        GetUserProfileByIdQuery query = new GetUserProfileByIdQuery() { UserProfileId = UserProfileId };

        OperationResult<UserProfile> response = await mediator.Send(query, cancellationToken);

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        return Results.Ok(response);
    }

    /// <summary>
    /// Update information of user profile
    /// </summary>
    /// <param name="userProfile">UserProfileRequest</param>
    /// <param name="mapper">IMapper</param>
    /// <param name="mediator">IMediator</param>
    /// <param name="responseCacheService">IResponseCacheService</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>IResult</returns>
    /// CreatedBy: ThiepTT(18/12/2023)
    public async Task<IResult> UpdateInformationOfUserProfile(UserProfileRequest userProfile, IMapper mapper, IMediator mediator, IResponseCacheService responseCacheService,
        CancellationToken cancellationToken)
    {
        UpdateUserProfileCommand command = mapper.Map<UpdateUserProfileCommand>(userProfile);
        command.UserProfileId = UserProfileId;

        OperationResult<string> response = await mediator.Send(command, cancellationToken);

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        string pattern = SystemConfig.GeneratePattern(
             $"{nameof(ApiRoutes.UserProfiles)}",
             $"{ApiRoutes.Api}",
             $"{ApiRoutes.UserProfiles.GetAllUserProfiles}");

        await responseCacheService.RemoveCacheResponseAsync(pattern);

        return Results.Ok(response);
    }

    /// <summary>
    /// Remove account
    /// </summary>
    /// <param name="mediator">IMediator</param>
    /// <param name="responseCacheService">IResponseCacheService</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>IResult</returns>
    /// CreatedBy: ThiepTT(18/12/2023)
    public async Task<IResult> RemoveAccount(IMediator mediator, IResponseCacheService responseCacheService, CancellationToken cancellationToken)
    {
        RemoveAccountCommand command = new RemoveAccountCommand() { UserProfileId = UserProfileId };

        OperationResult<string> response = await mediator.Send(command, cancellationToken);

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        string pattern = SystemConfig.GeneratePattern(
            $"{nameof(ApiRoutes.UserProfiles)}",
            $"{ApiRoutes.Api}",
            $"{ApiRoutes.UserProfiles.GetAllUserProfiles}");

        await responseCacheService.RemoveCacheResponseAsync(pattern);

        return Results.Ok(response);
    }
}