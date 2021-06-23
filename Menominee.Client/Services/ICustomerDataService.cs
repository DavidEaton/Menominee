using CustomerVehicleManagement.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Client.Services
{
    public interface ICustomerDataService
    {
        Task<IReadOnlyList<CustomerInListDto>> GetAllCustomers();
        Task<CustomerReadDto> GetCustomerDetails(int id);
        Task<CustomerReadDto> AddCustomer(CustomerCreateDto person);
        //Task UpdateCustomer(CustomerUpdateDto person);
        Task DeleteCustomer(int id);
    }
}
