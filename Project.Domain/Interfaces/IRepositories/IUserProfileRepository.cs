﻿namespace Project.Domain.Interfaces.IRepositories;

/// <summary>
/// Information of interface user profile repository
/// CreatedBy: ThiepTT(03/11/2023)
/// </summary>
/// <typeparam name="E">Entity</typeparam>
public interface IUserProfileRepository<E> : IBaseRepository<E> where E : class
{
    /// <summary>
    /// Get user profile by email
    /// </summary>
    /// <param name="email">Email</param>
    /// <returns>Information of entity</returns>
    /// CreatedBy: ThiepTT(03/11/2023)
    public Task<E> GetUserProfileByEmail(string email);
}