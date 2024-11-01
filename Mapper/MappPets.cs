using AutoMapper;
using EcommercePetsFoodBackend.Data.Dto;
using EcommercePetsFoodBackend.Data.Models;

namespace EcommercePetsFoodBackend.Mapper
{
    public class MappPets : Profile
    {
        public MappPets()
        {
            CreateMap<Customers,CustomerRegisterDto>().ReverseMap();
            CreateMap<Customers,LoginDto>().ReverseMap();   
            CreateMap<Customers,AdminRegDto>().ReverseMap();
        }
    }
}
