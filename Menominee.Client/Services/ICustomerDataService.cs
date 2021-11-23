using CustomerVehicleManagement.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Client.Services
{
    public interface ICustomerDataService
    {
        Task<IReadOnlyList<CustomerToReadInList>> GetAllCustomers();
        Task<CustomerToRead> GetCustomerDetails(long id);
        Task<CustomerToRead> AddCustomer(CustomerToWrite customer);
        //Task UpdateCustomer(CustomerToWrite customer;
        Task DeleteCustomer(long id);
    }
}
