using CustomerVehicleManagement.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Client.Services
{
    public interface ICustomerDataService
    {
        Task<IReadOnlyList<CustomerToReadInList>> GetAllCustomers();
        Task<CustomerToRead> GetCustomer(long id);
        Task<CustomerToRead> AddCustomer(CustomerToWrite customer);
        Task UpdateCustomer(CustomerToWrite customer, long id);
        Task DeleteCustomer(long id);
    }
}
