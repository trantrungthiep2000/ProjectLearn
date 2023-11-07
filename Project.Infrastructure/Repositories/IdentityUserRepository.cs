using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.DAL.Data;
using Project.Domain.Aggregates;
using Project.Infrastructure.Interfaces;
using System.Data;
using System.Data.SqlClient;

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
    /// <returns>Information of user</returns>
    /// CreatedBy: ThiepTT(03/11/2023)
    public async Task<IdentityUser> GetUserByEmailAsync(string email)
    {
        using (IDbConnection dbConnection = new SqlConnection(_dataContext.Database.GetConnectionString()))
        {
            string sqlQuery = $"Proc_GetUserByEmail";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"@Email", email);

            IdentityUser? ideityUser = await dbConnection
                .QueryFirstOrDefaultAsync<IdentityUser>(sqlQuery, param: parameters, commandType: CommandType.StoredProcedure);

            return ideityUser!;
        }
    }
}