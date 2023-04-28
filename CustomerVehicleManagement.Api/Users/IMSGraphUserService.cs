using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerVehicleManagement.Shared.Models;

namespace CustomerVehicleManagement.Api.Users
{
    public interface IMSGraphUserService
    {
        string ShopIdAttributeName { get; }
        string ShopNameAttributeName { get; }
        string ShopRoleAttributeName { get; }
        Task<List<UserToRead>> GetUsers();
        Task<RegisterUserResult> CreateUser(RegisterUser user);
        public string CustomAttributeName(string baseName);
    }
}