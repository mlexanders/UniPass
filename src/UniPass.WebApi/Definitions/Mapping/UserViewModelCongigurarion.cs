using AutoMapper;
using UniPass.Infrastructure.Models;
using UniPass.Infrastructure.ViewModels;

namespace UniPass.WebApi.Definitions.Mapping;

public class UserViewModelConfiguration : Profile
{
    public UserViewModelConfiguration()
    {
        CreateMap<ApplicationUserModel, UserModelViewModel>()
            .ForMember(x => x.Id,
                o => o.MapFrom(a => a.Id))
            .ForMember(x => x.FirstName,
                o => o.MapFrom(a => a.FirstName))
            .ForMember(x => x.LastName,
                o => o.MapFrom(a => a.LastName))
            .ForMember(x => x.Email,
                o => o.MapFrom(a => a.Email));
    }
}