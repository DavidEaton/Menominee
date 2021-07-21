using SharedKernel.ValueObjects;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models
{
    public class OrganizationUpdateDto
    {
        public string Name { get; set; }
        public virtual PersonUpdateDto Contact { get; set; }
        public Address Address { get; set; }
        public string Note { get; set; }
        public IList<PhoneUpdateDto> Phones { get; set; } = new List<PhoneUpdateDto>();
        public IList<EmailUpdateDto> Emails { get; set; } = new List<EmailUpdateDto>();

    }
}
