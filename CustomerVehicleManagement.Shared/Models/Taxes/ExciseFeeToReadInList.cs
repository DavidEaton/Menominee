using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models.Taxes
{
    public class ExciseFeeToReadInList
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public ExciseFeeType FeeType { get; set; }
        public double Amount { get; set; }
        public string DisplayAmount
        {
            get
            {
                if (FeeType == ExciseFeeType.Percentage)
                    return Amount.ToString("#0.00#'%'");
                return Amount.ToString("C");
            }
        }
    }
}
