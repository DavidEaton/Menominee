using AutoMapper;
using CustomerVehicleManagement.Api.Data.Models;
using CustomerVehicleManagement.Domain.Entities;

namespace CustomerVehicleManagement.Api.Profiles
{
    public class PersonProfile : Profile
    {
        public PersonProfile()
        {
            // Map from source type to destination type
            CreateMap<Person, PersonReadDto>()
                .ForMember(
                    destination => destination.Name,
                    configuration => configuration.MapFrom(source => source.Name.LastFirstMiddle));

            CreateMap<Person, PersonListDto>()
                .ForMember(
                    destination => destination.Name,
                    configuration => configuration.MapFrom(source => source.Name.LastFirstMiddle))
                .ForMember(
                    destination => destination.Address,
                    configuration => configuration.MapFrom(source => source.Address.AddressFull));

            CreateMap<Person, PersonCreateDto>().ReverseMap();

        }
    }
}
