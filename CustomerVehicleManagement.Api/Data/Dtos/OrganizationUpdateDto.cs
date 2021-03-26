using CustomerVehicleManagement.Domain.BaseClasses;
using SharedKernel.ValueObjects;

namespace CustomerVehicleManagement.Api.Data.Dtos
{
    public class OrganizationUpdateDto : Contactable
    {
        public string Name { get; set; }
        public virtual PersonUpdateDto Contact { get; set; }
        public Address Address { get; set; }
        public string Notes { get; set; }
    }
}
