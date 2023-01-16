using Menominee.Common;
using Menominee.Common.Enums;

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

        // EF requires a parameterless constructor
        public CreditCard() { }

        #endregion     
    }
}
