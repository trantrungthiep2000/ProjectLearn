using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Project.Application.Dtos.Responses;
using Project.Application.Models;
using Project.Application.Products.Queries;
using Project.Domain.Aggregates;
using Project.Domain.Interfaces.IServices;
using Project.Presentation.APIs.V2;

namespace Project.Test.APITests;

/// <summary>
/// Information of product api test
/// CreatedBy: ThiepTT(18/12/2023)
/// </summary>
public class ProductApiTest
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IResponseCacheService> _responseCacheServiceMock;
    private readonly CancellationToken _cancellationToken;

    public ProductApiTest()
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
    public async Task GetAllProducts_Returns_OkObjectResult()
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

        ProductApi productApi = new ProductApi();

        // Act
        IResult result = await productApi.GetAllProducts(_mapperMock.Object, _mediatorMock.Object, _cancellationToken);

        // Assert
        Ok<OperationResult<IEnumerable<ProductResponse>>> okResult = Assert.IsType<Ok<OperationResult<IEnumerable<ProductResponse>>>>(result);
        OperationResult<IEnumerable<ProductResponse>> model = Assert.IsAssignableFrom<OperationResult<IEnumerable<ProductResponse>>>(okResult.Value);

        Assert.False(model.IsError);
        Assert.Single(model.Data);
    }
}