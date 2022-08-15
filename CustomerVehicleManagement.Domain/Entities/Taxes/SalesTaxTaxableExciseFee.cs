using Menominee.Common;

namespace CustomerVehicleManagement.Domain.Entities.Taxes
{
    public class SalesTaxTaxableExciseFee : Entity
    {
        public SalesTax SalesTax { get; set; }
        public ExciseFee ExciseFee { get; set; }

        #region ORM

        // EF requires an empty constructor
        public SalesTaxTaxableExciseFee() { }

        #endregion  

    }
}
