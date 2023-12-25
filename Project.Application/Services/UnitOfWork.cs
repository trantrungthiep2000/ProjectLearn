using Microsoft.EntityFrameworkCore.Storage;
using Project.DAL.Data;
using Project.Domain.Interfaces.IServices;

namespace Project.Application.Services;

/// <summary>
/// Information of unit of work
/// CreatedBy: ThiepTT(03/11/2023)
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _dataContext;

    public UnitOfWork(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    /// <summary>
    /// Begin transaction async
    /// </summary>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>IDbContextTransaction</returns>
    /// CreatedBy: ThiepTT(03/11/2023)
    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        return await _dataContext.Database.BeginTransactionAsync(cancellationToken);
    }

    /// <summary>
    /// Save changes async
    /// </summary>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>int</returns>
    /// CreatedBy: ThiepTT(03/11/2023)
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _dataContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Dispose
    /// </summary>
    /// CreatedBy: ThiepTT(03/11/2023)
    public void Dispose()
    {
        _dataContext.Dispose();
    }
}