using CustomerVehicleManagement.Shared.Models;
using System.Threading.Tasks;

namespace Menominee.Client.Secure.Services
{
    public interface IAuthService
    {
        Task<LoginResult> Login(LoginModel loginModel);
        Task Logout();
        Task<RegisterUserResult> Register(RegisterUser registerModel);
    }
}