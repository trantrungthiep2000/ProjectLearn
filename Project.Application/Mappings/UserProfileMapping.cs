using AutoMapper;
using Project.Application.Dtos.Requests;
using Project.Application.Users.Commands;

namespace Project.Application.Mappings;

/// <summary>
/// Information of user profile mapping
/// </summary>
public class UserProfileMapping : Profile
{
    public UserProfileMapping()
    {
        CreateMap<UpdateUserProfileCommand, UserProfileRequest>().ReverseMap();
    }
}