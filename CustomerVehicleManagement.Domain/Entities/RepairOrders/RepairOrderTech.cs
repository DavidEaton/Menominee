using Menominee.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Domain.Entities.RepairOrders
{
    public class RepairOrderTech : Entity
    {
        public long RepairOrderServiceId { get; set; }
        public long TechnicianId { get; set; }

        #region ORM

        // EF requires an empty constructor
        public RepairOrderTech() { }

        #endregion
    }
}
