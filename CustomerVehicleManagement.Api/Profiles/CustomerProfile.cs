using AutoMapper;
using CustomerVehicleManagement.Api.Data.Models;
using CustomerVehicleManagement.Domain.Entities;

namespace CustomerVehicleManagement.Api.Profiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            // Map from source type to destination type, and back.
            CreateMap<Customer, CustomerReadDto>()
                    .ForMember(
                        destination => destination.Contact,
                        configuration =>
                        {
                            configuration.PreCondition(customer => customer.Entity as Organization != null);
                            configuration.MapFrom(source => (source.Entity as Organization).Contact);
                        });
                    //.ReverseMap();
        }
    }
}
