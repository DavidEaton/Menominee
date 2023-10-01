using CSharpFunctionalExtensions;
using Menominee.Common.Http;
using Menominee.Shared.Models.Persons;

namespace Menominee.Client.Services
{
    public interface IPersonDataService
    {
        Task<Result<IReadOnlyList<PersonToReadInList>>> GetAllAsync();
        Task<Result<PersonToRead>> GetAsync(long id);
        Task<Result<PostResponse>> AddAsync(PersonToWrite person);
        Task<Result> UpdateAsync(PersonToWrite person);
    }
}
