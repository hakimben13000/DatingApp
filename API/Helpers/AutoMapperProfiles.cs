using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            // source --> target
            CreateMap<AppUser, MemberDto>() // Map from AppUser to MemberDto
                .ForMember(dest => dest.PhotoUrl,
                        opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url)) // Map photoUrl with Main Photos
                .ForMember(dest => dest.Age, 
                    opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge())); 
            CreateMap<Photo, PhotoDto>(); // Map from Photo to PhotoDto

            CreateMap<MemberUpdateDto, AppUser>(); // Map from MemberUpdateDto to AppUser

            CreateMap<RegisterDto, AppUser>(); // Map from RegisterDto to AppUser
        }
    }
}
