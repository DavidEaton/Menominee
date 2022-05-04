using Menominee.Common;
using Menominee.Common.Enums;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Domain.Entities.Taxes
{
    public class SalesTax : Entity
    {
        public string Description { get; set; }
        public SalesTaxType TaxType { get; set; }
        public int Order { get; set; }
        public bool IsAppliedByDefault { get; set; }
        public bool IsTaxable { get; set; }
        public string TaxIdNumber { get; set; }
        public double PartTaxRate { get; set; }
        public double LaborTaxRate { get; set; }
        public virtual List<SalesTaxTaxableExciseFee> TaxedExciseFees { get; set; } = new List<SalesTaxTaxableExciseFee>();

        // TODO - Should we have a list of taxable taxes too?

        #region ORM

        // EF requires an empty constructor
        public SalesTax() { }

        #endregion  
    }
}
