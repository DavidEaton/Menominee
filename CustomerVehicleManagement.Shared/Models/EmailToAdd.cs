using CustomerVehicleManagement.Domain.Entities;

namespace CustomerVehicleManagement.Shared.Models
{
    public class EmailToAdd
    {
        public string Address { get; set; } = string.Empty;
        public bool IsPrimary { get; set; } = false;

        public static Email ConvertToEntity(EmailToAdd email)
        {
            if (email != null)
            {
                return Email.Create(email.Address, email.IsPrimary).Value;
            }

            return null;
        }
    }
}
