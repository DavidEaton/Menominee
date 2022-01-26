using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Shared.Entities.RepairOrders.Enums
{
    public enum PaymentMethod
    {
        Cash,
        Check,
        Charge,
        Deposit,
        RefundCheck,
        OtherBilling,
        Deductible,
        Text2Pay,
        Card
    }
}
