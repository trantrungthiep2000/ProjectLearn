using AutoMapper;
using Project.Application.Dtos.Requests;
using Project.Application.Dtos.Responses;
using Project.Application.Models;
using Project.Application.Products.Commands;
using Project.Domain.Aggregates;

namespace Project.Application.Mappings;

/// <summary>
/// Information of product mapping
/// CreatedBy: ThiepTT(07/11/2023)
/// </summary>
public class ProductMapping : Profile
{
    public ProductMapping()
    {
        CreateMap<OperationResult<IEnumerable<ProductResponse>>, OperationResult<IEnumerable<Product>>>().ReverseMap();
        CreateMap<OperationResult<ProductResponse>, OperationResult<Product>>().ReverseMap();
        CreateMap<ProductResponse, Product>().ReverseMap();
        CreateMap<CreateProductCommand, ProductRequest>().ReverseMap();
        CreateMap<UpdateProductCommand, ProductRequest>().ReverseMap();
    }
}