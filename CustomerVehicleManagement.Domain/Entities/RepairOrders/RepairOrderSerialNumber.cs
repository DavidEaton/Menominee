using CSharpFunctionalExtensions;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities.RepairOrders
{
    // TODO: DDD: Rename this class to ServiceLineSerialNumber
    public class RepairOrderSerialNumber : Entity
    {
        public static readonly string RequiredMessage = $"Please include all required items.";
        public static readonly int MinimumLength = 2;
        public static readonly int MaximumLength = 255;
        public static readonly string InvalidLengthMessage = $"SerialNumber must be between {MinimumLength} and {MaximumLength} characters in length";
        public string SerialNumber { get; private set; }

        private RepairOrderSerialNumber(string serialNumber)
        {
            SerialNumber = serialNumber;
        }

        public static Result<RepairOrderSerialNumber> Create(string serialNumber)
        {
            if (string.IsNullOrWhiteSpace(serialNumber))
                return Result.Failure<RepairOrderSerialNumber>(RequiredMessage);

            if (serialNumber.Length < MinimumLength ||
                serialNumber.Length > MaximumLength)
                return Result.Failure<RepairOrderSerialNumber>(InvalidLengthMessage);

            return Result.Success(new RepairOrderSerialNumber(serialNumber));
        }
        public Result<string> SetSerialNumber(string serialNumber)
        {
            if (string.IsNullOrWhiteSpace(serialNumber))
                return Result.Failure<string>(RequiredMessage);

            serialNumber = (serialNumber ?? string.Empty).Trim();

            if (serialNumber.Length < MinimumLength ||
                serialNumber.Length > MaximumLength)
                return Result.Failure<string>(InvalidLengthMessage);

            return Result.Success(SerialNumber = serialNumber);
        }

        #region ORM

        // EF requires a parameterless constructor
        protected RepairOrderSerialNumber() { }

        #endregion
    }
}
