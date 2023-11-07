using MediatR;
using Project.Application.Messages;
using Project.Application.Models;
using Project.Application.Users.Queries;
using Project.Domain.Aggregates;
using Project.Infrastructure.Interfaces;

namespace Project.Application.Users.QueryHandlers;

/// <summary>
/// Information of get user profile by id query handler
/// CreatedBy: ThiepTT(31/10/2023)
/// </summary>
public class GetUserProfileByIdQueryHandler : IRequestHandler<GetUserProfileByIdQuery, OperationResult<UserProfile>>
{
    private readonly IUserProfileRepository<UserProfile> _userProfileRepository;

    public GetUserProfileByIdQueryHandler(IUserProfileRepository<UserProfile> userProfileRepository)
    {
        _userProfileRepository = userProfileRepository;
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
            UserProfile userProfileById = await _userProfileRepository.GetByIdAsync(request.UserProfileId);

            if (userProfileById is null)
            {
                result.AddError(ErrorCode.NotFound, string.Format(ErrorMessage.UserProfile.UserProfileNotFound, request.UserProfileId));
                return result;
            }

            result.Data = userProfileById;
        }
        catch (Exception ex)
        {
            result.AddError(ErrorCode.InternalServerError, ex.Message);
        }

        return result;
    }
}