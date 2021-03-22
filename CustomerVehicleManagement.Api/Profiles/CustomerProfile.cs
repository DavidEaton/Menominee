using AutoMapper;
using CustomerVehicleManagement.Api.Data.Dtos;
using CustomerVehicleManagement.Domain.Entities;

namespace CustomerVehicleManagement.Api.Profiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            // TODO: Try replacing AutoMapper PreCondition with Condition:

            //CreateMap<Customer, CustomerReadDto>()
            //        .ForMember(
            //            destination => destination.Contact,
            //            configuration =>
            //            {
            //                configuration.PreCondition(customer => customer.Entity as Organization != null);
            //                configuration.MapFrom(source => (source.Entity as Organization).Contact);
            //            });

            CreateMap<CustomerUpdateDto, Customer>();
        }
    }
}
