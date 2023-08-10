using CSharpFunctionalExtensions;
using System.Collections.Generic;

namespace Menominee.Common.ValueObjects
{
    public class BusinessName : AppValueObject
    {
        public static readonly int MinimumLength = 2;
        public static readonly int MaximumLength = 255;
        public static readonly string InvalidMessage = $"Business Name must be between {MinimumLength} and {MaximumLength} character(s) in length.";

        public string Name { get; private set; }

        private BusinessName(string name)
        {
            Name = name;
        }

        public static Result<BusinessName> Create(string name)
        {
            name = (name ?? string.Empty).Trim();

            if (name.Length < MinimumLength || name.Length > MaximumLength)
                return Result.Failure<BusinessName>($"{InvalidMessage} You entered {name.Length} character(s).");

            return Result.Success(new BusinessName(name));
        }

        public Result<BusinessName> NewBusinessName(string name)
        {
            name = (name ?? string.Empty).Trim();

            if (name.Length < MinimumLength || name.Length > MaximumLength)
                return Result.Failure<BusinessName>($"{InvalidMessage} You entered {name.Length} character(s).");
            
            return Result.Success(new BusinessName(name));
        }

        public override string ToString()
        {
            return Name;
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
        }
    }
}