using Menominee.Common;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class CreditCard : Entity
    {
        public string Name { get; set; }
        public CreditCardFeeType FeeType { get; set; }
        public double Fee { get; set; }
        public bool? IsAddedToDeposit { get; set; }

        //public virtual CreditCardProcessor Processor { get; set; }

        #region ORM

        // EF requires an empty constructor
        public CreditCard() { }

        #endregion     
    }
}
