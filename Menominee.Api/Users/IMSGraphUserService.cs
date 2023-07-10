using System.Collections.Generic;
using System.Threading.Tasks;
using Menominee.Shared.Models;

namespace Menominee.Api.Users
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