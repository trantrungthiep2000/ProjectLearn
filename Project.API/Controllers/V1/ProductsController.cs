﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.API.Attributes;
using Project.API.Extenstions;
using Project.API.Options;
using Project.Application.Dtos.Requests;
using Project.Application.Dtos.Responses;
using Project.Application.Models;
using Project.Application.Products.Commands;
using Project.Application.Products.Queries;
using Project.Application.Services.IServices;
using Project.Domain.Aggregates;

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
    [Route($"{ApiRoutes.Product.GetAllProducts}")]
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
    [Route($"{ApiRoutes.Product.GetProductById}")]
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
    [Route($"{ApiRoutes.Product.CreateBulkProduct}")]
    [JwtAuthorize($"{ApiRoutes.Role.Admin}")]
    public async Task<IActionResult> CreateBulkProduct(IFormFile? file, CancellationToken cancellationToken)
    {
        string fullName = HttpContext.GetFullName();

        CreateBulkProductCommand command = new CreateBulkProductCommand() { File = file, CreatedBy = fullName };

        OperationResult<string> response = await _mediator.Send(command, cancellationToken);

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        string pattern = SystemConfig.GeneratePattern(
            ControllerContext.ActionDescriptor.ControllerTypeInfo.Name,
            $"{ApiRoutes.Api}",
            $"{ApiRoutes.Product.GetAllProducts}");

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
    [Route($"{ApiRoutes.Product.CreateProduct}")]
    public async Task<IActionResult> CreateProduct(ProductRequest product, CancellationToken cancellationToken)
    {
        string fullName = HttpContext.GetFullName();

        CreateProductCommand command = _mapper.Map<CreateProductCommand>(product);
        command.CreatedBy = fullName;

        OperationResult<string> response = await _mediator.Send(command, cancellationToken);

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        string pattern = SystemConfig.GeneratePattern(
            ControllerContext.ActionDescriptor.ControllerTypeInfo.Name,
            $"{ApiRoutes.Api}",
            $"{ApiRoutes.Product.GetAllProducts}");

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
    [Route($"{ApiRoutes.Product.UpdateProduct}")]
    [ValidateGuid("productId")]
    public async Task<IActionResult> UpdateProduct(string? productId, ProductRequest product, CancellationToken cancellationToken)
    {
        Guid.TryParse(productId, out Guid id);

        string fullName = HttpContext.GetFullName();

        UpdateProductCommand command = _mapper.Map<UpdateProductCommand>(product);
        command.ProductId = id;
        command.UpdatedBy = fullName;

        OperationResult<string> response = await _mediator.Send(command, cancellationToken);

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        string pattern = SystemConfig.GeneratePattern(
           ControllerContext.ActionDescriptor.ControllerTypeInfo.Name,
           $"{ApiRoutes.Api}",
           $"{ApiRoutes.Product.GetAllProducts}");

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
    [Route($"{ApiRoutes.Product.DeleteBulkProduct}")]
    [JwtAuthorize($"{ApiRoutes.Role.Admin}")]
    public async Task<IActionResult> DeleteBulkProduct(List<string>? listProductId, CancellationToken cancellationToken)
    {
        DeleteBulkProductCommand command = new DeleteBulkProductCommand() { ListProductId = listProductId! };

        OperationResult<string> response = await _mediator.Send(command, cancellationToken);

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        string pattern = SystemConfig.GeneratePattern(
           ControllerContext.ActionDescriptor.ControllerTypeInfo.Name,
           $"{ApiRoutes.Api}",
           $"{ApiRoutes.Product.GetAllProducts}");

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
    [Route($"{ApiRoutes.Product.DeleteProduct}")]
    [ValidateGuid("productId")]
    public async Task<IActionResult> DeleteProduct(string? productId, CancellationToken cancellationToken)
    {
        Guid.TryParse(productId, out Guid id);

        DeleteProductCommand command = new DeleteProductCommand() { ProductId = id };

        OperationResult<string> response = await _mediator.Send(command, cancellationToken);

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        string pattern = SystemConfig.GeneratePattern(
           ControllerContext.ActionDescriptor.ControllerTypeInfo.Name,
           $"{ApiRoutes.Api}",
           $"{ApiRoutes.Product.GetAllProducts}");

        await _responseCacheService.RemoveCacheResponseAsync(pattern);

        return Ok(response);
    }
}