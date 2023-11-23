using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Project.API.Controllers.V1;
using Project.Application.Dtos.Responses;
using Project.Application.Models;
using Project.Application.Products.Queries;
using Project.Application.Services.IServices;
using Project.Domain.Aggregates;

namespace Project.Test.ControllerTests;

public class ProductControllerTest
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IResponseCacheService> _responseCacheServiceMock;
    private readonly CancellationToken _cancellationToken;

    public ProductControllerTest()
    {
        _mediatorMock = new Mock<IMediator>();
        _mapperMock = new Mock<IMapper>();
        _responseCacheServiceMock = new Mock<IResponseCacheService>();
        _cancellationToken = new CancellationToken();
    }

    /// <summary>
    /// Get all products returns ok result
    /// </summary>
    /// <returns>Task</returns>
    /// CreatedBy: ThiepTT(23/11/2023)
    [Fact]
    public async Task GetAllProducts_Returns_OkResult()
    {
        // Arrange
        var productResponse = new ProductResponse
        {
            ProductId = Guid.Parse("fd03dba4-eef9-44f8-e324-08dbe1bd3015"),
            ProductName = "iphone 15 pro max",
            Price = 30000000,
            Description = "this is a iphone"
        };

        OperationResult<IEnumerable<ProductResponse>> response = new OperationResult<IEnumerable<ProductResponse>>
        {
            Data = new List<ProductResponse> { productResponse }
        };

        // Setup the mediator to return the mock response
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllProductsQuery>(), _cancellationToken)).ReturnsAsync(new OperationResult<IEnumerable<Product>>());

        // Setup the mapper to return the mock response
        _mapperMock.Setup(m => m.Map<OperationResult<IEnumerable<ProductResponse>>>(It.IsAny<OperationResult<IEnumerable<Product>>>())).Returns(response);

        ProductsController productsController = new ProductsController(_mediatorMock.Object, _mapperMock.Object, _responseCacheServiceMock.Object);

        // Act
        IActionResult result = await productsController.GetAllProducts(_cancellationToken);

        // Assert
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        OperationResult<IEnumerable<ProductResponse>> model = Assert.IsAssignableFrom<OperationResult<IEnumerable<ProductResponse>>>(okResult.Value);

        Assert.True(!model.IsError);
        Assert.Single(model.Data);
    }

    [Fact]
    public async Task GetProductById_Returns_OkResult()
    {
        // Arrange
        ProductResponse productResponse = new ProductResponse
        {
            ProductId = Guid.Parse("fd03dba4-eef9-44f8-e324-08dbe1bd3015"),
            ProductName = "iphone 15 pro max",
            Price = 30000000,
            Description = "this is a iphone"
        };

        OperationResult<ProductResponse> response = new OperationResult<ProductResponse>
        {
            Data = productResponse
        };

        // Setup the mediator to return the mock response
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetProductByIdQuery>(), _cancellationToken)).ReturnsAsync(new OperationResult<Product>());

        // Setup the mapper to return the mock response
        _mapperMock.Setup(m => m.Map<OperationResult<ProductResponse>>(It.IsAny<OperationResult<Product>>())).Returns(response);

        ProductsController productsController = new ProductsController(_mediatorMock.Object, _mapperMock.Object, _responseCacheServiceMock.Object);

        // Act
        IActionResult result = await productsController.GetProductById(productResponse.ProductId.ToString(), _cancellationToken);

        // Assert
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        OperationResult<ProductResponse> model = Assert.IsAssignableFrom<OperationResult<ProductResponse>>(okResult.Value);

        Assert.True(!model.IsError);
        Assert.Equal("fd03dba4-eef9-44f8-e324-08dbe1bd3015", model.Data.ProductId.ToString());
    }
}