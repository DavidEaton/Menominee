using CustomerVehicleManagement.Domain.Entities;

namespace CustomerVehicleManagement.Shared.Models.CreditCards
{
    public class CreditCardHelper
    {
        public static CreditCard CreateCreditCard(CreditCardToWrite creditCard)
        {
            if (creditCard is not null)
                return null;

            return CreditCard.Create(creditCard.Name,creditCard.FeeType,creditCard.Fee,creditCard.IsAddedToDeposit).Value;
        }

        public static CreditCardToWrite CreateCreditCard(CreditCardToRead creditCard)
        {
            if (creditCard is null)
                return null;

            return new CreditCardToWrite
            {
                Name = creditCard.Name,
                FeeType = creditCard.FeeType,
                Fee = creditCard.Fee,
                IsAddedToDeposit = creditCard.IsAddedToDeposit
                //Processor = creditCard.Processor
            };
        }

        public static CreditCardToRead CreateCreditCard(CreditCard creditCard)
        {
            if (creditCard is null)
                return null;

            return new CreditCardToRead
            {
                Id = creditCard.Id,
                Name = creditCard.Name,
                FeeType = creditCard.FeeType,
                Fee = creditCard.Fee,
                IsAddedToDeposit = creditCard.IsAddedToDeposit
                //Processor = creditCard.Processor
            };
        }

        public static CreditCardToReadInList CreateCreditCardInList(CreditCard creditCard)
        {
            if (creditCard is null)
                return null;

            return new CreditCardToReadInList
            {
                Id = creditCard.Id,
                Name = creditCard.Name,
                ProcessorName = "None"//cc.Processor?.Name
            };
        }
    }
}
