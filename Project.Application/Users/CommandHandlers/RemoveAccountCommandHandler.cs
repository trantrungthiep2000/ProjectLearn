using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Project.Application.Models;
using Project.Application.Users.Commands;
using Project.DAL.Data;
using Project.Domain.Aggregates;

namespace Project.Application.Users.CommandHandlers;

/// <summary>
/// Information of remove account command handler
/// CreatedBy: ThiepTT(02/11/2023)
/// </summary>
public class RemoveAccountCommandHandler : IRequestHandler<RemoveAccountCommand, OperationResult<string>>
{
    private readonly DataContext _dataContext;

    public RemoveAccountCommandHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    /// <summary>
    /// Handle
    /// </summary>
    /// <param name="request">RemoveAccountCommand</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>Message remove account</returns>
    /// CreatedBy: ThiepTT(02/11/2023)
    public async Task<OperationResult<string>> Handle(RemoveAccountCommand request, CancellationToken cancellationToken)
    {
        OperationResult<string> result = new OperationResult<string>();

        await using IDbContextTransaction transaction = await _dataContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            UserProfile? userProfileById = await _dataContext.UserProfiles
                .FirstOrDefaultAsync(userProfile => userProfile.UserProfileId == request.UserProfileId, cancellationToken);

            if (userProfileById is null)
            {
                result.AddError(ErrorCode.NotFound, string.Format(UserProfileErrorMessage.UserProfileNotFound, request.UserProfileId));
                return result;
            }

            IdentityUser? identityUser = await _dataContext.Users
                .FirstOrDefaultAsync(user => user.Email!.Trim().ToLower().Equals(userProfileById.Email.Trim().ToLower()), cancellationToken);

            if (identityUser is null)
            {
                result.AddError(ErrorCode.NotFound, string.Format(UserProfileErrorMessage.UserProfileByEmailNotFound, userProfileById.Email));
                return result;
            }

            _dataContext.Users.Remove(identityUser);
            _dataContext.UserProfiles.Remove(userProfileById);
            await _dataContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            result.Data = UserProfileResponseMessage.RemoveAccountSuccess;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            result.AddError(ErrorCode.InternalServerError, ex.Message);
        }

        return result;
    }
}