using AutoMapper;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Project.Application.Dtos.Requests;
using Project.Application.Identities.Commands;
using Project.Application.Models;
using Project.Application.Services.IServices;
using Project.Presentation.Options;

namespace Project.Presentation.APIs.V2;

/// <summary>
/// Information off authentication api
/// CreatedBy: ThiepTT(15/12/2023)
/// </summary>
public class AuthenticationApi : BaseApi, ICarterModule
{
    public const string BaseUrl = "api/v{version:apiversion}/Authentications";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.NewVersionedApi("Authentications").HasApiVersion(2, 0).MapGroup(BaseUrl);

        group.MapPost($"{ApiRoutes.Authentications.Register}", Register);
        group.MapPost($"{ApiRoutes.Authentications.Login}", Login);
    }

    /// <summary>
    /// Register
    /// </summary>
    /// <param name="mapper">IMapper</param>
    /// <param name="mediator">IMediator</param>
    /// <param name="responseCacheService">IResponseCacheService</param>
    /// <param name="registerRequest">RegisterRequest</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>IResult</returns>
    /// CreatedBy: ThiepTT(15/12/2023)
    public async Task<IResult> Register(IMapper mapper, IMediator mediator, IResponseCacheService responseCacheService, RegisterRequest registerRequest,
        CancellationToken cancellationToken)
    {
        RegisterCommand command = mapper.Map<RegisterCommand>(registerRequest);

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
    /// Login
    /// </summary>
    /// <param name="mapper">IMapper</param>
    /// <param name="mediator">IMediator</param>
    /// <param name="loginRequest">LoginRequest</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>IResult</returns>
    /// CreatedBy: ThiepTT(15/12/2023)
    public async Task<IResult> Login(IMapper mapper, IMediator mediator, LoginRequest loginRequest, CancellationToken cancellationToken)
    {
        LoginCommand command = mapper.Map<LoginCommand>(loginRequest);

        OperationResult<string> response = await mediator.Send(command, cancellationToken);

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        return Results.Ok(response);
    }
}