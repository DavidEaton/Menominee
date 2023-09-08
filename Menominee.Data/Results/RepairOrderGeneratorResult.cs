using Menominee.Domain.Entities;
using Menominee.Domain.Entities.Inventory;
using Menominee.Domain.Entities.RepairOrders;

namespace Menominee.Data.Results
{
    public class RepairOrderGeneratorResult
    {
        public static List<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();
        public static List<RepairOrder> RepairOrders { get; set; } = new List<RepairOrder>();
        public static List<Person> Persons { get; set; } = new List<Person>();
        public static List<Business> Businesses { get; set; } = new List<Business>();
        public static List<Customer> Customers { get; set; } = new List<Customer>();
        public static List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();

    }
}
