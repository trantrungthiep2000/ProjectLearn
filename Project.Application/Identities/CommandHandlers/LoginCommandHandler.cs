using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Project.Application.Identities.Commands;
using Project.Application.Models;
using Project.Application.Services;
using Project.Application.Validators;
using Project.DAL.Data;
using Project.Domain.Aggregates;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Project.Application.Identities.CommandHandlers;

/// <summary>
/// Information of login command handler
/// CreatedBy: ThiepTT(31/10/2023)
/// </summary>
public class LoginCommandHandler : IRequestHandler<LoginCommand, OperationResult<string>>
{
    private readonly DataContext _dataContext;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IdentityService _identityService;

    public LoginCommandHandler(DataContext dataContext,
        UserManager<IdentityUser> userManager,
        IdentityService identityService)
    {
        _dataContext = dataContext;
        _userManager = userManager;
        _identityService = identityService;
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
            UserProfile userProfile = await GetUserProfileByEmailAsync(request.Email, cancellationToken);

            if (userProfile == null)
            {
                result.AddError(ErrorCode.BadRequest, IdentityErrorMessage.IdentityUserNotExists);
                return result;
            }

            result.Data = GetJwtToken(userProfile);
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
                result.AddError(ErrorCode.BadRequest, IdentityErrorMessage.IdentityUserIncorrectPassword);
                return true;
            }
        }
        else
        {
            result.AddError(ErrorCode.BadRequest, IdentityErrorMessage.IdentityUserIncorrectPassword);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Get user profile by email async
    /// </summary>
    /// <param name="email">Email</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>UserProfile</returns>
    /// CreatedBy: ThiepTT(01/11/2023)
    private async Task<UserProfile> GetUserProfileByEmailAsync(string email, CancellationToken cancellationToken)
    {
        UserProfile? userProfileById = await _dataContext.UserProfiles
            .FirstOrDefaultAsync(userProfile => userProfile.Email.Trim().ToLower().Equals(email.Trim().ToLower()), cancellationToken);

        return userProfileById!;
    }

    /// <summary>
    /// Get jwt token
    /// </summary>
    /// <param name="userProfile">UserProfile</param>
    /// <returns>Token</returns>
    /// CreatedBy: ThiepTT(01/11/2023)
    private string GetJwtToken(UserProfile userProfile)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userProfile.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, userProfile.Email),
            new Claim(ClaimTypes.Name, userProfile.FullName),
            new Claim("UserProfileId", userProfile.UserProfileId.ToString())
        };

        var identity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);

        var token = _identityService.CreateSecurityToken(identity);
        return _identityService.WriteToken(token);
    }
}