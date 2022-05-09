using CustomerVehicleManagement.Domain.Entities;

namespace CustomerVehicleManagement.Shared.Models.CreditCards
{
    public class CreditCardHelper
    {
        public static CreditCard CreateCreditCard(CreditCardToWrite creditCard)
        {
            if (creditCard == null)
                return null;

            return new CreditCard()
            {
                Name = creditCard.Name,
                FeeType = creditCard.FeeType,
                Fee = creditCard.Fee,
                IsAddedToDeposit = creditCard.IsAddedToDeposit
                //Processor = creditCard.Processor
            };
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

        public static CreditCardToReadInList CreateCreditCardInList(CreditCard cc)
        {
            if (cc is null)
                return null;

            return new CreditCardToReadInList
            {
                Id = cc.Id,
                Name = cc.Name,
                ProcessorName = "None"//cc.Processor?.Name
            };
        }
    }
}
