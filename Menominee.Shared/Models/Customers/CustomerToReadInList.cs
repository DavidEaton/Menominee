using Menominee.Common.Enums;

namespace Menominee.Shared.Models.Customers
{
    public class CustomerToReadInList
    {
        public long Id { get; set; }
        public EntityType EntityType { get; set; }
        public string Name { get; set; }
        public string AddressFull { get; set; }
        public string PrimaryPhone { get; set; }
        public string PrimaryEmail { get; set; }
        public CustomerType CustomerType { get; set; }
    }
}
