namespace Project.Domain.Interfaces.IRepositories;

/// <summary>
/// Information of interface identity user repository
/// CreatedBy: ThiepTT(03/11/2023)
/// </summary>
/// <typeparam name="E">Entity</typeparam>
public interface IIdentityUserRepository<E> : IBaseRepository<E> where E : class
{
    /// <summary>
    /// Get user by email
    /// </summary>
    /// <param name="email">Email</param>
    /// <returns>Information of entity</returns>
    /// CreatedBy: ThiepTT(03/11/2023)
    public Task<E> GetUserByEmailAsync(string email);
}