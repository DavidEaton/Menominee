using SharedKernel.Utilities;
using System.Collections.Generic;

namespace SharedKernel.ValueObjects
{
    public class OrganizationName : ValueObject
    {
        public string Name { get; }
        public static readonly int MinimumLength = 1;
        public static readonly int MaximumLength = 255;
        public static readonly string OrganizationNameEmptyMessage = $"Organization Name cannot be less than {MinimumLength} character(s) in length";
        public static readonly string OrganizationNameInvalidMessage = $"Organization Name cannot be over {MaximumLength} characters in length";

        private OrganizationName(string name)
        {
            Name = name;
        }

        public static Result<OrganizationName> Create(string name)
        {
            name = (name ?? string.Empty).Trim();

            if (name.Length < MinimumLength)
                return Result.Fail<OrganizationName>(OrganizationNameEmptyMessage);

            if (name.Length > MaximumLength)
                return Result.Fail<OrganizationName>(OrganizationNameInvalidMessage);

            return Result.Ok(new OrganizationName(name));
        }

        public static OrganizationName NewOrganizationName(string newOrganizationName)
        {
            return Create(newOrganizationName).Value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
        }
    }
}
