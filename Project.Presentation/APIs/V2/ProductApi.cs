using AutoMapper;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Project.Application.Dtos.Requests;
using Project.Application.Dtos.Responses;
using Project.Application.Models;
using Project.Application.Products.Commands;
using Project.Application.Products.Queries;
using Project.Application.Services.IServices;
using Project.Presentation.Options;

namespace Project.Presentation.APIs.V2;

/// <summary>
/// Information of product api
/// CreatedBy: ThiepTT(11/12/2023)
/// </summary>
public class ProductApi : BaseApi, ICarterModule
{
    private const string BaseUrl = "api/v{version:apiversion}/Products";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.NewVersionedApi("Products").HasApiVersion(2, 0).MapGroup(BaseUrl).RequireAuthorization();

        group.MapGet($"{ApiRoutes.Products.GetAllProducts}", GetAllProducts);
        group.MapGet($"{ApiRoutes.Products.GetProductById}", GetProductById);
        group.MapPost($"{ApiRoutes.Products.CreateBulkProduct}", CreateBulkProduct);
        group.MapPost($"{ApiRoutes.Products.CreateProduct}", CreateProduct);
        group.MapPut($"{ApiRoutes.Products.UpdateProduct}", UpdateProduct);
        group.MapDelete($"{ApiRoutes.Products.DeleteBulkProduct}", DeleteBulkProduct);
        group.MapDelete($"{ApiRoutes.Products.DeleteProduct}", DeleteProduct);
    }

    /// <summary>
    /// Get all products
    /// </summary>
    /// <param name="mapper">IMapper</param>
    /// <param name="mediator">IMediator</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>IResult</returns>
    /// CreatedBy: ThiepTT(11/12/2023)
    public async Task<IResult> GetAllProducts(IMapper mapper, IMediator mediator, CancellationToken cancellationToken)
    {
        GetAllProductsQuery query = new GetAllProductsQuery();

        OperationResult<IEnumerable<ProductResponse>> response = mapper
            .Map<OperationResult<IEnumerable<ProductResponse>>>(await mediator.Send(query, cancellationToken));

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        return Results.Ok(response);
    }

    /// <summary>
    /// Get product by id
    /// </summary>
    /// <param name="productId">Id of product</param>
    /// <param name="mapper">IMapper</param>
    /// <param name="mediator">IMediator</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>IResult</returns>
    /// CreatedBy: ThiepTT(11/12/2023)
    public async Task<IResult> GetProductById(string? productId, IMapper mapper, IMediator mediator, CancellationToken cancellationToken)
    {
        Guid.TryParse(productId, out Guid id);

        GetProductByIdQuery query = new GetProductByIdQuery() { ProductId = id };

        OperationResult<ProductResponse> response = mapper
            .Map<OperationResult<ProductResponse>>(await mediator.Send(query, cancellationToken));

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        return Results.Ok(response);
    }

    /// <summary>
    /// Create bulk product
    /// </summary>
    /// <param name="file">IFormFile</param>
    /// <param name="mapper">IMapper</param>
    /// <param name="mediator">IMediator</param>
    /// <param name="responseCacheService">IResponseCacheService</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>IResult</returns>
    /// CreatedBy: ThiepTT(11/12/2023)
    public async Task<IResult> CreateBulkProduct(IFormFile? file, IMapper mapper, IMediator mediator, IResponseCacheService responseCacheService,
        CancellationToken cancellationToken)
    {
        CreateBulkProductCommand command = new CreateBulkProductCommand() { File = file, CreatedBy = FullName };

        OperationResult<string> response = await mediator.Send(command, cancellationToken);

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        string pattern = SystemConfig.GeneratePattern(
            $"{nameof(ApiRoutes.Products)}",
            $"{ApiRoutes.Api}",
            $"{ApiRoutes.Products.GetAllProducts}");

        await responseCacheService.RemoveCacheResponseAsync(pattern);

        return Results.Ok(response);
    }

    /// <summary>
    /// Created product
    /// </summary>
    /// <param name="product">ProductRequest</param>
    /// <param name="mapper">IMapper</param>
    /// <param name="mediator">IMediator</param>
    /// <param name="responseCacheService">IResponseCacheService</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>IResult</returns>
    /// CreatedBy: ThiepTT(11/12/2023)
    public async Task<IResult> CreateProduct([FromBody] ProductRequest product, IMapper mapper, IMediator mediator, IResponseCacheService responseCacheService,
        CancellationToken cancellationToken)
    {
        CreateProductCommand command = mapper.Map<CreateProductCommand>(product);
        command.CreatedBy = FullName;

        OperationResult<string> response = await mediator.Send(command, cancellationToken);

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        string pattern = SystemConfig.GeneratePattern(
            $"{nameof(ApiRoutes.Products)}",
            $"{ApiRoutes.Api}",
            $"{ApiRoutes.Products.GetAllProducts}");

        await responseCacheService.RemoveCacheResponseAsync(pattern);

        return Results.Ok(response);
    }

    /// <summary>
    /// Update product
    /// </summary>
    /// <param name="productId">Id of product</param>
    /// <param name="product">ProductRequest</param>
    /// <param name="mapper">IMapper</param>
    /// <param name="mediator">IMediator</param>
    /// <param name="responseCacheService">IResponseCacheService</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>IResult</returns>
    /// CreatedBy: ThiepTT(11/12/2023)
    public async Task<IResult> UpdateProduct(string? productId, [FromBody] ProductRequest product, IMapper mapper, IMediator mediator,
        IResponseCacheService responseCacheService, CancellationToken cancellationToken)
    {
        Guid.TryParse(productId, out Guid id);

        UpdateProductCommand command = mapper.Map<UpdateProductCommand>(product);
        command.ProductId = id;
        command.UpdatedBy = FullName;

        OperationResult<string> response = await mediator.Send(command, cancellationToken);

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        string pattern = SystemConfig.GeneratePattern(
           $"{nameof(ApiRoutes.Products)}",
           $"{ApiRoutes.Api}",
           $"{ApiRoutes.Products.GetAllProducts}");

        await responseCacheService.RemoveCacheResponseAsync(pattern);

        return Results.Ok(response);
    }

    /// <summary>
    /// Delete bulk product
    /// </summary>
    /// <param name="listProductId">List id of product</param>
    /// <param name="mediator">IMediator</param>
    /// <param name="responseCacheService">IResponseCacheService</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>IResult</returns>
    /// CreatedBy: ThiepTT(11/12/2023)
    public async Task<IResult> DeleteBulkProduct([FromBody] List<string>? listProductId, IMediator mediator, IResponseCacheService responseCacheService,
        CancellationToken cancellationToken)
    {
        DeleteBulkProductCommand command = new DeleteBulkProductCommand() { ListProductId = listProductId! };

        OperationResult<string> response = await mediator.Send(command, cancellationToken);

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        string pattern = SystemConfig.GeneratePattern(
           $"{nameof(ApiRoutes.Products)}",
           $"{ApiRoutes.Api}",
           $"{ApiRoutes.Products.GetAllProducts}");

        await responseCacheService.RemoveCacheResponseAsync(pattern);

        return Results.Ok(response);
    }

    /// <summary>
    /// Delete product
    /// </summary>
    /// <param name="productId">Id of product</param>
    /// <param name="mediator">IMediator</param>
    /// <param name="responseCacheService">IResponseCacheService</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>IResult</returns>
    /// CreatedBy: ThiepTT(11/12/2023)
    public async Task<IResult> DeleteProduct(string? productId, IMediator mediator, IResponseCacheService responseCacheService,
        CancellationToken cancellationToken)
    {
        Guid.TryParse(productId, out Guid id);

        DeleteProductCommand command = new DeleteProductCommand() { ProductId = id };

        OperationResult<string> response = await mediator.Send(command, cancellationToken);

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        string pattern = SystemConfig.GeneratePattern(
           $"{nameof(ApiRoutes.Products)}",
           $"{ApiRoutes.Api}",
           $"{ApiRoutes.Products.GetAllProducts}");

        await responseCacheService.RemoveCacheResponseAsync(pattern);

        return Results.Ok(response);
    }
}