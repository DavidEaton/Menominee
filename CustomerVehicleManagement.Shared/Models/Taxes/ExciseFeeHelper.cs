using CustomerVehicleManagement.Domain.Entities.Taxes;

namespace CustomerVehicleManagement.Shared.Models.Taxes
{
    public class ExciseFeeHelper
    {
        public static ExciseFee CreateExciseFee(ExciseFeeToWrite exciseFee)
        {
            if (exciseFee == null)
                return null;

            return new()
            {
                Description = exciseFee.Description,
                FeeType = exciseFee.FeeType,
                Amount = exciseFee.Amount
            };
        }

        public static ExciseFeeToWrite CreateExciseFee(ExciseFeeToRead exciseFee)
        {
            if (exciseFee == null)
                return null;

            return new()
            {
                Description = exciseFee.Description,
                FeeType = exciseFee.FeeType,
                Amount = exciseFee.Amount
            };
        }

        public static ExciseFeeToRead CreateExciseFee(ExciseFee exciseFee)
        {
            if (exciseFee is null)
                return null;

            return new()
            {
                Id = exciseFee.Id,
                Description = exciseFee.Description,
                FeeType = exciseFee.FeeType,
                Amount = exciseFee.Amount
            };
        }

        public static ExciseFeeToReadInList CreateExciseFeeInList(ExciseFee excisefee)
        {
            if (excisefee is null)
                return null;

            return new()
            {
                Id = excisefee.Id,
                Description = excisefee.Description,
                FeeType = excisefee.FeeType,
                Amount = excisefee.Amount
            };
        }
    }
}
