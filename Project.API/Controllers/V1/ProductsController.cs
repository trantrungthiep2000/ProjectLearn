using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.API.Attributes;
using Project.API.Options;
using Project.Application.Dtos.Responses;
using Project.Application.Models;
using Project.Application.Products.Queries;

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

    /// <summary>
    /// Products controller
    /// </summary>
    /// <param name="mediator">IMediator</param>
    /// <param name="mapper">IMapper</param>
    /// CreatedBy: ThiepTT(07/11/2023)
    public ProductsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
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
    /// Get all products
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

        OperationResult<IEnumerable<ProductResponse>> response = _mapper
            .Map<OperationResult<IEnumerable<ProductResponse>>>(await _mediator.Send(query, cancellationToken));

        if (response.IsError) { return HandlerErrorResponse(response.Errors); }

        return Ok(response);
    }
}