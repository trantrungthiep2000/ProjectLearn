using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.DAL.Data;
using Project.Infrastructure.Interfaces;

namespace Project.Infrastructure.Repositories;

/// <summary>
/// Information of idenity user repository
/// CreatedBy: ThiepTT(03/11/2023)
/// </summary>
public class IdentityUserRepository : BaseRepository<IdentityUser>, IIdentityUserRepository<IdentityUser>
{
    public IdentityUserRepository(DataContext dataContext) : base(dataContext) { }

    /// <summary>
    /// Get user by email async
    /// </summary>
    /// <param name="email">Email</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>Information of user</returns>
    /// CreatedBy: ThiepTT(03/11/2023)
    public async Task<IdentityUser> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        IdentityUser? identityUser = await _dataContext.Users
            .FirstOrDefaultAsync(user => user.Email!.Trim().ToLower().Equals(email.Trim().ToLower()), cancellationToken);

        return identityUser!;
    }
}