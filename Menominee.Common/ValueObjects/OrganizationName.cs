using Menominee.Common.Utilities;
using System.Collections.Generic;

namespace Menominee.Common.ValueObjects
{
    public class OrganizationName : ValueObject
    {
        public string Name { get; }
        public static readonly int MinimumLength = 1;
        public static readonly int MaximumLength = 255;
        public static readonly string UnderMinimumLengthMessage = $"Organization Name cannot be less than {MinimumLength} character(s) in length";
        public static readonly string OverMaximumLengthMessage = $"Organization Name cannot be over {MaximumLength} characters in length";
        
        private OrganizationName(string name)
        {
            Name = name;
        }

        public static Result<OrganizationName> Create(string name)
        {
            name = (name ?? string.Empty).Trim();

            if (name.Length < MinimumLength)
                return Result.Fail<OrganizationName>(UnderMinimumLengthMessage);

            if (name.Length > MaximumLength)
                return Result.Fail<OrganizationName>(OverMaximumLengthMessage);

            return Result.Ok(new OrganizationName(name));
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
