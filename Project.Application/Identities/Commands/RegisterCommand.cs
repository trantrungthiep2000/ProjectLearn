using MediatR;
using Project.Application.Models;

namespace Project.Application.Identities.Commands;

/// <summary>
/// Information of register command
/// CreatedBy: ThiepTT(31/10/2023)
/// </summary>
public class RegisterCommand : IRequest<OperationResult<string>>
{
    /// <summary>
    /// Full name
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Phone number
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Date of birth
    /// </summary>
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// Password
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Role name
    /// </summary>
    public string RoleName { get; set; } = "User";
}