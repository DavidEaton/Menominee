using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Api.Customers
{
    public interface ICustomerRepository
    {
        Task AddCustomerAsync(Customer entity);
        Task DeleteCustomerAsync(long id);
        void FixTrackingState();
        Task<bool> CustomerExistsAsync(long id);
        Task<Customer> UpdateCustomerAsync(Customer entity);
        Task<IReadOnlyList<CustomerReadDto>> GetCustomersAsync();
        Task<IReadOnlyList<CustomerInListDto>> GetCustomersInListAsync();
        Task<CustomerReadDto> GetCustomerAsync(long id);
        Task<bool> SaveChangesAsync();
    }
}
