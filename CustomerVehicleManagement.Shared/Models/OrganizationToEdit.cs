using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models
{
    public class OrganizationToEdit
    {
        public string Name { get; set; } = string.Empty;
        public virtual PersonToEdit Contact { get; set; }
        public AddressToEdit Address { get; set; }
        public string Note { get; set; } = string.Empty;
        public IList<PhoneToEdit> Phones { get; set; } = new List<PhoneToEdit>();
        public IList<EmailToEdit> Emails { get; set; } = new List<EmailToEdit>();
    }
}
