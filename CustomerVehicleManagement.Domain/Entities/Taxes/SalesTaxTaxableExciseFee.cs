using Menominee.Common;

namespace CustomerVehicleManagement.Domain.Entities.Taxes
{
    public class SalesTaxTaxableExciseFee : Entity
    {
        public virtual SalesTax SalesTax { get; set; }
        public virtual ExciseFee ExciseFee { get; set; }

        #region ORM

        // EF requires an empty constructor
        public SalesTaxTaxableExciseFee() { }

        #endregion  

    }
}
