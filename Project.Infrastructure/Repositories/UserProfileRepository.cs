using Dapper;
using Microsoft.EntityFrameworkCore;
using Project.DAL.Data;
using Project.Domain.Aggregates;
using Project.Domain.Interfaces.IRepositories;
using System.Data;
using System.Data.SqlClient;

namespace Project.Infrastructure.Repositories;

/// <summary>
/// Information of user profile repository
/// CreatedBy: ThiepTT(03/11/2023)
/// </summary>
public class UserProfileRepository : BaseRepository<UserProfile>, IUserProfileRepository<UserProfile>
{
    public UserProfileRepository(DataContext dataContext) : base(dataContext) { }

    /// <summary>
    /// Get user profile by email
    /// </summary>
    /// <param name="email">Email</param>
    /// <returns>Information of entity</returns>
    /// CreatedBy: ThiepTT(03/11/2023)
    public async Task<UserProfile> GetUserProfileByEmail(string email)
    {
        using (IDbConnection dbConnection = new SqlConnection(_dataContext.Database.GetConnectionString()))
        {
            string sqlQuery = $"Proc_GetUserProfileByEmail";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"@Email", email);

            UserProfile? userProfile = await dbConnection
                .QueryFirstOrDefaultAsync<UserProfile>(sqlQuery, param: parameters, commandType: CommandType.StoredProcedure);

            return userProfile!;
        }
    }
}