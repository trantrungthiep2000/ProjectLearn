using AutoMapper;
using Project.Application.Dtos.Requests;
using Project.Application.Identities.Commands;
using Project.Application.Users.Commands;
using Project.Domain.Aggregates;

namespace Project.Application.MappingProfiles;

/// <summary>
/// Information of identity mapping
/// CreatedBy: ThiepTT(31/10/2023)
/// </summary>
public class IdentityMapping : Profile
{
    public IdentityMapping()
    {
        CreateMap<RegisterCommand, UserProfile>().ReverseMap();
        CreateMap<RegisterCommand, RegisterRequest>().ReverseMap();
        CreateMap<LoginCommand, LoginRequest>().ReverseMap();
    }
}