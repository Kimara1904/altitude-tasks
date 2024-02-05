using AutoMapper;
using Task4.DTOs;
using Task4.Models;

namespace Task4.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<NewUserDTO, User>();
        }
    }
}
