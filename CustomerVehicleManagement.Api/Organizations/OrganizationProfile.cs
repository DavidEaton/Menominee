using AutoMapper;
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
