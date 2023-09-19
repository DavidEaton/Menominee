using CSharpFunctionalExtensions;
using Menominee.Common.Http;
using Menominee.Shared.Models.Customers;

namespace Menominee.Client.Services.Customers
{
    public interface ICustomerDataService
    {
        Task<Result<IReadOnlyList<CustomerToReadInList>>> GetAllCustomers();
        Task<Result<CustomerToRead>> GetCustomer(long id);
        Task<Result<PostResponse>> AddCustomer(CustomerToWrite customer);
        Task<Result> UpdateCustomer(CustomerToWrite customer);
        Task DeleteCustomer(long id);
    }
}
