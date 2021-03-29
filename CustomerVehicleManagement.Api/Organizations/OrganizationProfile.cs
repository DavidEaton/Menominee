using AutoMapper;
using CustomerVehicleManagement.Api.Data.Dtos;
using CustomerVehicleManagement.Domain.Entities;

namespace CustomerVehicleManagement.Api.Organizations
{
    public class OrganizationProfile : Profile
    {
        public OrganizationProfile()
        {
            // Map from source type to destination type, and back.
            CreateMap<Organization, OrganizationReadDto>()
                .ForMember(
                    destination => destination.AddressLine,
                    configuration => configuration
                        .MapFrom(
                            source => source.Address.AddressLine))
                .ForMember(
                    destination => destination.City,
                    configuration => configuration
                        .MapFrom(
                            source => source.Address.City))
                .ForMember(
                    destination => destination.State,
                    configuration => configuration
                        .MapFrom(
                            source => source.Address.State))
                .ForMember(
                    destination => destination.PostalCode,
                    configuration => configuration
                        .MapFrom(
                            source => source.Address.PostalCode))
                .ForMember(
                    destination => destination.Name,
                    configuration => configuration
                        .MapFrom(
                            source => source.Name.Value))
                .ReverseMap();

            CreateMap<Organization, OrganizationUpdateDto>()
                .ForMember(
                    destination => destination.Name,
                    configuration => configuration
                        .MapFrom(
                            source => source.Name.Value));

            CreateMap<OrganizationUpdateDto, Organization>()
                .ForMember(
                    destination => destination.Name,
                    configuration => configuration
                        .MapFrom(
                            source => source.Name));

            CreateMap<Organization, OrganizationCreateDto>()
                .ForMember(
                    destination => destination.Name,
                    configuration => configuration
                        .MapFrom(
                            source => source.Name.Value))
                .ReverseMap();
        }
    }
}
