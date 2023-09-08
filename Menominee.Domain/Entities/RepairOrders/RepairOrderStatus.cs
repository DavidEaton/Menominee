using CSharpFunctionalExtensions;
using System;
using Entity = Menominee.Common.Entity;

namespace Menominee.Domain.Entities.RepairOrders
{
    public class RepairOrderStatus : Entity
    {
        public static readonly string RequiredMessage = $"Please include all required items.";
        public static readonly string InvalidDateMessage = $"Date cannot be in the future.";
        public static readonly int MaximumLength = 999999;
        public static readonly string InvalidLengthMessage = $"Must be under {MaximumLength + 1} characters in length.";

        public Status Status { get; private set; }
        public string Description { get; private set; }
        public DateTime Date { get; private set; }
        private RepairOrderStatus(Status status, string description)
        {
            Status = status;
            Description = (description ?? string.Empty).Trim();
            Date = DateTime.Now;
        }

        public static Result<RepairOrderStatus> Create(
            Status status,
            string description
            )
        {
            if (!Enum.IsDefined(typeof(Status), status))
                return Result.Failure<RepairOrderStatus>(RequiredMessage);

            description = (description ?? string.Empty).Trim();

            if (description.Length > MaximumLength)
                return Result.Failure<RepairOrderStatus>(InvalidLengthMessage);

            return Result.Success(new RepairOrderStatus(status, description));
        }

        public Result<DateTime> SetDate(DateTime date)
        {
            if (date > DateTime.Today)
                return Result.Failure<DateTime>(InvalidDateMessage);

            return Result.Success(Date = date);
        }

        public Result<Status> SetStatus(Status status)
        {
            if (!Enum.IsDefined(typeof(Status), status))
                return Result.Failure<Status>(RequiredMessage);

            return Result.Success(Status = status);
        }

        public Result<string> SetDescription(string description)
        {
            description = (description ?? string.Empty).Trim();

            if (description.Length > MaximumLength)
                return Result.Failure<string>(InvalidLengthMessage);

            return Result.Success(Description = description);
        }

        #region ORM

        // EF requires a parameterless constructor
        protected RepairOrderStatus() { }

        #endregion
    }
}
