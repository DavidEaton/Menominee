using CSharpFunctionalExtensions;
using System.Collections.Generic;

namespace Menominee.Common.ValueObjects
{
    public class OrganizationName : AppValueObject
    {
        public string Name { get; private set; }
        public static readonly int MinimumLength = 2;
        public static readonly int MaximumLength = 255;
        public static readonly string InvalidMessage = $"Organization must be between {MinimumLength} and {MaximumLength} character(s) in length.";

        private OrganizationName(string name)
        {
            Name = name;
        }

        public static Result<OrganizationName> Create(string name)
        {
            name = (name ?? "").Trim();

            if (name.Length < MinimumLength || name.Length > MaximumLength)
                return Result.Failure<OrganizationName>($"{InvalidMessage} You entered {name.Length} character(s).");

            return Result.Success(new OrganizationName(name));
        }

        public static OrganizationName NewOrganizationName(string name)
        {
            return Create(name).Value;
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