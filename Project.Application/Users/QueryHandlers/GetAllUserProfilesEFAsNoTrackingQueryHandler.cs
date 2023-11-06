using MediatR;
using Microsoft.EntityFrameworkCore;
using Project.Application.Models;
using Project.Application.Users.Queries;
using Project.DAL.Data;
using Project.Domain.Aggregates;

namespace Project.Application.Users.QueryHandlers;

/// <summary>
/// Information of get all user profiles entity framework as no tracking query handler
/// CreatedBy: ThiepTT(06/11/2023)
/// </summary>
public class GetAllUserProfilesEFAsNoTrackingQueryHandler : IRequestHandler<GetAllUserProfilesEFAsNoTrackingQuery, OperationResult<IEnumerable<UserProfile>>>
{
    private readonly DataContext _dataContext;

    public GetAllUserProfilesEFAsNoTrackingQueryHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    /// <summary>
    /// Handle
    /// </summary>
    /// <param name="request">GetAllUserProfilesEFAsNoTrackingQuery</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>List of user profile</returns>
    /// CreatedBy: ThiepTT(06/11/2023)
    public async Task<OperationResult<IEnumerable<UserProfile>>> Handle(GetAllUserProfilesEFAsNoTrackingQuery request, CancellationToken cancellationToken)
    {
        OperationResult<IEnumerable<UserProfile>> result = new OperationResult<IEnumerable<UserProfile>>();

        try
        {
            IEnumerable<UserProfile> userProfiles = await _dataContext.UserProfiles.AsNoTracking().ToListAsync();

            result.Data = userProfiles;
        }
        catch (Exception ex)
        {
            result.AddError(ErrorCode.InternalServerError, ex.Message);
        }

        return result;
    }
}