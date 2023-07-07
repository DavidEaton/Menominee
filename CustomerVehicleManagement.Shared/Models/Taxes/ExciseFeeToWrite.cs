using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models.Taxes
{
    public class ExciseFeeToWrite
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public ExciseFeeType FeeType { get; set; }
        public double Amount { get; set; }
    }
}
