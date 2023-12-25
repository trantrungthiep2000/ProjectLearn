using MediatR;
using Project.Application.Models;
using Project.Application.Users.Queries;
using Project.Domain.Aggregates;
using Project.Domain.Interfaces.IRepositories;

namespace Project.Application.Users.QueryHandlers;

/// <summary>
/// Information of get all user profiles query handler
/// CreatedBy: ThiepTT(06/11/2023)
/// </summary>
public class GetAllUserProfilesQueryHandler : IRequestHandler<GetAllUserProfilesQuery, OperationResult<IEnumerable<UserProfile>>>
{
    private readonly IUserProfileRepository<UserProfile> _userProfileRepository;

    public GetAllUserProfilesQueryHandler(IUserProfileRepository<UserProfile> userProfileRepository)
    {
        _userProfileRepository = userProfileRepository;
    }

    /// <summary>
    /// Handle
    /// </summary>
    /// <param name="request">GetAllUserProfilesQuery</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>List of user profile</returns>
    /// CreatedBy: ThiepTT(06/11/2023)
    public async Task<OperationResult<IEnumerable<UserProfile>>> Handle(GetAllUserProfilesQuery request, CancellationToken cancellationToken)
    {
        OperationResult<IEnumerable<UserProfile>> result = new OperationResult<IEnumerable<UserProfile>>();

        try
        {
            IEnumerable<UserProfile> userProfiles = await _userProfileRepository.GetAllAsync();

            result.Data = userProfiles;
        }
        catch (Exception ex)
        {
            result.AddError(ErrorCode.InternalServerError, ex.Message);
        }

        return result;
    }
}