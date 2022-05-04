using Menominee.Common;
using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Domain.Entities.Taxes
{
    public class ExciseFee : Entity
    {
        public string Description { get; set; }
        public ExciseFeeType FeeType { get; set; }
        public double Amount { get; set; }

        #region ORM

        // EF requires an empty constructor
        public ExciseFee() { }

        #endregion
    }
}
