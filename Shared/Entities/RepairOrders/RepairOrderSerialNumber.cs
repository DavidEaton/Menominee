using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Shared.Entities.RepairOrders
{
    public class RepairOrderSerialNumber : Entity
    {
        public long RepairOrderItemId { get; set; }
        public string SerialNumber { get; set; }
    }
}
