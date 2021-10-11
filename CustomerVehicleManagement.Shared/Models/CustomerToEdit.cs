using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models
{
    public class CustomerToEdit
    {
        public CustomerType CustomerType { get; set; }
        public PersonToEdit PersonUpdateDto { get; set; }
        public OrganizationToEdit OrganizationUpdateDto { get; set; }
        //public ContactPreferences ContactPreferences { get; set; }
    }
}
