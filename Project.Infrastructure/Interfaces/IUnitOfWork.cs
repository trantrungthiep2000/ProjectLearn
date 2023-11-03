using Microsoft.EntityFrameworkCore.Storage;

namespace Project.Infrastructure.Interfaces;

/// <summary>
/// Information of interface unit of work
/// CreatedBy: ThiepTT(03/11/2023)
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Begin transaction async
    /// </summary>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>IDbContextTransaction</returns>
    /// CreatedBy: ThiepTT(03/11/2023)
    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Save changes async
    /// </summary>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>int</returns>
    /// CreatedBy: ThiepTT(03/11/2023)
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}