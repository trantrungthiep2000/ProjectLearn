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

    public ProductControllerTest()
    {
        _mediatorMock = new Mock<IMediator>();
        _mapperMock = new Mock<IMapper>();
        _responseCacheServiceMock = new Mock<IResponseCacheService>();
    }

    [Fact]
    public async Task GetAllProducts_ReturnsOkResult()
    {
        // Arrange
        var productsController = new ProductsController(
            _mediatorMock.Object,
            _mapperMock.Object,
            _responseCacheServiceMock.Object);

        // Act
        var result = await productsController.GetAllProducts(CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
    }
}