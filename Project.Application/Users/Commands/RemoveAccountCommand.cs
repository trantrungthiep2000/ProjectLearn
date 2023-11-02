using MediatR;
using Project.Application.Models;

namespace Project.Application.Users.Commands;

/// <summary>
/// Information of remove account command
/// </summary>
public class RemoveAccountCommand : IRequest<OperationResult<string>>
{
    /// <summary>
    /// Id of user profile
    /// </summary>
    public Guid UserProfileId { get; set; }
}