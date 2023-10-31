using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using Project.Application.Identities.Commands;
using Project.Application.Models;
using Project.Application.Services;
using Project.DAL.Data;

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

    public async Task<OperationResult<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        OperationResult<string> result = new OperationResult<string>();

        await using IDbContextTransaction transaction = await _dataContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var identityUser = await _userManager.FindByEmailAsync(request.Email);

            if (identityUser is not null)
            {
                var validPassword = await _userManager.CheckPasswordAsync(identityUser, request.Password);

                if (!validPassword)
                {
                    result.AddError(ErrorCode.BadRequest, IdentityErrorMessage.IdentityUserIncorrectPassword);

                    return result;
                }
            }
            else
            {
                result.AddError(ErrorCode.BadRequest, IdentityErrorMessage.IdentityUserIncorrectPassword);

                return result;
            }

            
        }
        catch (Exception ex)
        {
            result.AddError(ErrorCode.InternalServerError, ex.Message);
        }

        return result;
    }
}