using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Project.DAL.Data;
using Project.Domain.Interfaces.IRepositories;
using System.Data;

namespace Project.Infrastructure.Repositories;

/// <summary>
/// Information of base repository
/// CreatedBy: ThiepTT(03/11/2023)
/// </summary>
/// <typeparam name="E">Entity</typeparam>
public class BaseRepository<E> : IBaseRepository<E> where E : class
{
    protected readonly DataContext _dataContext;
    protected readonly DbSet<E> _dbSet;

    public BaseRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
        _dbSet = _dataContext.Set<E>();
    }

    /// <summary>
    /// Get all asybc
    /// </summary>
    /// <returns>List of entity</returns>
    /// CreatedBy: ThiepTT(03/11/2023)
    public async Task<IEnumerable<E>> GetAllAsync()
    {
        using (IDbConnection dbConnection = new SqlConnection(_dataContext.Database.GetConnectionString()))
        {
            string sqlQuery = $"Proc_GetAll{typeof(E).Name}s";

            IEnumerable<E> entities = await dbConnection.QueryAsync<E>(sqlQuery, commandType: CommandType.StoredProcedure);

            return entities;
        }
    }

    /// <summary>
    /// Get by id async
    /// </summary>
    /// <param name="id">Id of entity</param>
    /// <returns>Information of entity</returns>
    public async Task<E> GetByIdAsync(Guid id)
    {
        using (IDbConnection dbConnection = new SqlConnection(_dataContext.Database.GetConnectionString()))
        {
            string sqlQuery = $"Proc_Get{typeof(E).Name}ById";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"@{typeof(E).Name}Id", id);

            E? entity = await dbConnection.QueryFirstOrDefaultAsync<E>(sqlQuery, param: parameters, commandType: CommandType.StoredProcedure);

            return entity!;
        }
    }

    /// <summary>
    /// Create
    /// </summary>
    /// <param name="entity">Entity</param>
    /// CreatedBy: ThiepTT(03/11/2023)
    public void Create(E entity)
    {
        _dbSet.Add(entity);
    }

    /// <summary>
    /// Create bulk
    /// </summary>
    /// <param name="entities">List of entity</param>
    /// CreatedBy: ThiepTT(03/11/2023)
    public void CreateBulk(List<E> entities)
    {
        _dbSet.AddRange(entities);
    }

    /// <summary>
    /// Update
    /// </summary>
    /// <param name="entity">Entity</param>
    /// CreatedBy: ThiepTT(03/11/2023)
    public void Update(E entity)
    {
        _dbSet.Update(entity);
    }

    /// <summary>
    /// Update bulk
    /// </summary>
    /// <param name="entities">List of entity</param>
    /// CreatedBy: ThiepTT(03/11/2023)
    public void UpdateBulk(List<E> entities)
    {
        _dbSet.UpdateRange(entities);
    }

    /// <summary>
    /// Delete
    /// </summary>
    /// <param name="entity">Entity</param>
    /// CreatedBy: ThiepTT(03/11/2023)
    public void Delete(E entity)
    {
        _dbSet.Remove(entity);
    }

    /// <summary>
    /// Delete bulk
    /// </summary>
    /// <param name="entities">List of entity</param>
    /// CreatedBy: ThiepTT(03/11/2023)
    public void DeleteBulk(List<E> entities)
    {
        _dbSet.RemoveRange(entities);
    }
}