using MediatR;
using Microsoft.EntityFrameworkCore;
using Project.Application.Identities;
using Project.Application.Models;
using Project.Application.Users.Commands;
using Project.DAL.Data;
using Project.Domain.Aggregates;

namespace Project.Application.Users.CommandHandlers;

/// <summary>
/// Information of update user profile command handler
/// CreatedBy: ThiepTT(02/11/2023)
/// </summary>
public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, OperationResult<string>>
{
    private readonly DataContext _dataContext;

    public UpdateUserProfileCommandHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<OperationResult<string>> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        OperationResult<string> result = new OperationResult<string>();

		try
		{
            var userProfile = await GetUserProfileByIdAsync(request.UserProfileId, cancellationToken, result);

            if (result.IsError) { return result; }

        }
		catch (Exception ex)
		{
			result.AddError(ErrorCode.InternalServerError, ex.Message);
		}

		return result;
    }

    /// <summary>
    /// Get user profile by id async
    /// </summary>
    /// <param name="userProfileId">Id of user profile</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <param name="result">OperationResult<string> </param>
    /// <returns>UserProfile</returns>
    /// CreatedBy: ThiepTT(02/11/2023)
    private async Task<UserProfile> GetUserProfileByIdAsync(Guid userProfileId, CancellationToken cancellationToken,
        OperationResult<string> result)
    {
        UserProfile? userProfileById = await _dataContext.UserProfiles
            .FirstOrDefaultAsync(userProfile => userProfile.UserProfileId == userProfileId, cancellationToken);
        
        if (userProfileById is null)
        {
            result.AddError(ErrorCode.NotFound, string.Format(UserProfileErrorMessage.UserProfileNotFound, userProfileId));
        }

        return userProfileById!;
    }
}