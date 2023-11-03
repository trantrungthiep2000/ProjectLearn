﻿using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using Project.Application.Models;
using Project.Application.Users.Commands;
using Project.Domain.Aggregates;
using Project.Infrastructure.Interfaces;

namespace Project.Application.Users.CommandHandlers;

/// <summary>
/// Information of remove account command handler
/// CreatedBy: ThiepTT(02/11/2023)
/// </summary>
public class RemoveAccountCommandHandler : IRequestHandler<RemoveAccountCommand, OperationResult<string>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserProfileRepository<UserProfile> _userProfileRepository;
    private readonly IIdentityUserRepository<IdentityUser> _identityUserRepository;

    public RemoveAccountCommandHandler(IUnitOfWork unitOfWork, IUserProfileRepository<UserProfile> userProfileRepository,
        IIdentityUserRepository<IdentityUser> identityUserRepository)
    {
        _unitOfWork = unitOfWork;
        _userProfileRepository = userProfileRepository;
        _identityUserRepository = identityUserRepository;
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

        await using IDbContextTransaction transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            UserProfile userProfileById = await _userProfileRepository.GetByIdAsync(request.UserProfileId);

            if (userProfileById is null)
            {
                result.AddError(ErrorCode.NotFound, string.Format(UserProfileErrorMessage.UserProfileNotFound, request.UserProfileId));
                return result;
            }

            IdentityUser identityUser = await _identityUserRepository.GetUserByEmailAsync(userProfileById.Email, cancellationToken);

            if (identityUser is null)
            {
                result.AddError(ErrorCode.NotFound, string.Format(UserProfileErrorMessage.UserProfileByEmailNotFound, userProfileById.Email));
                return result;
            }

            _identityUserRepository.Delete(identityUser);
            _userProfileRepository.Delete(userProfileById);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
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