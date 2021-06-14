using AutoMapper;
using CustomerVehicleManagement.Domain.Entities;

namespace CustomerVehicleManagement.Api.Customers
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<CustomerUpdateDto, Customer>().ReverseMap();

            CreateMap<Customer, CustomerCreateDto>()
                .ReverseMap();
        }
    }
}
