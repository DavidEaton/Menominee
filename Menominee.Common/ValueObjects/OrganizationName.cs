using CSharpFunctionalExtensions;
using Menominee.Common.Utilities;
using System.Collections.Generic;

namespace Menominee.Common.ValueObjects
{
    public class OrganizationName : AppValueObject
    {
        public string Name { get; private set; }
        public static readonly int MinimumLength = 1;
        public static readonly int MaximumLength = 255;
        public static readonly string MinimumLengthMessage = $"Organization name cannot be less than {MinimumLength} character(s) in length.";
        public static readonly string MaximumLengthMessage = $"Organization name cannot be over {MaximumLength} characters in length.";
        public static readonly string RequiredMessage = $"Organization name is required.";
        private OrganizationName(string name)
        {
            Name = name;
        }

        public static Result<OrganizationName> Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<OrganizationName>(RequiredMessage);

            name = name.Trim();

            if (name.Length < MinimumLength)
                return Result.Failure<OrganizationName>(MinimumLengthMessage);

            if (name.Length > MaximumLength)
                return Result.Failure<OrganizationName>(MaximumLengthMessage);

            return Result.Success(new OrganizationName(name));
        }

        public static OrganizationName NewOrganizationName(string newOrganizationName)
        {
            newOrganizationName = (newOrganizationName ?? string.Empty).Trim();
            Guard.ForNullOrEmpty(newOrganizationName, "newOrganizationName");
            return Create(newOrganizationName).Value;
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
