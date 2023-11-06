using MediatR;
using Microsoft.EntityFrameworkCore;
using Project.Application.Models;
using Project.Application.Users.Queries;
using Project.DAL.Data;
using Project.Domain.Aggregates;

namespace Project.Application.Users.QueryHandlers;

/// <summary>
/// Information of get all user profiles entity framework query handler
/// CreatedBy: ThiepTT(06/11/2023)
/// </summary>
public class GetAllUserProfilesEFQueryHandler : IRequestHandler<GetAllUserProfilesEFQuery, OperationResult<IEnumerable<UserProfile>>>
{
    private readonly DataContext _dataContext;

    public GetAllUserProfilesEFQueryHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    /// <summary>
    /// Handle
    /// </summary>
    /// <param name="request">GetAllUserProfilesEFQuery</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>List of user profile</returns>
    /// CreatedBy: ThiepTT(06/11/2023)
    public async Task<OperationResult<IEnumerable<UserProfile>>> Handle(GetAllUserProfilesEFQuery request, CancellationToken cancellationToken)
    {
        OperationResult<IEnumerable<UserProfile>> result = new OperationResult<IEnumerable<UserProfile>>();

        try
        {
            IEnumerable<UserProfile> userProfiles = await _dataContext.UserProfiles.ToListAsync();

            result.Data = userProfiles;
        }
        catch (Exception ex)
        {
            result.AddError(ErrorCode.InternalServerError, ex.Message);
        }

        return result;
    }
}