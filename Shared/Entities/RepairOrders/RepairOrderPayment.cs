using MenomineePlayWASM.Shared.Entities.RepairOrders.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Shared.Entities.RepairOrders
{
    public class RepairOrderPayment : Entity
    {
        public long RepairOrderId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public double Amount { get; set; }
    }
}
