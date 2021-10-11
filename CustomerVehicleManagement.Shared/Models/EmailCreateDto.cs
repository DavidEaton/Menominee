using CustomerVehicleManagement.Domain.Entities;

namespace CustomerVehicleManagement.Shared.Models
{
    public class EmailCreateDto
    {
        public string Address { get; set; } = string.Empty;
        public bool IsPrimary { get; set; } = false;

        public static Email ConvertToEntity(EmailCreateDto email)
        {
            if (email != null)
            {
                return new Email(email.Address, email.IsPrimary);
            }

            return null;
        }
    }
}
