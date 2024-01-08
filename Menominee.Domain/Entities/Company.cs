using CSharpFunctionalExtensions;
using Entity = Menominee.Domain.BaseClasses.Entity;

namespace Menominee.Domain.Entities
{
    public class Company : Entity
    {
        public static readonly long MinimumValue = 0;
        public static readonly string MinimumValueMessage = $"Invoice Number Starting value must be >= {MinimumValue}.";
        public static readonly string RequiredMessage = $"Please include all required items.";

        public Business Business { get; private set; }
        public long NextInvoiceNumberOrSeed { get; private set; } = 0;
        private Company(Business business, long invoiceNumberSeed)
        {
            Business = business;
            NextInvoiceNumberOrSeed = invoiceNumberSeed;
        }

        public static Result<Company> Create(Business business, long seed)
        {
            if (business is null)
                return Result.Failure<Company>(RequiredMessage);

            if (seed <= MinimumValue || seed > long.MaxValue)
                return Result.Failure<Company>(MinimumValueMessage);

            return Result.Success(new Company(business, seed));
        }

        public Result<long> SetInvoiceNumberSeed(long seed)
        {
            return seed <= MinimumValue || seed > long.MaxValue
                ? Result.Failure<long>(MinimumValueMessage)
                : Result.Success(NextInvoiceNumberOrSeed = seed);
        }

        #region ORM

        // EF requires a parameterless constructor
        protected Company() { }

        #endregion
    }
}
