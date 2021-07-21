using CustomerVehicleManagement.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Customers
{
    public interface ICustomerRepository
    {
        Task AddCustomerAsync(Customer entity);
        Task DeleteCustomerAsync(int id);
        void FixTrackingState();
        Task<bool> CustomerExistsAsync(int id);
        Task<Customer> UpdateCustomerAsync(Customer entity);
        Task<IEnumerable<CustomerReadDto>> GetCustomersAsync();
        Task<CustomerReadDto> GetCustomerAsync(int id);
        Task<bool> SaveChangesAsync();
    }
}
