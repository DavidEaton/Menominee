using CSharpFunctionalExtensions;
using Menominee.Shared.Models.Customers;

namespace Menominee.Client.Services.Customers
{
    public interface ICustomerDataService
    {
        Task<Result<IReadOnlyList<CustomerToReadInList>>> GetAllCustomers();
        Task<Result<CustomerToRead>> GetCustomer(long id);
        Task<CustomerToRead> AddCustomer(CustomerToWrite customer);
        Task UpdateCustomer(CustomerToWrite customer);
        Task DeleteCustomer(long id);
    }
}
