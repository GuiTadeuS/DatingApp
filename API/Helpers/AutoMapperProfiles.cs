using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>()
                .ForMember(member => member.PhotoUrl,
                receives => receives.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(member => member.Age, receives => receives.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoDto>();
        }
    }
}
