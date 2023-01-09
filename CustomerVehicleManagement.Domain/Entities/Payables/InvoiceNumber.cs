using Menominee.Common.ValueObjects;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Domain.Entities.Payables
{
    // TODO: Replace stringly-typed VendorInvoice.InvoiceNumber
    // with this strongly-typed AppValueObject
    public class InvoiceNumber : AppValueObject
    {
        public string Value { get; set; }
        public InvoiceNumber(string invoiceNumber)
        {
            Value = invoiceNumber;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        #region ORM

        // EF requires a parameterless constructor
        protected InvoiceNumber() { }

        #endregion

    }



}

