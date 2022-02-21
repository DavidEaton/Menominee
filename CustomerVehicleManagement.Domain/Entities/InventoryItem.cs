using Menominee.Common;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Domain.Entities
{
    // -------------------------------------------------------------
    // FIX ME - this is just a placeholder
    // -------------------------------------------------------------

    public class InventoryItem : Entity
    {
        public virtual Manufacturer Manufacturer { get; set; }
        public long ManufacturerId { get; set; }
        public string PartNumber { get; set; }
        public string Description { get; set; }
        public virtual ProductCode ProductCode { get; set; }
        public long ProductCodeId { get; set; }
        public PartType PartType { get; set; }
        public int QuantityOnHand { get; set; }
        public double Cost { get; set; }
        public double SuggestedPrice { get; set; }
        public double Labor { get; set; }

        #region ORM

        // EF requires an empty constructor
        public InventoryItem() { }

        #endregion
    }
}
