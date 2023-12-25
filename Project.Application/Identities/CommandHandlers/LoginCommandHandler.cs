using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Project.Application.Identities.Commands;
using Project.Application.Messages;
using Project.Application.Models;
using Project.Application.Services;
using Project.Application.Validators;
using Project.Domain.Aggregates;
using Project.Domain.Interfaces.IRepositories;
using Project.Domain.Interfaces.IServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Project.Application.Identities.CommandHandlers;

/// <summary>
/// Information of login command handler
/// CreatedBy: ThiepTT(31/10/2023)
/// </summary>
public class LoginCommandHandler : IRequestHandler<LoginCommand, OperationResult<string>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IdentityService _identityService;
    private readonly IUserProfileRepository<UserProfile> _userProfileRepository;
    private readonly IIdentityUserRepository<IdentityUser> _identityUserRepository;

    public LoginCommandHandler(
        IUnitOfWork unitOfWork,
        UserManager<IdentityUser> userManager,
        IdentityService identityService,
        IUserProfileRepository<UserProfile> userProfileRepository,
        IIdentityUserRepository<IdentityUser> identityUserRepository)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _identityService = identityService;
        _userProfileRepository = userProfileRepository;
        _identityUserRepository = identityUserRepository;
    }

    /// <summary>
    /// Handle
    /// </summary>
    /// <param name="request">LoginCommand</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>Token</returns>
    /// CreatedBy: ThiepTT(01/11/2023)
    public async Task<OperationResult<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        OperationResult<string> result = new OperationResult<string>();

        try
        {
            // Validate request of login
            if (ValidateLogin(request, result)) { return result; }

            // Check login account
            if (await CheckLoginAccountAsync(request, result)) { return result; }

            // Get information of user profile
            UserProfile userProfile = await _userProfileRepository.GetUserProfileByEmail(request.Email);

            IdentityUser identityUser = await _identityUserRepository.GetUserByEmailAsync(userProfile.Email);

            if (userProfile is null || identityUser is null)
            {
                result.AddError(ErrorCode.BadRequest, ErrorMessage.Identity.IdentityUserNotExists);
                return result;
            }

            IEnumerable<string> roles = await _userManager.GetRolesAsync(identityUser);

            result.Data = GetJwtToken(userProfile, roles);
        }
        catch (Exception ex)
        {
            result.AddError(ErrorCode.InternalServerError, ex.Message);
        }

        return result;
    }

    /// <summary>
    /// Validate login
    /// </summary>
    /// <param name="request">LoginCommand</param>
    /// <param name="result">OperationResult<string></param>
    /// <returns>True | False</returns>
    /// CreatedBy: ThiepTT(01/11/2023)
    private bool ValidateLogin(LoginCommand request, OperationResult<string> result)
    {
        LoginValidator validator = new LoginValidator();

        ValidationResult validationLogin = validator.Validate(request);

        if (!validationLogin.IsValid)
        {
            foreach (ValidationFailure error in validationLogin.Errors)
            {
                result.AddError(ErrorCode.BadRequest, error.ErrorMessage);
            }

            return true;
        }

        return false;
    }

    /// <summary>
    /// Check login account async
    /// </summary>
    /// <param name="request">LoginCommand</param>
    /// <param name="result">OperationResult<string></param>
    /// <returns>True | False</returns>
    /// CreatedBy: ThiepTT(01/11/2023)
    private async Task<bool> CheckLoginAccountAsync(LoginCommand request, OperationResult<string> result)
    {
        var identityUser = await _userManager.FindByEmailAsync(request.Email);

        if (identityUser is not null)
        {
            var validPassword = await _userManager.CheckPasswordAsync(identityUser, request.Password);

            if (!validPassword)
            {
                result.AddError(ErrorCode.BadRequest, ErrorMessage.Identity.IdentityUserIncorrectPassword);
                return true;
            }
        }
        else
        {
            result.AddError(ErrorCode.BadRequest, ErrorMessage.Identity.IdentityUserIncorrectPassword);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Get jwt token
    /// </summary>
    /// <param name="userProfile">UserProfile</param>
    /// <param name="roles">IEnumerable<string></param>
    /// <returns>Token</returns>
    /// CreatedBy: ThiepTT(01/11/2023)
    private string GetJwtToken(UserProfile userProfile, IEnumerable<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userProfile.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, userProfile.Email),
            new Claim($"FullName", userProfile.FullName),
            new Claim($"UserProfileId", userProfile.UserProfileId.ToString())
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var identity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);

        var token = _identityService.CreateSecurityToken(identity);
        return _identityService.WriteToken(token);
    }
}