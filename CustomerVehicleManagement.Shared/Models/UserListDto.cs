using SharedKernel.Enums;

namespace CustomerVehicleManagement.Shared.Models
{
    public class UserListDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public ShopRole ShopRole { get; set; }
    }
}
