using CustomerVehicleManagement.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Client.Services
{
    public interface ICustomerDataService
    {
        Task<IReadOnlyList<CustomerToReadInList>> GetAllCustomers();
        Task<CustomerToRead> GetCustomerDetails(long id);
        Task<CustomerToRead> AddCustomer(CustomerToAdd person);
        //Task UpdateCustomer(CustomerUpdateDto person);
        Task DeleteCustomer(long id);
    }
}
