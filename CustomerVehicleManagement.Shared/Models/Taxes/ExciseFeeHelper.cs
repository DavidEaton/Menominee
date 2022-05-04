using CustomerVehicleManagement.Domain.Entities.Taxes;

namespace CustomerVehicleManagement.Shared.Models.Taxes
{
    public class ExciseFeeHelper
    {
        public static ExciseFee Transform(ExciseFeeToWrite feeToWrite)
        {
            if (feeToWrite == null)
                return null;

            return new ExciseFee()
            {
                Description = feeToWrite.Description,
                FeeType = feeToWrite.FeeType,
                Amount = feeToWrite.Amount
            };
        }

        public static ExciseFeeToWrite Transform(ExciseFeeToRead feeToRead)
        {
            return new ExciseFeeToWrite()
            {
                Description = feeToRead.Description,
                FeeType = feeToRead.FeeType,
                Amount = feeToRead.Amount
            };
        }

        public static ExciseFeeToRead Transform(ExciseFee exciseFee)
        {
            if (exciseFee is null)
                return null;

            return new ExciseFeeToRead()
            {
                Id = exciseFee.Id,
                Description = exciseFee.Description,
                FeeType = exciseFee.FeeType,
                Amount = exciseFee.Amount
            };
        }

        public static ExciseFeeToReadInList TransformToListItem(ExciseFee fee)
        {
            if (fee is null)
                return null;

            return new ExciseFeeToReadInList
            {
                Id = fee.Id,
                Description = fee.Description,
                FeeType = fee.FeeType,
                Amount = fee.Amount
            };
        }
    }
}
