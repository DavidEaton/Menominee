using CSharpFunctionalExtensions;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class SaleCodeShopSupplies : Entity
    {
        public static readonly double MinimumValue = 0;
        public static readonly string MinimumValueMessage = $"Value(s) cannot be negative.";
        public static readonly string RequiredMessage = $"Please include all required items.";

        public double Percentage { get; private set; }
        public double MinimumJobAmount { get; private set; }
        public double MinimumCharge { get; private set; }
        public double MaximumCharge { get; private set; }
        public bool IncludeParts { get; private set; }
        public bool IncludeLabor { get; private set; }

        private SaleCodeShopSupplies(
            double percentage,
            double minimumJobAmount,
            double minimumCharge,
            double maximumCharge,
            bool includeParts,
            bool includeLabor)
        {
            Percentage = percentage;
            MinimumJobAmount = minimumJobAmount;
            MinimumCharge = minimumCharge;
            MaximumCharge = maximumCharge;
            IncludeParts = includeParts;
            IncludeLabor = includeLabor;
        }

        public static Result<SaleCodeShopSupplies> Create(
            double percentage,
            double minimumJobAmount,
            double minimumCharge,
            double maximumCharge,
            bool includeParts,
            bool includeLabor)
        {
            if (percentage < MinimumValue)
                return Result.Failure<SaleCodeShopSupplies>(MinimumValueMessage);

            if (minimumJobAmount < MinimumValue)
                return Result.Failure<SaleCodeShopSupplies>(MinimumValueMessage);

            if (minimumCharge < MinimumValue)
                return Result.Failure<SaleCodeShopSupplies>(MinimumValueMessage);

            if (maximumCharge < MinimumValue)
                return Result.Failure<SaleCodeShopSupplies>(MinimumValueMessage);

            return Result.Success(new SaleCodeShopSupplies(percentage, minimumJobAmount, minimumCharge, maximumCharge, includeParts, includeLabor));
        }

        public Result<double> SetPercentage(double percentage)
        {
            if (percentage < MinimumValue)
                return Result.Failure<double>(MinimumValueMessage);

            return Result.Success(Percentage = percentage);
        }

        public Result<double> SetMinimumJobAmount(double minimumJobAmount)
        {
            if (minimumJobAmount < MinimumValue)
                return Result.Failure<double>(MinimumValueMessage);

            return Result.Success(MinimumJobAmount = minimumJobAmount);
        }

        public Result<double> SetMinimumCharge(double minimumCharge)
        {
            if (minimumCharge < MinimumValue)
                return Result.Failure<double>(MinimumValueMessage);

            return Result.Success(MinimumCharge = minimumCharge);
        }

        public Result<double> SetMaximumCharge(double maximumCharge)
        {
            if (maximumCharge < MinimumValue)
                return Result.Failure<double>(MinimumValueMessage);

            return Result.Success(MaximumCharge = maximumCharge);
        }

        public Result<bool> SetIncludeParts(bool includeParts)
        {
            return Result.Success(IncludeParts = includeParts);
        }

        public Result<bool> SetIncludeLabor(bool includeLabor)
        {
            return Result.Success(IncludeLabor = includeLabor);
        }

        #region ORM

        // EF requires a parameterless constructor
        public SaleCodeShopSupplies() { }

        #endregion 
    }
}