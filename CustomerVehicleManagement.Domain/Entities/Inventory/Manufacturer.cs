using CSharpFunctionalExtensions;
using System;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class Manufacturer : Entity
    {
        public static readonly string RequiredMessage = $"Please include all required items.";
        public static readonly string InvalidLengthMessage = $"Each item must be between {MinimumLength} and {MaximumLength} characters";
        public static readonly int MinimumLength = 1;
        public static readonly int MaximumLength = 255;

        public string Name { get; private set; }
        public string Prefix { get; private set; }
        public string Code { get; private set; }
        //public xxx Country { get; private set; }
        //public xxx Franchise { get; private set; }

        private Manufacturer(string name, string prefix, string code)
        {
            Name = name;
            Prefix = prefix;
            Code = code;
        }

        public static Result<Manufacturer> Create(string name, string prefix, string code)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<Manufacturer>(RequiredMessage);

            if (string.IsNullOrWhiteSpace(prefix))
                return Result.Failure<Manufacturer>(RequiredMessage);

            if (string.IsNullOrWhiteSpace(code))
                return Result.Failure<Manufacturer>(RequiredMessage);

            name = (name ?? string.Empty).Trim();
            prefix = (prefix ?? string.Empty).Trim();
            code = (code ?? string.Empty).Trim();

            if (name.Length > MaximumLength || name.Length < MinimumLength
                ||
                prefix.Length > MaximumLength || prefix.Length < MinimumLength
                ||
                code.Length > MaximumLength || code.Length < MinimumLength
                )
                return Result.Failure<Manufacturer>(InvalidLengthMessage);

            return Result.Success(new Manufacturer(name, prefix, code));
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentOutOfRangeException(RequiredMessage);

            name = (name ?? string.Empty).Trim();

            if (name.Length > MaximumLength || name.Length < MinimumLength)
                throw new ArgumentOutOfRangeException(InvalidLengthMessage);

            Name = name;
        }

        public void SetPrefix(string prefix)
        {
            if (string.IsNullOrWhiteSpace(prefix))
                throw new ArgumentOutOfRangeException(RequiredMessage);

            prefix = (prefix ?? string.Empty).Trim();

            if (prefix.Length > MaximumLength || prefix.Length < MinimumLength)
                throw new ArgumentOutOfRangeException(InvalidLengthMessage);

            Prefix = prefix;
        }

        public void SetCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentOutOfRangeException(RequiredMessage);

            code = (code ?? string.Empty).Trim();

            if (code.Length > MaximumLength || code.Length < MinimumLength)
                throw new ArgumentOutOfRangeException(InvalidLengthMessage);

            Code = code;
        }

        #region ORM

        // EF requires an empty constructor
        public Manufacturer() { }

        #endregion

    }
}
