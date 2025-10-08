using AutoMapper;
using Recruiting.Application.DTOs;
using Recruiting.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Recruiting.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<CreateUserDto, User>();
        }
    }
}

