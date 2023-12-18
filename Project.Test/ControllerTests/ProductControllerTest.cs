using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Project.API.Controllers.V1;
using Project.API.Options;
using Project.Application.Dtos.Requests;
using Project.Application.Dtos.Responses;
using Project.Application.Messages;
using Project.Application.Models;
using Project.Application.Products.Commands;
using Project.Application.Products.Queries;
using Project.Application.Services.IServices;
using Project.Domain.Aggregates;

namespace Project.Test.ControllerTests;

/// <summary>
/// Information of product controller test
/// CreatedBy: ThiepTT(12/12/2023)
/// </summary>
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

        ProductsController productsController = new ProductsController(_mediatorMock.Object, _mapperMock.Object, _responseCacheServiceMock.Object);

        // Act
        IActionResult result = await productsController.GetAllProducts(_cancellationToken);

        // Assert
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        OperationResult<IEnumerable<ProductResponse>> model = Assert.IsAssignableFrom<OperationResult<IEnumerable<ProductResponse>>>(okResult.Value);

        Assert.False(model.IsError);
        Assert.Single(model.Data);
    }

    /// <summary>
    /// Get product by id returns ok result
    /// </summary>
    /// <returns>Task</returns>
    /// CreatedBy: ThiepTT(24/11/2023)
    [Fact]
    public async Task GetProductById_Returns_OkObjectResult()
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

        Assert.False(model.IsError);
        Assert.Equal("fd03dba4-eef9-44f8-e324-08dbe1bd3015", model.Data.ProductId.ToString());
    }

    /// <summary>
    /// Get product by id product not found returns not found object result
    /// </summary>
    /// <returns>Task</returns>
    /// CreatedBy: ThiepTT(24/11/2023)
    [Fact]
    public async Task GetProductById_ProductNotFound_Returns_NotFoundObjectResult()
    {
        // Arrange
        Guid productId = Guid.NewGuid();

        OperationResult<ProductResponse> response = new OperationResult<ProductResponse>();
        response.AddError(ErrorCode.NotFound, $"No find Product with ID {productId}");

        // Setup the mediator to return the mock response
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetProductByIdQuery>(), _cancellationToken)).ReturnsAsync(new OperationResult<Product>());

        // Setup the mapper to return the mock response
        _mapperMock.Setup(m => m.Map<OperationResult<ProductResponse>>(It.IsAny<OperationResult<Product>>())).Returns(response);

        ProductsController productsController = new ProductsController(_mediatorMock.Object, _mapperMock.Object, _responseCacheServiceMock.Object);

        // Act
        IActionResult result = await productsController.GetProductById(productId.ToString(), _cancellationToken);

        // Assert
        NotFoundObjectResult notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        ErrorResponse model = Assert.IsAssignableFrom<ErrorResponse>(notFoundResult.Value);

        Assert.Equal(404, model.StatusCode);
        Assert.Equal($"No find Product with ID {productId}", model.Errors[0]);
    }

    /// <summary>
    /// Get product by id product not found returns not found object result
    /// </summary>
    /// <returns>Task</returns>
    /// CreatedBy: ThiepTT(24/11/2023)
    [Fact]
    public async Task CreateProduct_Returns_OkObjectResult()
    {
        // Arrange
        ProductRequest product = new ProductRequest()
        {
            ProductName = "Name test",
            Price = 20000000,
            Description = "Description test"
        };
        string fullName = "thiep tran trung";
        string pattern = SystemConfig.GeneratePattern($"{nameof(ApiRoutes.Products)}", $"{ApiRoutes.Api}", $"{ApiRoutes.Products.GetAllProducts}");

        CreateProductCommand createProductCommand = new CreateProductCommand()
        {
            ProductName = product.ProductName,
            Price = product.Price,
            Description = product.Description,
            CreatedBy = fullName,
        };

        OperationResult<string> response = new OperationResult<string>()
        {
            Data = ResponseMessage.Product.CreateProductSuccess,
        };

        // Setup the mapper to return the mock request
        _mapperMock.Setup(m => m.Map<CreateProductCommand>(product)).Returns(createProductCommand);

        // Setup the mediator to return the mock response
        _mediatorMock.Setup(m => m.Send(createProductCommand, _cancellationToken)).ReturnsAsync(response);

        // Setup the response cache service to return mock response
        _responseCacheServiceMock.Setup(r => r.RemoveCacheResponseAsync(pattern)).Returns(Task.CompletedTask);

        ProductsController productsController = new ProductsController(_mediatorMock.Object, _mapperMock.Object, _responseCacheServiceMock.Object);

        // Act
        IActionResult result = await productsController.CreateProduct(product, _cancellationToken);

        // Assert
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        OperationResult<string> model = Assert.IsAssignableFrom<OperationResult<string>>(okResult.Value);

        Assert.False(model.IsError);
        Assert.Equal(ResponseMessage.Product.CreateProductSuccess, model.Data);
    }

    /// <summary>
    /// Create product product name null retruns bad request object result
    /// </summary>
    /// <returns>Task</returns>
    /// CreatedBy: ThiepTT(24/11/2023)
    [Fact]
    public async Task CreateProduct_ProductName_Null_Returns_BadRequestObjectResult()
    {
        // Arrange
        ProductRequest product = new ProductRequest()
        {
            ProductName = string.Empty,
            Price = 20000000,
            Description = "Description test"
        };
        string fullName = "thiep tran trung";
        string pattern = SystemConfig.GeneratePattern($"{nameof(ApiRoutes.Products)}", $"{ApiRoutes.Api}", $"{ApiRoutes.Products.GetAllProducts}");

        CreateProductCommand createProductCommand = new CreateProductCommand()
        {
            ProductName = product.ProductName,
            Price = product.Price,
            Description = product.Description,
            CreatedBy = fullName,
        };

        OperationResult<string> response = new OperationResult<string>();
        response.AddError(ErrorCode.BadRequest, $"Product name cannot be empty");

        // Setup the mapper to return the mock request
        _mapperMock.Setup(m => m.Map<CreateProductCommand>(product)).Returns(createProductCommand);

        // Setup the mediator to return the mock response
        _mediatorMock.Setup(m => m.Send(createProductCommand, _cancellationToken)).ReturnsAsync(response);

        // Setup the response cache service to return mock response
        _responseCacheServiceMock.Setup(r => r.RemoveCacheResponseAsync(pattern)).Returns(Task.CompletedTask);

        ProductsController productsController = new ProductsController(_mediatorMock.Object, _mapperMock.Object, _responseCacheServiceMock.Object);

        // Act
        IActionResult result = await productsController.CreateProduct(product, _cancellationToken);

        // Assert
        BadRequestObjectResult okResult = Assert.IsType<BadRequestObjectResult>(result);
        ErrorResponse model = Assert.IsAssignableFrom<ErrorResponse>(okResult.Value);

        Assert.Equal(400, model.StatusCode);
        Assert.Equal("Product name cannot be empty", model.Errors[0]);
    }

    /// <summary>
    /// Create product product name min character retruns bad request object result
    /// </summary>
    /// <returns>Task</returns>
    /// CreatedBy: ThiepTT(30/11/2023)
    [Fact]
    public async Task CreateProduct_ProductName_MinCharacter_Returns_BadRequestObjectResult()
    {
        // Arrange
        ProductRequest product = new ProductRequest()
        {
            ProductName = "a",
            Price = 20000000,
            Description = "Description test"
        };
        string fullName = "thiep tran trung";
        string pattern = SystemConfig.GeneratePattern($"{nameof(ApiRoutes.Products)}", $"{ApiRoutes.Api}", $"{ApiRoutes.Products.GetAllProducts}");

        CreateProductCommand createProductCommand = new CreateProductCommand()
        {
            ProductName = product.ProductName,
            Price = product.Price,
            Description = product.Description,
            CreatedBy = fullName,
        };

        OperationResult<string> response = new OperationResult<string>();
        response.AddError(ErrorCode.BadRequest, $"Product name must be at least 1 character long");

        // Setup the mapper to return the mock request
        _mapperMock.Setup(m => m.Map<CreateProductCommand>(product)).Returns(createProductCommand);

        // Setup the mediator to return the mock response
        _mediatorMock.Setup(m => m.Send(createProductCommand, _cancellationToken)).ReturnsAsync(response);

        // Setup the response cache service to return mock response
        _responseCacheServiceMock.Setup(r => r.RemoveCacheResponseAsync(pattern)).Returns(Task.CompletedTask);

        ProductsController productsController = new ProductsController(_mediatorMock.Object, _mapperMock.Object, _responseCacheServiceMock.Object);

        // Act
        IActionResult result = await productsController.CreateProduct(product, _cancellationToken);

        // Assert
        BadRequestObjectResult okResult = Assert.IsType<BadRequestObjectResult>(result);
        ErrorResponse model = Assert.IsAssignableFrom<ErrorResponse>(okResult.Value);

        Assert.Equal(400, model.StatusCode);
        Assert.Equal("Product name must be at least 1 character long", model.Errors[0]);
    }

    /// <summary>
    /// Create product product name max character retruns bad request object result
    /// </summary>
    /// <returns>Task</returns>
    /// CreatedBy: ThiepTT(30/11/2023)
    [Fact]
    public async Task CreateProduct_ProductName_MaxCharacter_Returns_BadRequestObjectResult()
    {
        // Arrange
        ProductRequest product = new ProductRequest()
        {
            ProductName = "abcdefghikabcdefghikabcdefghikabcdefghikabcdefghik",
            Price = 20000000,
            Description = "Description test"
        };
        string fullName = "thiep tran trung";
        string pattern = SystemConfig.GeneratePattern($"{nameof(ApiRoutes.Products)}", $"{ApiRoutes.Api}", $"{ApiRoutes.Products.GetAllProducts}");

        CreateProductCommand createProductCommand = new CreateProductCommand()
        {
            ProductName = product.ProductName,
            Price = product.Price,
            Description = product.Description,
            CreatedBy = fullName,
        };

        OperationResult<string> response = new OperationResult<string>();
        response.AddError(ErrorCode.BadRequest, $"Product name must be at least 1 character long");

        // Setup the mapper to return the mock request
        _mapperMock.Setup(m => m.Map<CreateProductCommand>(product)).Returns(createProductCommand);

        // Setup the mediator to return the mock response
        _mediatorMock.Setup(m => m.Send(createProductCommand, _cancellationToken)).ReturnsAsync(response);

        // Setup the response cache service to return mock response
        _responseCacheServiceMock.Setup(r => r.RemoveCacheResponseAsync(pattern)).Returns(Task.CompletedTask);

        ProductsController productsController = new ProductsController(_mediatorMock.Object, _mapperMock.Object, _responseCacheServiceMock.Object);

        // Act
        IActionResult result = await productsController.CreateProduct(product, _cancellationToken);

        // Assert
        BadRequestObjectResult okResult = Assert.IsType<BadRequestObjectResult>(result);
        ErrorResponse model = Assert.IsAssignableFrom<ErrorResponse>(okResult.Value);

        Assert.Equal(400, model.StatusCode);
        Assert.Equal("Product name must be at least 1 character long", model.Errors[0]);
    }

    /// <summary>
    /// Create product price null returns bad request object result
    /// </summary>
    /// <returns>Task</returns>
    /// CreatedBy: ThiepTT(30/11/2023)
    [Fact]
    public async Task CreateProduct_Price_Null_Returns_BadRequestObjectResult()
    {
        // Arrange
        ProductRequest product = new ProductRequest()
        {
            ProductName = "Name test",
            Price = 0,
            Description = "Description test"
        };
        string fullName = "thiep tran trung";
        string pattern = SystemConfig.GeneratePattern($"{nameof(ApiRoutes.Products)}", $"{ApiRoutes.Api}", $"{ApiRoutes.Products.GetAllProducts}");

        CreateProductCommand createProductCommand = new CreateProductCommand()
        {
            ProductName = product.ProductName,
            Price = product.Price,
            Description = product.Description,
            CreatedBy = fullName,
        };

        OperationResult<string> response = new OperationResult<string>();
        response.AddError(ErrorCode.BadRequest, $"Price cannot be empty");

        // Setup the mapper to return the mock request
        _mapperMock.Setup(m => m.Map<CreateProductCommand>(product)).Returns(createProductCommand);

        // Setup the mediator to return the mock response
        _mediatorMock.Setup(m => m.Send(createProductCommand, _cancellationToken)).ReturnsAsync(response);

        // Setup the response cache service to return mock response
        _responseCacheServiceMock.Setup(r => r.RemoveCacheResponseAsync(pattern)).Returns(Task.CompletedTask);

        ProductsController productsController = new ProductsController(_mediatorMock.Object, _mapperMock.Object, _responseCacheServiceMock.Object);

        // Act
        IActionResult result = await productsController.CreateProduct(product, _cancellationToken);

        // Assert
        BadRequestObjectResult okResult = Assert.IsType<BadRequestObjectResult>(result);
        ErrorResponse model = Assert.IsAssignableFrom<ErrorResponse>(okResult.Value);

        Assert.Equal(400, model.StatusCode);
        Assert.Equal("Price cannot be empty", model.Errors[0]);
    }

    /// <summary>
    /// Create product description null returns bad request object result
    /// </summary>
    /// <returns>Task</returns>
    /// CreatedBy: ThiepTT(30/11/2023)
    [Fact]
    public async Task CreateProduct_Description_Null_Returns_BadRequestObjectResult()
    {
        // Arrange
        ProductRequest product = new ProductRequest()
        {
            ProductName = "Name test",
            Price = 0,
            Description = string.Empty
        };
        string fullName = "thiep tran trung";
        string pattern = SystemConfig.GeneratePattern($"{nameof(ApiRoutes.Products)}", $"{ApiRoutes.Api}", $"{ApiRoutes.Products.GetAllProducts}");

        CreateProductCommand createProductCommand = new CreateProductCommand()
        {
            ProductName = product.ProductName,
            Price = product.Price,
            Description = product.Description,
            CreatedBy = fullName,
        };

        OperationResult<string> response = new OperationResult<string>();
        response.AddError(ErrorCode.BadRequest, $"Price cannot be empty");

        // Setup the mapper to return the mock request
        _mapperMock.Setup(m => m.Map<CreateProductCommand>(product)).Returns(createProductCommand);

        // Setup the mediator to return the mock response
        _mediatorMock.Setup(m => m.Send(createProductCommand, _cancellationToken)).ReturnsAsync(response);

        // Setup the response cache service to return mock response
        _responseCacheServiceMock.Setup(r => r.RemoveCacheResponseAsync(pattern)).Returns(Task.CompletedTask);

        ProductsController productsController = new ProductsController(_mediatorMock.Object, _mapperMock.Object, _responseCacheServiceMock.Object);

        // Act
        IActionResult result = await productsController.CreateProduct(product, _cancellationToken);

        // Assert
        BadRequestObjectResult okResult = Assert.IsType<BadRequestObjectResult>(result);
        ErrorResponse model = Assert.IsAssignableFrom<ErrorResponse>(okResult.Value);

        Assert.Equal(400, model.StatusCode);
        Assert.Equal("Price cannot be empty", model.Errors[0]);
    }

    /// <summary>
    /// Delete product returns ok object result
    /// </summary>
    /// <returns>Task</returns>
    /// CreatedBy: ThiepTT(30/11/2023)
    [Fact]
    public async Task DeleteProduct_Returns_OkObjectResult()
    {
        // Arrange
        Guid productId = Guid.Parse("fd03dba4-eef9-44f8-e324-08dbe1bd3015");
        string pattern = SystemConfig.GeneratePattern($"{nameof(ApiRoutes.Products)}", $"{ApiRoutes.Api}", $"{ApiRoutes.Products.GetAllProducts}");

        OperationResult<string> response = new OperationResult<string> { Data = ResponseMessage.Product.DeleteProductSuccess };

        DeleteProductCommand deleteProductCommand = new DeleteProductCommand() { ProductId = productId };

        // Setup the mediator to return the mock response
        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteProductCommand>(), _cancellationToken)).ReturnsAsync(response);

        // Setup the response cache service to return mock response
        _responseCacheServiceMock.Setup(r => r.RemoveCacheResponseAsync(pattern)).Returns(Task.CompletedTask);

        ProductsController productsController = new ProductsController(_mediatorMock.Object, _mapperMock.Object, _responseCacheServiceMock.Object);

        // Act
        IActionResult result = await productsController.DeleteProduct(productId.ToString(), _cancellationToken);

        // Assert
        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        OperationResult<string> model = Assert.IsAssignableFrom<OperationResult<string>>(okResult.Value);

        Assert.False(model.IsError);
        Assert.Equal(ResponseMessage.Product.DeleteProductSuccess, model.Data.ToString());
    }

    /// <summary>
    /// Delete product product not found returns not found object result
    /// </summary>
    /// <returns>Task</returns>
    /// CreatedBy: ThiepTT(30/11/2023)
    [Fact]
    public async Task DeleteProduct_ProductNotFound_Returns_NotFoundObjectResult()
    {
        // Arrange
        Guid productId = Guid.NewGuid();
        string pattern = SystemConfig.GeneratePattern($"{nameof(ApiRoutes.Products)}", $"{ApiRoutes.Api}", $"{ApiRoutes.Products.GetAllProducts}");

        DeleteProductCommand deleteProductCommand = new DeleteProductCommand() { ProductId = productId };

        OperationResult<string> response = new OperationResult<string>();
        response.AddError(ErrorCode.NotFound, $"No find Product with ID {productId}");

        // Setup the mediator to return the mock response
        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteProductCommand>(), _cancellationToken)).ReturnsAsync(response);

        // Setup the response cache service to return mock response
        _responseCacheServiceMock.Setup(r => r.RemoveCacheResponseAsync(pattern)).Returns(Task.CompletedTask);

        ProductsController productsController = new ProductsController(_mediatorMock.Object, _mapperMock.Object, _responseCacheServiceMock.Object);

        // Act
        IActionResult result = await productsController.DeleteProduct(productId.ToString(), _cancellationToken);

        // Assert
        NotFoundObjectResult notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        ErrorResponse model = Assert.IsAssignableFrom<ErrorResponse>(notFoundResult.Value);

        Assert.Equal(404, model.StatusCode);
        Assert.Equal($"No find Product with ID {productId}", model.Errors[0]);
    }
}