using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.ViewModels;

namespace Demo.PL.Mapping_Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserViewModel>()
                .ForMember(dest => dest.FName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LName, opt => opt.MapFrom(src => src.LastName))
                .ReverseMap()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LName));
        }
    }
}
