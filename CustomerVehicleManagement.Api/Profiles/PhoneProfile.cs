using AutoMapper;
using CustomerVehicleManagement.Api.Data.Models;
using CustomerVehicleManagement.Domain.Entities;

namespace CustomerVehicleManagement.Api.Profiles
{
    public class PhoneProfile : Profile
    {
        public PhoneProfile()
        {
            CreateMap<Phone, PhoneReadDto>()
                .ReverseMap();
            CreateMap<Phone, PhoneUpdateDto>()
                .ReverseMap();

        }
    }
}
