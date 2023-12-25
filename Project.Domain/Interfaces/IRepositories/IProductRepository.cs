namespace Project.Domain.Interfaces.IRepositories;

/// <summary>
/// Information of interface product repository
/// CreatedBy: ThiepTT(07/11/2023)
/// </summary>
/// <typeparam name="E">Entity</typeparam>
public interface IProductRepository<E> : IBaseRepository<E> where E : class { }