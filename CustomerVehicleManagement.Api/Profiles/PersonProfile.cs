using AutoMapper;
using CustomerVehicleManagement.Api.Data.Models;
using CustomerVehicleManagement.Domain.Entities;

namespace CustomerVehicleManagement.Api.Profiles
{
    public class PersonProfile : Profile
    {
        public PersonProfile()
        {
            // Map from source type to destination type, and back.
            CreateMap<Person, PersonReadDto>()
                .ForMember(
                    destination => destination.Name,
                    configuration => configuration.MapFrom(source => source.Name.LastFirstMiddle))
                .ReverseMap();

            CreateMap<Person, PersonInListDto>()
                .ForMember(
                    destination => destination.Name,
                    configuration => configuration.MapFrom(
                        source => source.Name.LastFirstMiddle))
                .ForMember(
                    destination => destination.AddressLine,
                    configuration => configuration.MapFrom(
                        source => source.Address.AddressLine))
                .ForMember(
                    destination => destination.City,
                    configuration => configuration.MapFrom(
                        source => source.Address.City))
                .ForMember(
                    destination => destination.State,
                    configuration => configuration.MapFrom(
                        source => source.Address.State))
                .ForMember(
                    destination => destination.PostalCode,
                    configuration => configuration.MapFrom(
                        source => source.Address.PostalCode))
                .ReverseMap();

            CreateMap<Person, PersonCreateDto>()
                .ReverseMap();

            CreateMap<Person, PersonUpdateDto>()
                .ReverseMap();

            CreateMap<PersonUpdateDto, PersonReadDto>()
                .ForMember(
                    destination => destination.Name,
                    configuration => configuration.MapFrom(source => source.Name.LastFirstMiddle))
                .ReverseMap();

        }
    }
}
