using Menominee.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    // TODO: No detail for this entitiy.  Can we avoid using this class?
    public class InventoryItemGiftCertificate : Entity
    {
        public long InventoryItemId { get; set; }

        #region ORM

        // EF requires a parameterless constructor
        protected InventoryItemGiftCertificate() { }

        #endregion  
    }
}
