using Menominee.Common;

namespace CustomerVehicleManagement.Domain.Entities.RepairOrders
{
    public class RepairOrderServiceTax : Entity
    {
        public long RepairOrderServiceId { get; set; }
        public long TaxId { get; set; }
        public double PartTaxRate { get; set; }
        public double LaborTaxRate { get; set; }
        public double PartTax { get; set; }
        public double LaborTax { get; set; }

        #region ORM

        // EF requires a parameterless constructor
        public RepairOrderServiceTax() { }

        #endregion
    }
}
