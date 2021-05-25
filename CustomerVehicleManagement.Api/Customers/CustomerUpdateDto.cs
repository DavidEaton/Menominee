using SharedKernel.Enums;
using SharedKernel.Interfaces;
using SharedKernel.ValueObjects;

namespace CustomerVehicleManagement.Api.Customers
{
    public class CustomerUpdateDto
    {
        public IEntity Entity { get; private set; }
        public CustomerType CustomerType { get; set; }
        public ContactPreferences ContactPreferences { get; private set; }
    }
}
