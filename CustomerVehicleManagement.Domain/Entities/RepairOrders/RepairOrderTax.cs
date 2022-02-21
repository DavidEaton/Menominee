using Menominee.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Domain.Entities.RepairOrders
{
    public class RepairOrderTax : Entity
    {
        public long RepairOrderId { get; set; }
        public int SequenceNumber { get; set; }
        public long TaxId { get; set; }
        public double PartTaxRate { get; set; }
        public double LaborTaxRate { get; set; }
        public double PartTax { get; set; }
        public double LaborTax { get; set; }

        #region ORM

        // EF requires an empty constructor
        public RepairOrderTax() { }

        #endregion
    }
}
