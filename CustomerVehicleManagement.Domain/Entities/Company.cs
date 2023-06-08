using CSharpFunctionalExtensions;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class Company : Entity
    {
        public static readonly long MinimumValue = 0;
        public static readonly string MinimumValueMessage = $"Invoice Number Starting value must be >= {MinimumValue}.";
        public static readonly string RequiredMessage = $"Please include all required items.";

        public Organization Organization { get; private set; }
        public long NextInvoiceNumberOrSeed { get; private set; } = 0;
        private Company(Organization organization, long invoiceNumberSeed)
        {
            Organization = organization;
            NextInvoiceNumberOrSeed = invoiceNumberSeed;
        }

        public static Result<Company> Create(Organization organization, long seed)
        {
            if (organization is null)
                return Result.Failure<Company>(RequiredMessage);

            if (seed <= MinimumValue || seed > long.MaxValue)
                return Result.Failure<Company>(MinimumValueMessage);

            return Result.Success(new Company(organization, seed));
        }

        public Result<long> SetInvoiceNumberSeed(long seed)
        {
            if (seed <= MinimumValue || seed > long.MaxValue)
                return Result.Failure<long>(MinimumValueMessage);

            return Result.Success(NextInvoiceNumberOrSeed = seed);
        }

        #region ORM

        // EF requires a parameterless constructor
        protected Company() { }

        #endregion
    }
}
