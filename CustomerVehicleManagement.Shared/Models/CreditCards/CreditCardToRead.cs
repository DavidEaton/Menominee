using Menominee.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Shared.Models.CreditCards
{
    public class CreditCardToRead
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public CreditCardFeeType FeeType { get; set; }
        public double Fee { get; set; }
        public bool IsAddedToDeposit { get; set; }
        //public CreditCardProcessor Processor { get; set; }
    }
}
