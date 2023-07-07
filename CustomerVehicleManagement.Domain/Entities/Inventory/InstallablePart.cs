using CSharpFunctionalExtensions;
using CustomerVehicleManagement.Domain.Entities.Taxes;
using System.Collections.Generic;
using System.Linq;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public abstract class InstallablePart : Entity
    {
        // Always test only concrete classes; don’t test abstract classes directly.
        // Abstract classes are implementation details. Targeting tests at the
        // abstract base class binds them to the code’s implementation details, 
        // which is an anit-pattern.
        public static readonly string RequiredMessage = $"Please include all required items.";
        public static readonly int MaximumLineCodeLength = 10;
        public static readonly int MinimumMoneyAmount = 0;
        public static readonly int MaximumMoneyAmount = 99999;
        public static readonly string InvalidMoneyAmountMessage = $"Amount must be between {MinimumMoneyAmount} and {MaximumMoneyAmount}.";
        public static readonly string InvalidLineCodeLengthMessage = $"Line Code must not be more than {MaximumLineCodeLength} characters";
        public double List { get; private set; }
        public double Cost { get; private set; }
        public double Core { get; private set; }
        public double Retail { get; private set; }
        public TechAmount TechAmount { get; private set; }
        public string LineCode { get; private set; }
        public string SubLineCode { get; private set; }
        public bool Fractional { get; private set; }
        public double ExciseFeesTotal => ExciseFees.Select(
            exciseFee => exciseFee.Amount)
            .Sum();

        private IList<ExciseFee> exciseFees = new List<ExciseFee>();
        public IReadOnlyList<ExciseFee> ExciseFees => exciseFees.ToList();

        protected InstallablePart(double list, double cost, double core, double retail, TechAmount techAmount, bool fractional, string lineCode = null, string subLineCode = null, List<ExciseFee> exciseFees = null)
        {
            List = list;
            Cost = cost;
            Core = core;
            Retail = retail;
            TechAmount = techAmount;
            LineCode = lineCode;
            SubLineCode = subLineCode;
            Fractional = fractional;
            this.exciseFees = exciseFees ?? new List<ExciseFee>();
        }

        public Result<List<ExciseFee>> UpdateExciseFees(IReadOnlyList<ExciseFee> exciseFees)
        {
            var toAdd = exciseFees
                .Where(fee => fee.Id == 0)
                .ToList();

            var toModify = ExciseFees
                .Where(fee => exciseFees.Any(
                    callerfee => callerfee.Id == fee.Id))
                .ToList();

            var toDelete = ExciseFees
                .Where(fee => !exciseFees.Any(callerfee => callerfee.Id == fee.Id))
                .ToList();

            var combinedAddResult = toAdd
                .Select(fee => AddExciseFee(fee))
                .ToList()
                .Combine()
                .Map(list => list.ToList());

            var fees = new List<ExciseFee>();
            var errors = new List<string>();

            foreach (var fee in toModify)
            {
                var feeFromCaller = exciseFees.Single(callerfee => callerfee.Id == fee.Id);

                var feeTypeResult = fee.FeeType != feeFromCaller.FeeType
                    ? fee.SetFeeType(feeFromCaller.FeeType).Map(_ => fee)
                    : Result.Success(fee);

                var amountResult = fee.Amount != feeFromCaller.Amount
                    ? fee.SetAmount(feeFromCaller.Amount).Map(_ => fee)
                    : Result.Success(fee);

                var descriptionResult = fee.Description != feeFromCaller.Description
                    ? fee.SetDescription(feeFromCaller.Description).Map(_ => fee)
                    : Result.Success(fee);

                var feeCombinedResult = Result.Combine(new[] { feeTypeResult, amountResult, descriptionResult });

                if (feeCombinedResult.IsSuccess)
                    fees.Add(fee);
                else
                    errors.Add(feeCombinedResult.Error);
            }

            var combinedModifyResult = errors.Any()
                ? Result.Failure<List<ExciseFee>>(string.Join(", ", errors))
                : Result.Success(fees);

            var combinedDeleteResult = toDelete
                .Select(fee => RemoveExciseFee(fee))
                .ToList()
                .Combine()
                .Map(list => list.ToList());

            var combinedResult = Result.Combine(
                combinedAddResult,
                combinedModifyResult,
                combinedDeleteResult);

            return combinedResult.IsSuccess
                ? Result.Success(ExciseFees.ToList())
                : Result.Failure<List<ExciseFee>>(combinedResult.Error);
        }


        public Result<double> SetList(double list)
        {
            if (list < MinimumMoneyAmount || list > MaximumMoneyAmount)
                return Result.Failure<double>(InvalidMoneyAmountMessage);

            return Result.Success(List = list);
        }

        public Result<double> SetCost(double cost)
        {
            if (cost < MinimumMoneyAmount || cost > MaximumMoneyAmount)
                return Result.Failure<double>(InvalidMoneyAmountMessage);

            return Result.Success(Cost = cost);
        }

        public Result<double> SetCore(double core)
        {
            if (core < MinimumMoneyAmount || core > MaximumMoneyAmount)
                return Result.Failure<double>(InvalidMoneyAmountMessage);

            return Result.Success(Core = core);
        }

        public Result<double> SetRetail(double retail)
        {
            if (retail < MinimumMoneyAmount || retail > MaximumMoneyAmount)
                return Result.Failure<double>(InvalidMoneyAmountMessage);

            return Result.Success(Retail = retail);
        }

        public Result<TechAmount> SetTechAmount(TechAmount techAmount)
        {
            if (techAmount is null)
                return Result.Failure<TechAmount>(RequiredMessage);

            return Result.Success(TechAmount = techAmount);
        }

        public Result<string> SetLineCode(string lineCode)
        {
            if (lineCode.Length > MaximumLineCodeLength)
                return Result.Failure<string>(InvalidLineCodeLengthMessage);

            return Result.Success(LineCode = lineCode);
        }

        public Result<string> SetSubLineCode(string subLineCode)
        {
            if (subLineCode.Length > MaximumLineCodeLength)
                return Result.Failure<string>(InvalidLineCodeLengthMessage);

            return Result.Success(SubLineCode = subLineCode);
        }

        public Result<bool> SetFractional(bool fractional)
        {
            return Result.Success(Fractional = fractional);
        }

        public Result<ExciseFee> AddExciseFee(ExciseFee fee)
        {
            if (fee is null)
                return Result.Failure<ExciseFee>(RequiredMessage);

            exciseFees.Add(fee);

            return Result.Success(fee);

        }

        public Result<ExciseFee> RemoveExciseFee(ExciseFee fee)
        {
            if (fee is null)
                return Result.Failure<ExciseFee>(RequiredMessage);

            exciseFees.Remove(fee);

            return Result.Success(fee);
        }

        #region ORM

        // EF requires a parameterless constructor
        protected InstallablePart()
        {
            exciseFees = new List<ExciseFee>();
        }

        #endregion
    }
}
