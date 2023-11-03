namespace Project.Infrastructure.Interfaces;

/// <summary>
/// Information of interface base repository
/// CreatedBy: ThiepTT(03/11/2023)
/// </summary>
/// <typeparam name="E">Entity</typeparam>
public interface IBaseRepository<E> where E : class
{
    /// <summary>
    /// Get all async
    /// </summary>
    /// <returns>List of entity</returns>
    /// CreatedBy: ThiepTT(03/11/2023)
    public Task<IEnumerable<E>> GetAllAsync();

    /// <summary>
    /// Get by id async
    /// </summary>
    /// <param name="id">Id of entity</param>
    /// <returns>Information of entity</returns>
    /// CreatedBy: ThiepTT(03/11/2023)
    public Task<E> GetByIdAsync(Guid id);

    /// <summary>
    /// Create
    /// </summary>
    /// <param name="entity">Entity</param>
    /// CreatedBy: ThiepTT(03/11/2023)
    public void Create(E entity);

    /// <summary>
    /// Create bulk
    /// </summary>
    /// <param name="entities">List of entity</param>
    /// CreatedBy: ThiepTT(03/11/2023)
    public void CreateBulk(List<E> entities);

    /// <summary>
    /// Update
    /// </summary>
    /// <param name="entity">Entity</param>
    /// CreatedBy: ThiepTT(03/11/2023)
    public void Update(E entity);

    /// <summary>
    /// Update bulk
    /// </summary>
    /// <param name="entities">List of entity</param>
    /// CreatedBy: ThiepTT(03/11/2023)
    public void UpdateBulk(List<E> entities);

    /// <summary>
    /// Delete
    /// </summary>
    /// <param name="entity">Entity</param>
    /// CreatedBy: ThiepTT(03/11/2023)
    public void Delete(E entity);

    /// <summary>
    /// Delete bulk
    /// </summary>
    /// <param name="entities">List of entity</param>
    /// CreatedBy: ThiepTT(03/11/2023)
    public void DeleteBulk(List<E> entities);
}