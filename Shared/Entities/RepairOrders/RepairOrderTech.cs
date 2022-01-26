using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Shared.Entities.RepairOrders
{
    public class RepairOrderTech : Entity
    {
        public long RepairOrderServiceId { get; set; }
        public long TechnicianId { get; set; }
    }
}
