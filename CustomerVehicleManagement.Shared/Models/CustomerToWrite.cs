using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models
{
    public class CustomerToWrite
    {
        public EntityType EntityType { get; set; }
        public CustomerType CustomerType { get; set; }
        public PersonToWrite PersonToWrite { get; set; }
        public OrganizationToWrite OrganizationToWrite { get; set; }
        //public ContactPreferences ContactPreferences { get; set; }
    }
}
