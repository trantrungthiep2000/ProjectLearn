using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Project.Application.Models;
using Project.Application.Users.Queries;
using Project.DAL.Data;
using Project.Domain.Aggregates;
using System.Data;

namespace Project.Application.Users.QueryHandlers;

/// <summary>
/// Information of get user profile by id query handler
/// CreatedBy: ThiepTT(31/10/2023)
/// </summary>
public class GetUserProfileByIdQueryHandler : IRequestHandler<GetUserProfileByIdQuery, OperationResult<UserProfile>>
{
    private readonly DataContext _dataContext;

    public GetUserProfileByIdQueryHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    /// <summary>
    /// Handle
    /// </summary>
    /// <param name="request">GetUserProfileByIdQuery</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>Information of user profile</returns>
    /// CreatedBy: ThiepTT(31/10/2023)
    public async Task<OperationResult<UserProfile>> Handle(GetUserProfileByIdQuery request, CancellationToken cancellationToken)
    {
        OperationResult<UserProfile> result = new OperationResult<UserProfile>();

        try
        {
            using (IDbConnection dbConnection = new SqlConnection(_dataContext.Database.GetConnectionString()))
            {
                string sqlQuery = "Proc_GetUserProfileById";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserProfileId", request.UserProfileId);
                UserProfile? userProfile = await dbConnection
                    .QueryFirstOrDefaultAsync<UserProfile>(sqlQuery, param: parameters, commandType: CommandType.StoredProcedure);

                if (userProfile is null) 
                {
                    result.AddError(ErrorCode.NotFound, string.Format(UserProfileErrorMessage.UserProfileNotFound, request.UserProfileId));
                    return result;
                }

                result.Data = userProfile;
            }
        }
        catch (Exception ex)
        {
            result.AddError(ErrorCode.InternalServerError, ex.Message);
        }

        return result;  
    }
}