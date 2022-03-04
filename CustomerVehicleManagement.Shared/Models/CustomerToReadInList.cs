using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models
{
    public class CustomerToReadInList
    {
        public long Id { get; set; }
        public EntityType EntityType { get; set; }
        public long EntityId { get; set; }
        public string Name { get; set; }
        public string AddressFull { get; set; }
        public string PrimaryPhone { get; set; }
        public string PrimaryEmail { get; set; }
        public string CustomerType { get; set; }
    }
}
