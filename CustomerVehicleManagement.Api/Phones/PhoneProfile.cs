using AutoMapper;
using CustomerVehicleManagement.Domain.Entities;

namespace CustomerVehicleManagement.Api.Phones
{
    public class PhoneProfile : Profile
    {
        public PhoneProfile()
        {
            CreateMap<Phone, PhoneCreateDto>()
                .ReverseMap();
            CreateMap<Phone, PhoneReadDto>()
                .ReverseMap();
            CreateMap<Phone, PhoneUpdateDto>()
                .ReverseMap();
            CreateMap<PhoneReadDto, Phone>()
                .ForMember(destination => destination.TrackingState, configuration => configuration.Ignore())
                .ReverseMap();
            CreateMap<PhoneCreateDto, Phone>()
                .ForMember(destination => destination.TrackingState, configuration => configuration.Ignore())
                .ReverseMap();
            CreateMap<PhoneUpdateDto, Phone>()
                .ForMember(destination => destination.TrackingState, configuration => configuration.Ignore())
                .ReverseMap();
            CreateMap<PhoneUpdateDto, PhoneReadDto>()
                .ReverseMap();
        }
    }
}
