using AutoMapper;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;

namespace CustomerVehicleManagement.Api.Organizations
{
    public class OrganizationProfile : Profile
    {
        public OrganizationProfile()
        {
            // Map from source type to destination type, and back.
            CreateMap<Organization, OrganizationReadDto>()
                .ForMember(
                    destination => destination.Name,
                    configuration => configuration
                        .MapFrom(
                            source => source.Name.Name))
                .ReverseMap();

            CreateMap<Organization, OrganizationUpdateDto>()
                .ForMember(
                    destination => destination.Name,
                    configuration => configuration
                        .MapFrom(
                            source => source.Name.Name));

            CreateMap<Organization, OrganizationCreateDto>()
                .ForMember(
                    destination => destination.Name,
                    configuration => configuration
                        .MapFrom(
                            source => source.Name))
                .ReverseMap();
        }
    }
}
