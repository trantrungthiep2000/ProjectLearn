using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using OfficeOpenXml;
using Project.Application.Messages;
using Project.Application.Models;
using Project.Application.Products.Commands;
using Project.Application.Validators;
using Project.Domain.Aggregates;
using Project.Domain.Interfaces.IRepositories;
using Project.Domain.Interfaces.IServices;

namespace Project.Application.Products.CommandHandlers;

/// <summary>
/// Information of Create bulk product command handler
/// CreatedBy: ThiepTT(10/11/2023)
/// </summary>
public class CreateBulkProductCommandHandler : IRequestHandler<CreateBulkProductCommand, OperationResult<string>>
{
    private readonly IProductRepository<Product> _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBulkProductCommandHandler(IProductRepository<Product> productRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Handle
    /// </summary>
    /// <param name="request">CreateBulkProductCommand</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>Message created product</returns>
    /// CreatedBy: ThiepTT(10/11/2023)
    public async Task<OperationResult<string>> Handle(CreateBulkProductCommand request, CancellationToken cancellationToken)
    {
        OperationResult<string> result = new OperationResult<string>();

        await using IDbContextTransaction transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            // Check file empty
            if (request.File is null || request.File.Length <= 0)
            {
                result.AddError(ErrorCode.BadRequest, ErrorMessage.FileEmpty);
                return result;
            }

            // Create bulk product
            List<Product> products = CreateBulkProduct(request.File, request.CreatedBy);

            foreach (Product product in products)
            {
                // Validate request of product
                if (ValidateProduct(product, result)) { return result; }
            }

            _productRepository.CreateBulk(products);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            result.Data = ResponseMessage.Product.CreateProductSuccess;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            result.AddError(ErrorCode.InternalServerError, ex.Message);
        }

        return result;
    }

    /// <summary>
    /// Validate product
    /// </summary>
    /// <param name="product">Product</param>
    /// <param name="result">OperationResult<string></param>
    /// <returns>True | False</returns>
    /// CreatedBy: ThiepTT(10/11/2023)
    private bool ValidateProduct(Product product, OperationResult<string> result)
    {
        ProductValidator validator = new ProductValidator();

        ValidationResult validationRegister = validator.Validate(product);

        if (!validationRegister.IsValid)
        {
            foreach (ValidationFailure error in validationRegister.Errors)
            {
                result.AddError(ErrorCode.BadRequest, error.ErrorMessage);
            }

            return true;
        }

        return false;
    }

    /// <summary>
    /// Create bulk product
    /// </summary>
    /// <param name="file">IFormFile</param>
    /// <param name="createdBy">Created by</param>
    /// <returns>List product</returns>
    /// CreatedBy: ThiepTT(10/11/2023)
    private List<Product> CreateBulkProduct(IFormFile file, string createdBy)
    {
        List<Product> products = new List<Product>();

        using (var stream = new MemoryStream())
        {
            file.CopyTo(stream);

            using (var package = new ExcelPackage(stream))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    Product product = Product.CreateProduct(worksheet.Cells[row, 1].Value?.ToString()!,
                        Convert.ToDouble(worksheet.Cells[row, 2].Value?.ToString()),
                        worksheet.Cells[row, 3].Value?.ToString()!,
                        createdBy);

                    products.Add(product);
                }
            }
        }

        return products;
    }
}