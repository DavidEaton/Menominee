using CSharpFunctionalExtensions;
using System;
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

        public void SetPercentage(double percentage)
        {
            if (percentage < MinimumValue)
                throw new ArgumentOutOfRangeException(MinimumValueMessage);

            Percentage = percentage;
        }

        public void SetMinimumJobAmount(double minimumJobAmount)
        {
            if (minimumJobAmount < MinimumValue)
                throw new ArgumentOutOfRangeException(MinimumValueMessage);

            MinimumJobAmount = minimumJobAmount;
        }

        public void SetMinimumCharge(double minimumCharge)
        {
            if (minimumCharge < MinimumValue)
                throw new ArgumentOutOfRangeException(MinimumValueMessage);

            MinimumCharge = minimumCharge;
        }

        public void SetMaximumCharge(double maximumCharge)
        {
            if (maximumCharge < MinimumValue)
                throw new ArgumentOutOfRangeException(MinimumValueMessage);

            MaximumCharge = maximumCharge;
        }

        public void SetIncludeParts(bool includeParts)
        {
            IncludeParts = includeParts;
        }

        public void SetIncludeLabor(bool includeLabor)
        {
            IncludeLabor = includeLabor;
        }

        #region ORM

        // EF requires an empty constructor
        public SaleCodeShopSupplies() { }

        #endregion 
    }
}