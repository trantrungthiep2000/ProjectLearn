using MediatR;
using Project.Application.Models;
using Project.Domain.Aggregates;

namespace Project.Application.Users.Queries;

/// <summary>
/// Information of get user profile by id query
/// CreatedBy: ThiepTT(31/10/2023)
/// </summary>
public class GetUserProfileByIdQuery : IRequest<OperationResult<UserProfile>>
{
    /// <summary>
    /// Id of user profile
    /// </summary>
    public Guid UserProfileId { get; set; }
}