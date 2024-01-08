using Menominee.Domain.Enums;

namespace Menominee.Shared.Models.CreditCards
{
    public class CreditCardToWrite
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public CreditCardFeeType FeeType { get; set; }
        public double Fee { get; set; }
        public bool? IsAddedToDeposit { get; set; }
        //public CreditCardProcessor Processor { get; set; }
    }
}
