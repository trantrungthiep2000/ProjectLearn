using MediatR;
using Project.Application.Models;

namespace Project.Application.Identities.Commands;

/// <summary>
/// Information of login command
/// CreatedBy: ThiepTT(31/10/2023)
/// </summary>
public class LoginCommand : IRequest<OperationResult<string>>
{
    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Password
    /// </summary>
    public string Password { get; set; } = string.Empty;
}