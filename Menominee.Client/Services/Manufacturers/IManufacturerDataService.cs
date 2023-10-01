using CSharpFunctionalExtensions;
using Menominee.Common.Http;
using Menominee.Shared.Models.Manufacturers;

namespace Menominee.Client.Services.Manufacturers
{
    public interface IManufacturerDataService
    {
        Task<Result<IReadOnlyList<ManufacturerToReadInList>>> GetAllAsync();
        Task<Result<ManufacturerToRead>> GetAsync(long id);
        Task<Result<ManufacturerToRead>> GetAsync(string code);
        Task<Result<PostResponse>> AddAsync(ManufacturerToWrite manufacturer);
        Task<Result> UpdateAsync(ManufacturerToWrite manufacturer);
    }
}
