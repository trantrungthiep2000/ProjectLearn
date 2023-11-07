using Project.DAL.Data;
using Project.Domain.Aggregates;
using Project.Infrastructure.Interfaces;

namespace Project.Infrastructure.Repositories;

/// <summary>
/// Information of product repository
/// CreatedBy: ThiepTT(07/11/2023)
/// </summary>
public class ProductRepository : BaseRepository<Product>, IProductRepository<Product>
{
    public ProductRepository(DataContext dataContext) : base(dataContext) { }
}