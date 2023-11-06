using MediatR;
using Project.Application.Models;
using Project.Domain.Aggregates;

namespace Project.Application.Users.Queries;

/// <summary>
/// Information of get all user profile entity framework query
/// CreatedBy: ThiepTT(06/11/2023)
/// </summary>
public class GetAllUserProfilesEFQuery : IRequest<OperationResult<IEnumerable<UserProfile>>> { }