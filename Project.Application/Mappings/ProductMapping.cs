using AutoMapper;
using Project.Application.Dtos.Responses;
using Project.Application.Models;

namespace Project.Application.Mappings;

/// <summary>
/// Information of product mapping
/// CreatedBy: ThiepTT(07/11/2023)
/// </summary>
public class ProductMapping : Profile
{
    public ProductMapping()
    {
        CreateMap<OperationResult<IEnumerable<ProductResponse>>, OperationResult<IEnumerable<ProductResponse>>>().ReverseMap();
    }
}