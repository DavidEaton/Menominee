using CustomerVehicleManagement.Domain.BaseClasses;
using SharedKernel.ValueObjects;

namespace CustomerVehicleManagement.Api.Data.Models
{
    public class OrganizationUpdateDto : Contactable
    {
        public string Name { get; private set; }
        public virtual PersonUpdateDto Contact { get; private set; }
        public Address Address { get; private set; }
        public string Notes { get; private set; }
    }
}
