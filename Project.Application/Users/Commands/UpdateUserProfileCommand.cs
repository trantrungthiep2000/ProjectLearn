using MediatR;
using Project.Application.Models;

namespace Project.Application.Users.Commands;

/// <summary>
/// Information of update user profile command
/// CreatedBy: ThiepTT(02/11/2023)
/// </summary>
public class UpdateUserProfileCommand : IRequest<OperationResult<string>>
{
    /// <summary>
    /// Id of user profile
    /// </summary>
    public Guid UserProfileId { get; set; }

    /// <summary>
    /// Full name
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Phone number
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Date of birth
    /// </summary>
    public DateTime DateOfBirth { get; set; }
}