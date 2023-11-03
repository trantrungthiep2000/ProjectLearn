using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using Project.Application.Identities.Commands;
using Project.Application.Models;
using Project.Application.Validators;
using Project.Domain.Aggregates;
using Project.Infrastructure.Interfaces;

namespace Project.Application.Identities.CommandHandlers;

/// <summary>
/// Information of register command handler
/// CreatedBy: ThiepTT(31/10/2023)
/// </summary>
public class RegisterCommandHandler : IRequestHandler<RegisterCommand, OperationResult<string>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IMapper _mapper;
    private readonly IUserProfileRepository<UserProfile> _userProfileRepository;

    public RegisterCommandHandler(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, IMapper mapper,
        IUserProfileRepository<UserProfile> userProfileRepository)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _mapper = mapper;
        _userProfileRepository = userProfileRepository;

    }

    /// <summary>
    /// Handle
    /// </summary>
    /// <param name="request">RegisterCommand</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>Message created user profile</returns>
    /// CreatedBy: ThiepTT(31/10/2023)
    public async Task<OperationResult<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        OperationResult<string> result = new OperationResult<string>();

        await using IDbContextTransaction transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            UserProfile userProfile = _mapper.Map<UserProfile>(request);

            // Validate request of user profile
            if (ValidateUserProfile(userProfile, result)) { return result; }

            // Check email has been registered
            if (await IsEmailRegisteredAsync(request.Email, result)) { return result; }

            // Create identity user
            await CreateIdentityUserAsync(request, result, transaction, cancellationToken);

            if (result.IsError) { return result; }

            // Create user profile
            CreateUserProfile(userProfile);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            result.Data = IdentityResponseMessage.RegisterSuccess;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            result.AddError(ErrorCode.InternalServerError, ex.Message);
        }

        return result;
    }

    /// <summary>
    /// Validate user profile
    /// </summary>
    /// <param name="userProfile">UserProfile</param>
    /// <param name="result">OperationResult<string></param>
    /// <returns>True | False</returns>
    /// CreatedBy: ThiepTT(31/10/2023)
    private bool ValidateUserProfile(UserProfile userProfile, OperationResult<string> result)
    {
        UserProfileValidator validator = new UserProfileValidator();

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

    /// <summary>
    /// Is email registered
    /// </summary>
    /// <param name="email">Email</param>
    /// <param name="result">OperationResult<string></param>
    /// <returns>True | False</returns>
    /// CreatedBy: ThiepTT(01/11/2023)
    private async Task<bool> IsEmailRegisteredAsync(string email, OperationResult<string> result)
    {
        IdentityUser? identityUser = await _userManager.FindByEmailAsync(email);

        if (identityUser is not null)
        {
            result.AddError(ErrorCode.NotFound, IdentityErrorMessage.IdentityUserAlreadyExists);

            return true;
        }

        return false;
    }

    /// <summary>
    /// Create identity user async
    /// </summary>
    /// <param name="request">RegisterCommand</param>
    /// <param name="result">OperationResult<string></param>
    /// <param name="transaction">IDbContextTransaction</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>Task</returns>
    /// CreatedBy: ThiepTT(31/10/2023)
    private async Task CreateIdentityUserAsync(RegisterCommand request, OperationResult<string> result,
        IDbContextTransaction transaction, CancellationToken cancellationToken)
    {
        IdentityUser identityUser = new IdentityUser() { Email = request.Email, UserName = request.Email };

        IdentityResult identityUserCreate = await _userManager.CreateAsync(identityUser, request.Password);

        if (!identityUserCreate.Succeeded)
        {
            await transaction.RollbackAsync(cancellationToken);

            foreach (IdentityError identityError in identityUserCreate.Errors)
            {
                result.AddError(ErrorCode.BadRequest, identityError.Description);
            }
        }
    }

    /// <summary>
    /// Create user profile
    /// </summary>
    /// <param name="userProfile">UserProfile</param>
    /// <returns>void</returns>
    /// CreatedBy: ThiepTT(31/10/2023)
    private void CreateUserProfile(UserProfile userProfile)
    {
        UserProfile userProfileCreate = UserProfile.CreateUserProfile(userProfile.FullName,
            userProfile.Email, userProfile.PhoneNumber, userProfile.DateOfBirth);

        _userProfileRepository.Create(userProfileCreate);
    }
}