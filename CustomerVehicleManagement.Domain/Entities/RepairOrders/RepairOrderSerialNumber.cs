using Menominee.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Domain.Entities.RepairOrders
{
    public class RepairOrderSerialNumber : Entity
    {
        public long RepairOrderItemId { get; set; }
        public string SerialNumber { get; set; }

        #region ORM

        // EF requires an empty constructor
        public RepairOrderSerialNumber() { }

        #endregion
    }
}
