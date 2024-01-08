using CSharpFunctionalExtensions;
using Menominee.Shared.Models.Http;
using Menominee.Shared.Models.Users;

namespace Menominee.Client.Services
{
    public interface IUserDataService
    {
        Task<Result<IReadOnlyList<UserResponse>>> GetAllAsync();
        Task<Result<PostResponse>> RegisterAsync(RegisterUserRequest user);
        Task<Result> UpdateAsync(RegisterUserRequest user);
        Task<Result<UserResponse>> GetAsync(string id);
    }
}
