using CSharpFunctionalExtensions;
using Menominee.Common.Http;
using Menominee.Shared.Models.Customers;

namespace Menominee.Client.Services.Customers
{
    public interface ICustomerDataService
    {
        Task<Result<IReadOnlyList<CustomerToReadInList>>> GetAllAsync();
        Task<Result<CustomerToRead>> GetAsync(long id);
        Task<Result<PostResponse>> AddAsync(CustomerToWrite customer);
        Task<Result> UpdateAsync(CustomerToWrite customer);
        Task<Result> DeleteAsync(long id);
    }
}
