using Microsoft.EntityFrameworkCore;
using Project.DAL.Data;
using Project.Domain.Aggregates;
using Project.Infrastructure.Interfaces;

namespace Project.Infrastructure.Repositories;

/// <summary>
/// Information of user profile repository
/// CreatedBy: ThiepTT(03/11/2023)
/// </summary>
public class UserProfileRepository : BaseRepository<UserProfile>, IUserProfileRepository<UserProfile>
{
    public UserProfileRepository(DataContext dataContext) : base(dataContext)
    {
    }

    /// <summary>
    /// Get user profile by email
    /// </summary>
    /// <param name="email">Email</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>Information of entity</returns>
    /// CreatedBy: ThiepTT(03/11/2023)
    public async Task<UserProfile> GetUserProfileByEmail(string email, CancellationToken cancellationToken)
    {
        UserProfile? userProfile = await _dataContext.UserProfiles
            .FirstOrDefaultAsync(user => user.Email!.Trim().ToLower().Equals(email.Trim().ToLower()), cancellationToken);

        return userProfile!;
    }
}