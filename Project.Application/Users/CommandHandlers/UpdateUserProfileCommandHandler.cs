using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using Project.Application.Messages;
using Project.Application.Models;
using Project.Application.Users.Commands;
using Project.Application.Validators;
using Project.Domain.Aggregates;
using Project.Domain.Interfaces.IRepositories;
using Project.Domain.Interfaces.IServices;

namespace Project.Application.Users.CommandHandlers;

/// <summary>
/// Information of update user profile command handler
/// CreatedBy: ThiepTT(02/11/2023)
/// </summary>
public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, OperationResult<string>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserProfileRepository<UserProfile> _userProfileRepository;

    public UpdateUserProfileCommandHandler(IUnitOfWork unitOfWork, IUserProfileRepository<UserProfile> userProfileRepository)
    {
        _unitOfWork = unitOfWork;
        _userProfileRepository = userProfileRepository;
    }

    /// <summary>
    /// Handle
    /// </summary>
    /// <param name="request">UpdateUserProfileCommand</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>Message updated user profile</returns>
    /// CreatedBy: ThiepTT(02/11/2023)
    public async Task<OperationResult<string>> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        OperationResult<string> result = new OperationResult<string>();

        await using IDbContextTransaction transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            // Validate request of user profile
            if (ValidateUserProfile(request, result)) { return result; }

            UserProfile userProfile = await GetUserProfileByIdAsync(request.UserProfileId, result);

            if (result.IsError) { return result; }

            userProfile.UpdateUserProfile(request.FullName, request.PhoneNumber, request.DateOfBirth);
            _userProfileRepository.Update(userProfile);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            result.Data = ResponseMessage.UserProfile.UpdateAccountSuccess;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            result.AddError(ErrorCode.InternalServerError, ex.Message);
        }

        return result;
    }

    /// <summary>
    /// Get user profile by id async
    /// </summary>
    /// <param name="userProfileId">Id of user profile</param>
    /// <param name="result">OperationResult<string> </param>
    /// <returns>UserProfile</returns>
    /// CreatedBy: ThiepTT(02/11/2023)
    private async Task<UserProfile> GetUserProfileByIdAsync(Guid userProfileId, OperationResult<string> result)
    {
        UserProfile userProfileById = await _userProfileRepository.GetByIdAsync(userProfileId);

        if (userProfileById is null)
        {
            result.AddError(ErrorCode.NotFound, string.Format(ErrorMessage.UserProfile.UserProfileNotFound, userProfileId));
        }

        return userProfileById!;
    }

    /// <summary>
    /// Validate user profile
    /// </summary>
    /// <param name="userProfile">UpdateUserProfileCommand</param>
    /// <param name="result">OperationResult<string></param>
    /// <returns>True | False</returns>
    /// CreatedBy: ThiepTT(03/11/2023)
    private bool ValidateUserProfile(UpdateUserProfileCommand userProfile, OperationResult<string> result)
    {
        UpdateUserProfileValidator validator = new UpdateUserProfileValidator();

        ValidationResult validationRegister = validator.Validate(userProfile);

        if (!validationRegister.IsValid)
        {
            foreach (ValidationFailure error in validationRegister.Errors)
            {
                result.AddError(ErrorCode.BadRequest, error.ErrorMessage);
            }

            return true;
        }

        return false;
    }
}