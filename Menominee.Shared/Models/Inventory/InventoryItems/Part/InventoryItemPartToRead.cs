using Menominee.Shared.Models.Inventory.InventoryItems.Labor;
using Menominee.Shared.Models.Taxes;
using System.Collections.Generic;

namespace Menominee.Shared.Models.Inventory.InventoryItems.Part
{
    public class InventoryItemPartToRead
    {
        public long Id { get; set; }
        public double List { get; set; }
        public double Cost { get; set; }
        public double Core { get; set; }
        public double Retail { get; set; }
        public TechAmountToRead TechAmount { get; set; }
        public string LineCode { get; set; }
        public string SubLineCode { get; set; }
        public bool Fractional { get; set; }
        public IReadOnlyList<ExciseFeeToRead> ExciseFees { get; set; } = new List<ExciseFeeToRead>();
    }
}
