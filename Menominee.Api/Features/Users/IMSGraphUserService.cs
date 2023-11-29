using Menominee.Shared.Models.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Api.Features.Users
{
    public interface IMSGraphUserService
    {
        string ShopIdAttributeName { get; }
        string ShopNameAttributeName { get; }
        string ShopRoleAttributeName { get; }
        Task<List<UserResponse>> GetAllAsync();
        Task<RegisterUserResult> RegisterAsync(RegisterUserRequest user);
        string CustomAttributeName(string baseName);
    }
}