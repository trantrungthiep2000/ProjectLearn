using MediatR;
using Microsoft.EntityFrameworkCore;
using Project.Application.Models;
using Project.Application.Users.Queries;
using Project.DAL.Data;
using Project.Domain.Aggregates;

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
            UserProfile? userProfileById = await _dataContext.UserProfiles
                .FirstOrDefaultAsync(userProfile => userProfile.UserProfileId == request.UserProfileId, cancellationToken);

            if (userProfileById is null)
            {
                result.AddError(ErrorCode.NotFound, 
                    string.Format(UserProfileErrorMessage.UserProfileNotFound, request.UserProfileId));

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