using MediatR;
using Project.Application.Models;
using Project.Domain.Aggregates;

namespace Project.Application.Users.Queries;

/// <summary>
/// Information of get all user profiles entity framework as no tracking query
/// CreatedBy: ThiepTT(06/11/2023)
/// </summary>
public class GetAllUserProfilesEFAsNoTrackingQuery : IRequest<OperationResult<IEnumerable<UserProfile>>> { }