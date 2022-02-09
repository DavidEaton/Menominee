using CSharpFunctionalExtensions;
using Menominee.Common.Utilities;
using System.Collections.Generic;

namespace Menominee.Common.ValueObjects
{
    public class OrganizationName : AppValueObject
    {
        public string Name { get; private set; }
        public static readonly int MinimumLength = 2;
        public static readonly int MaximumLength = 255;
        public static readonly string MinimumLengthMessage = $"Organization name cannot be less than {MinimumLength} character(s) in length.";
        public static readonly string MaximumLengthMessage = $"Organization name cannot be over {MaximumLength} characters in length.";
        public static readonly string RequiredMessage = $"Organization name is required.";
        public static readonly string YouEntered = $"You entered";
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
                return Result.Failure<OrganizationName>($"{MinimumLengthMessage} {YouEntered} {name.Length} character(s).");

            if (name.Length > MaximumLength)
                return Result.Failure<OrganizationName>($"{MaximumLengthMessage} {YouEntered} {name.Length} character(s).");

            return Result.Success(new OrganizationName(name));
        }

        public static OrganizationName NewOrganizationName(string name)
        {
            name = (name ?? string.Empty).Trim();
            Guard.ForNullOrEmpty(name, "name");
            //VK: Are the previous two lines necessary?
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
