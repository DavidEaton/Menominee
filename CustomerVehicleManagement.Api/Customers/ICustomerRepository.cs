using CustomerVehicleManagement.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Customers
{
    public interface ICustomerRepository
    {
        Task<Customer> AddAndSaveCustomerAsync(CustomerCreateDto entity);
        void DeleteCustomer(Customer entity);
        void FixTrackingState();
        Task<bool> CustomerExistsAsync(int id);
        Task<Customer> UpdateCustomerAsync(Customer entity);
        Task<IEnumerable<CustomerReadDto>> GetCustomersAsync();
        Task<Customer> GetCustomerAsync(int id);
        Task<bool> SaveChangesAsync(Customer customer);
        Task<bool> SaveChangesAsync();
        Task GetCustomerOrganizationEntity(Customer customer);
        Task GetCustomerPersonEntity(Customer customer);
    }
}
