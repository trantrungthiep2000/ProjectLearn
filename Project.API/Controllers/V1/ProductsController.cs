using Asp.Versioning;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.API.Attributes;
using Project.API.Options;
using Project.Application.Dtos.Requests;
using Project.Application.Dtos.Responses;
using Project.Application.Models;
using Project.Application.Products.Commands;
using Project.Application.Products.Queries;
using Project.Domain.Interfaces.IServices;

namespace Project.API.Controllers.V1;

/// <summary>
/// Information of products controller
/// CreatedBy: ThiepTT(07/11/2023)
/// </summary>
[ApiVersion($"{ApiRoutes.Version.V1}")]
[Route($"{ApiRoutes.BaseRouter}")]
[ApiController]
[Authorize]
public class ProductsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IResponseCacheService _responseCacheService;

    /// <summary>
    /// Products controller
    /// </summary>
    /// <param name="mediator">IMediator</param>
    /// <param name="mapper">IMapper</param>
    /// <param name="responseCacheService">IResponseCacheService</param>
    /// CreatedBy: ThiepTT(07/11/2023)
    public ProductsController(IMediator mediator, IMapper mapper, IResponseCacheService responseCacheService)
    {
        _mediator = mediator;
        _mapper = mapper;
        _responseCacheService = responseCacheService;
    }

    /// <summary>
    /// Get all products
    /// </summary>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>IActionResult</returns>
    /// CreatedBy: ThiepTT(07/11/2023)
    [HttpGet]
    [Route($"{ApiRoutes.Products.GetAllProducts}")]
    [Cache(ApiRoutes.TimeToLive)]
    public async Task<IActionResult> GetAllProducts(CancellationToken cancellationToken)
    {
        GetAllProductsQuery query = new GetAllProductsQuery();

        OperationResult<IEnumerable<ProductResponse>> response = _mapper
            .Map<OperationResult<IEnumerable<ProductResponse>>>(await _mediator.Send(query, cancellationToken));

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        return Ok(response);
    }

    /// <summary>
    /// Get product by id
    /// </summary>
    /// <param name="productId">Id of product</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>IActionResult</returns>
    /// CreatedBy: ThiepTT(07/11/2023)
    [HttpGet]
    [Route($"{ApiRoutes.Products.GetProductById}")]
    [ValidateGuid("productId")]
    public async Task<IActionResult> GetProductById(string? productId, CancellationToken cancellationToken)
    {
        Guid.TryParse(productId, out Guid id);

        GetProductByIdQuery query = new GetProductByIdQuery() { ProductId = id };

        OperationResult<ProductResponse> response = _mapper
            .Map<OperationResult<ProductResponse>>(await _mediator.Send(query, cancellationToken));

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        return Ok(response);
    }

    /// <summary>
    /// Create bulk product
    /// </summary>
    /// <param name="file">IFormFile</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>IActionResult</returns>
    /// CreatedBy: ThiepTT(10/11/2023)
    [HttpPost]
    [Route($"{ApiRoutes.Products.CreateBulkProduct}")]
    [JwtAuthorize($"{ApiRoutes.Role.Admin}")]
    public async Task<IActionResult> CreateBulkProduct(IFormFile? file, CancellationToken cancellationToken)
    {
        CreateBulkProductCommand command = new CreateBulkProductCommand() { File = file, CreatedBy = FullName };

        OperationResult<string> response = await _mediator.Send(command, cancellationToken);

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        string pattern = SystemConfig.GeneratePattern(
            $"{nameof(ApiRoutes.Products)}",
            $"{ApiRoutes.Api}",
            $"{ApiRoutes.Products.GetAllProducts}");

        await _responseCacheService.RemoveCacheResponseAsync(pattern);

        return Ok(response);
    }

    /// <summary>
    /// Create product
    /// </summary>
    /// <param name="product">ProductRequest</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>IActionResult</returns>
    /// CreatedBy: ThiepTT(08/11/2023)
    [HttpPost]
    [Route($"{ApiRoutes.Products.CreateProduct}")]
    public async Task<IActionResult> CreateProduct(ProductRequest product, CancellationToken cancellationToken)
    {
        CreateProductCommand command = _mapper.Map<CreateProductCommand>(product);
        command.CreatedBy = FullName;

        OperationResult<string> response = await _mediator.Send(command, cancellationToken);

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        string pattern = SystemConfig.GeneratePattern(
            $"{nameof(ApiRoutes.Products)}",
            $"{ApiRoutes.Api}",
            $"{ApiRoutes.Products.GetAllProducts}");

        await _responseCacheService.RemoveCacheResponseAsync(pattern);

        return Ok(response);
    }

    /// <summary>
    /// Update product
    /// </summary>
    /// <param name="productId">Id of product</param>
    /// <param name="product">ProductRequest</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>IActionResult</returns>
    /// CreatedBy: ThiepTT(08/11/2023)
    [HttpPut]
    [Route($"{ApiRoutes.Products.UpdateProduct}")]
    [ValidateGuid("productId")]
    public async Task<IActionResult> UpdateProduct(string? productId, ProductRequest product, CancellationToken cancellationToken)
    {
        Guid.TryParse(productId, out Guid id);

        UpdateProductCommand command = _mapper.Map<UpdateProductCommand>(product);
        command.ProductId = id;
        command.UpdatedBy = FullName;

        OperationResult<string> response = await _mediator.Send(command, cancellationToken);

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        string pattern = SystemConfig.GeneratePattern(
           $"{nameof(ApiRoutes.Products)}",
           $"{ApiRoutes.Api}",
           $"{ApiRoutes.Products.GetAllProducts}");

        await _responseCacheService.RemoveCacheResponseAsync(pattern);

        return Ok(response);
    }

    /// <summary>
    /// Delete bulk product
    /// </summary>
    /// <param name="listProductId">List Id of product</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>IActionResult</returns>
    /// CreatedBy: ThiepTT(10/11/2023)
    [HttpDelete]
    [Route($"{ApiRoutes.Products.DeleteBulkProduct}")]
    [JwtAuthorize($"{ApiRoutes.Role.Admin}")]
    public async Task<IActionResult> DeleteBulkProduct(List<string>? listProductId, CancellationToken cancellationToken)
    {
        DeleteBulkProductCommand command = new DeleteBulkProductCommand() { ListProductId = listProductId! };

        OperationResult<string> response = await _mediator.Send(command, cancellationToken);

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        string pattern = SystemConfig.GeneratePattern(
           $"{nameof(ApiRoutes.Products)}",
           $"{ApiRoutes.Api}",
           $"{ApiRoutes.Products.GetAllProducts}");

        await _responseCacheService.RemoveCacheResponseAsync(pattern);

        return Ok(response);
    }

    /// <summary>
    /// Delete product
    /// </summary>
    /// <param name="productId">Id of product</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>IActionResult</returns>
    /// CreatedBy: ThiepTT(08/11/2023)
    [HttpDelete]
    [Route($"{ApiRoutes.Products.DeleteProduct}")]
    [ValidateGuid("productId")]
    public async Task<IActionResult> DeleteProduct(string? productId, CancellationToken cancellationToken)
    {
        Guid.TryParse(productId, out Guid id);

        DeleteProductCommand command = new DeleteProductCommand() { ProductId = id };

        OperationResult<string> response = await _mediator.Send(command, cancellationToken);

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        string pattern = SystemConfig.GeneratePattern(
           $"{nameof(ApiRoutes.Products)}",
           $"{ApiRoutes.Api}",
           $"{ApiRoutes.Products.GetAllProducts}");

        await _responseCacheService.RemoveCacheResponseAsync(pattern);

        return Ok(response);
    }
}