using System;
using System.Collections.Generic;
using System.Text;
using WPO.Application.DTO;
using WPO.Domain;
using AutoMapper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WPO.Application.Profiles
{
    public class PolyOmniaProfile : Profile
    {
        public PolyOmniaProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<UserDTO, User>()
                .ForMember(dest => dest.Videos, opt => opt.Ignore());

            CreateMap<Video, VideoDTO>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User != null ? src.User.Name : null));

            CreateMap<VideoDTO, Video>()
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<Category, CategoryDTO>().ReverseMap();
        }
    }
}
