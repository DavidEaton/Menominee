using CSharpFunctionalExtensions;
using Menominee.Domain.Entities.Payables;
using Menominee.Domain.Entities.Taxes;
using Menominee.Domain.Enums;
using Menominee.Shared.Models.Payables.Invoices;
using Menominee.Shared.Models.Payables.Invoices.LineItems;
using Menominee.Shared.Models.Payables.Invoices.LineItems.Items;
using Menominee.Shared.Models.Payables.Invoices.Payments;
using Menominee.Shared.Models.Payables.Invoices.Taxes;
using Menominee.Shared.Models.Payables.Vendors;
using Menominee.Shared.Models.Taxes;
using TestingHelperLibrary.Fakers;

namespace TestingHelperLibrary.Payables
{
    public static class VendorInvoiceTestHelper
    {
        public static VendorInvoiceToWrite CreateVendorInvoiceToWrite() => new()
        {
            Date = DateTime.Today,
            InvoiceNumber = "123",
            Vendor = CreateVendorToRead(),
            Status = VendorInvoiceStatus.Open,
            Total = 0
        };

        public static VendorInvoiceToWrite CreateVendorInvoiceToWrite(Vendor vendor)
        {
            return new()
            {
                Date = DateTime.Today,
                DocumentType = VendorInvoiceDocumentType.Invoice,
                Status = VendorInvoiceStatus.Open,
                Total = 10.0,
                Vendor = new VendorToRead()
                {
                    Id = vendor.Id,
                    IsActive = vendor.IsActive,
                    Name = vendor.Name,
                    VendorCode = vendor.VendorCode,
                    VendorRole = vendor.VendorRole
                }
            };
        }

        public static VendorInvoiceToWrite CreateVendorInvoiceToWrite(VendorToRead vendor)
        {
            return new()
            {
                Date = DateTime.Today,
                DocumentType = VendorInvoiceDocumentType.Invoice,
                Status = VendorInvoiceStatus.Open,
                Total = 10.0,
                Vendor = vendor
            };
        }

        public static VendorToRead CreateVendorToRead()
        {
            return new VendorToRead()
            {
                Id = 1,
                IsActive = true,
                Name = Utilities.RandomCharacters(Vendor.MinimumLength) + 1,
                VendorCode = Utilities.RandomCharacters(Vendor.MinimumLength + 1),
                VendorRole = VendorRole.PartsSupplier
            };
        }

        public static IReadOnlyList<string> CreateVendorInvoiceNumbers(Vendor vendor, List<int> invoiceNumbers)
        {
            return invoiceNumbers.Select(invoiceNumber => $"{vendor.Id}{invoiceNumber}").ToList();
        }

        public static List<string> CreateVendorInvoiceNumbersList(Vendor vendor)
        {
            return new List<string>()
            {
                { $"{vendor.Id}{1}" },
                { $"{vendor.Id}{2}" },
                { $"{vendor.Id}{3}" },
            };
        }

        public static IList<VendorInvoiceLineItemToWrite> CreateLineItemsToWrite(LineItemTestOptions options)
        {
            var result = new List<VendorInvoiceLineItemToWrite>();

            for (int i = 0; i < options.RowCount; i++)
            {
                result.Add(
                    new VendorInvoiceLineItemToWrite()
                    {
                        Core = options.Core,
                        Cost = options.Cost,
                        Quantity = options.Quantity,
                        PONumber = options.PONumber,
                        Type = options.Type,
                        Item = options.Item,
                        TransactionDate = options.TransactionDate
                    });
            }

            return result;
        }

        public static IList<VendorInvoiceLineItemToWrite> CreateLineItemsToWrite(VendorInvoiceLineItemType lineItemType, int lineItemCount, double core, double cost, double lineItemQuantity)
        {
            var lineItems = new List<VendorInvoiceLineItemToWrite>();

            for (int i = 0; i < lineItemCount; i++)
            {
                lineItems.Add(
                    new VendorInvoiceLineItemToWrite()
                    {
                        Core = core,
                        Cost = cost,
                        Quantity = lineItemQuantity,
                        PONumber = string.Empty,
                        Type = lineItemType,
                        Item = new VendorInvoiceItemToWrite()
                        {
                            Description = $"a desription for {i}",
                            PartNumber = $"Part {Utilities.RandomCharacters(i)}"
                        }
                    });
            }

            return lineItems;
        }

        public static IList<VendorInvoiceLineItem> CreateLineItems(VendorInvoiceLineItemType type, int lineItemCount, double core, double cost, double itemQuantity)
        {
            var lineItems = new List<VendorInvoiceLineItem>();
            for (int i = 0; i < lineItemCount; i++)
            {
                VendorInvoiceItem item = VendorInvoiceItem.Create(
                    $"Part {Utilities.RandomCharacters(i)}",
                    "a desription")
                    .Value;
                lineItems.Add(
                    VendorInvoiceLineItem.Create(type, item, itemQuantity, cost, core)
                .Value);
            }

            return lineItems;
        }

        public static IList<VendorInvoiceTaxToWrite> CreateTaxesToWrite(SalesTax salesTax, int taxLineCount, double taxAmount)
        {
            var taxes = new List<VendorInvoiceTaxToWrite>();

            for (int i = 0; i < taxLineCount; i++)
            {
                taxes.Add(new VendorInvoiceTaxToWrite()
                {
                    Amount = taxAmount,
                    SalesTax = new()
                    {
                        Id = salesTax.Id,
                        Description = salesTax.Description,
                        IsAppliedByDefault = salesTax.IsAppliedByDefault,
                        IsTaxable = salesTax.IsTaxable,
                        LaborTaxRate = salesTax.LaborTaxRate,
                        Order = salesTax.Order,
                        PartTaxRate = salesTax.PartTaxRate,
                        TaxIdNumber = salesTax.TaxIdNumber,
                        TaxType = salesTax.TaxType
                        //ExciseFees = salesTax.ExciseFees
                    }
                });
            }

            return taxes;
        }

        public static IList<VendorInvoiceTaxToWrite> CreateTaxesToWrite(SalesTaxToRead salesTax, int taxLineCount, double taxAmount)
        {
            var taxes = new List<VendorInvoiceTaxToWrite>();

            for (int i = 0; i < taxLineCount; i++)
            {
                taxes.Add(new VendorInvoiceTaxToWrite()
                {
                    Amount = taxAmount,
                    SalesTax = salesTax
                });
            }

            return taxes;
        }

        public static IList<VendorInvoiceTax> CreateTaxes(int taxLineCount, double taxAmount)
        {
            var taxes = new List<VendorInvoiceTax>();

            for (int i = 0; i < taxLineCount; i++)
            {
                taxes.Add(VendorInvoiceTax.Create(
                    CreateSalesTax(),
                    taxAmount)
                    .Value);
            }

            return taxes;
        }

        public static List<VendorInvoiceTax> CreateVendorInvoiceTaxes(IReadOnlyList<VendorInvoiceTaxToWrite> taxes)
        {
            return (taxes ?? new List<VendorInvoiceTaxToWrite>())
            .Select(tax =>
            {
                var createdTax = createTax(tax);
                var createdSalesTax = createSalesTax(tax);
                createdTax.SetSalesTax(createdSalesTax);
                return createdTax;

                static VendorInvoiceTax createTax(VendorInvoiceTaxToWrite tax)
                {
                    var editedTax = new VendorInvoiceTaxFaker(generateId: false, id: tax.Id).Generate();
                    editedTax.SetAmount(tax.Amount);
                    return editedTax;
                }

                static SalesTax createSalesTax(VendorInvoiceTaxToWrite tax)
                {
                    var editedSalesTax = new SalesTaxFaker(generateId: false, id: tax.SalesTax.Id).Generate();
                    editedSalesTax.SetTaxType(tax.SalesTax.TaxType);
                    editedSalesTax.SetTaxIdNumber(tax.SalesTax.TaxIdNumber);
                    editedSalesTax.SetPartTaxRate(tax.SalesTax.PartTaxRate);
                    editedSalesTax.SetDescription(tax.SalesTax.Description);
                    editedSalesTax.SetIsAppliedByDefault(tax.SalesTax.IsAppliedByDefault);
                    editedSalesTax.SetIsTaxable(tax.SalesTax.IsTaxable);
                    editedSalesTax.SetLaborTaxRate(tax.SalesTax.LaborTaxRate);
                    editedSalesTax.SetOrder(tax.SalesTax.Order);
                    return editedSalesTax;
                }
            })
            .ToList();
        }

        public static IList<VendorInvoicePaymentToWrite> CreatePaymentsToWrite(int paymentCount, double paymentAmount, VendorInvoicePaymentMethodToRead paymentMethod)
        {
            var payments = new List<VendorInvoicePaymentToWrite>();

            for (int i = 0; i < paymentCount; i++)
            {
                payments.Add(new VendorInvoicePaymentToWrite()
                {
                    Amount = paymentAmount,
                    PaymentMethod = paymentMethod
                });
            }

            return payments;
        }

        public static IList<VendorInvoicePayment> CreatePayments(int paymentCount, double paymentAmount)
        {
            var payments = new List<VendorInvoicePayment>();

            for (int i = 0; i < paymentCount; i++)
            {
                payments.Add(VendorInvoicePayment.Create(
                    VendorInvoicePaymentMethod.Create(
                        (IReadOnlyList<string>)CreatePaymentMethodNames(5),
                        Utilities.RandomCharacters(VendorInvoicePaymentMethod.MinimumLength + i),
                        isActive: true,
                        VendorInvoicePaymentMethodType.Normal,
                        reconcilingVendor: null).Value,
                    paymentAmount + i).Value);
            }

            return payments;
        }

        public static VendorInvoicePaymentMethod CreateVendorInvoicePaymentMethod()
        {
            string name = Utilities.RandomCharacters(VendorInvoicePaymentMethod.MinimumLength + 30);
            bool isActive = true;
            List<string> paymentMethodNames = new();

            return VendorInvoicePaymentMethod.Create(
                paymentMethodNames, name, isActive, VendorInvoicePaymentMethodType.Normal).Value;
        }

        public static VendorInvoicePayment CreateVendorInvoicePayment()
        {
            var paymentMethod = CreateVendorInvoicePaymentMethod();
            double amount = VendorInvoicePayment.InvalidValue + 1;
            return VendorInvoicePayment.Create(paymentMethod, amount).Value;
        }

        public static IList<VendorInvoicePaymentMethodToRead> CreateVendorInvoicePaymentMethodsToRead(int count)
        {
            var result = new List<VendorInvoicePaymentMethodToRead>();

            for (int i = 0; i < count; i++)
            {
                result.Add(new VendorInvoicePaymentMethodToRead()
                {
                    Id = i,
                    Name = Utilities.RandomCharacters(VendorInvoicePaymentMethod.MaximumLength - i),
                    IsActive = true,
                    //IsOnAccountPaymentType = false,
                    PaymentType = VendorInvoicePaymentMethodType.Normal
                });
            }

            return result;
        }

        public static IList<string> CreatePaymentMethodNames(int count)
        {
            IList<string> result = new List<string>();
            var list = CreateVendorInvoicePaymentMethodsToRead(count);

            foreach (var method in list)
            {
                result.Add(method.Name);
            }

            return result;
        }

        public static SalesTax CreateSalesTax(int descriptionSeed = 0)
        {
            var description = Utilities.RandomCharacters(descriptionSeed + 100);
            var taxType = SalesTaxType.Normal;
            var order = (int)SalesTax.MinimumValue + 10;
            var taxIdNumber = Utilities.RandomCharacters((int)SalesTax.MinimumValue + 11);
            var partTaxRate = SalesTax.MinimumValue + .1;
            var laborTaxRate = SalesTax.MinimumValue + .25;
            bool? isAppliedByDefault = true;
            bool? isTaxable = true;

            return SalesTax.Create(description, taxType, order, taxIdNumber, partTaxRate, laborTaxRate, isAppliedByDefault: isAppliedByDefault, isTaxable: isTaxable).Value;
        }

        public static SalesTaxToRead CreateSalesTaxToRead()
        {
            return new()
            {
                Id = 1,
                Description = Utilities.LoremIpsum(SalesTax.DescriptionMaximumLength - 10),
                ExciseFees = new(),
                IsAppliedByDefault = true,
                IsTaxable = true,
                LaborTaxRate = .05,
                Order = 1,
                PartTaxRate = .06,
                TaxIdNumber = "001",
                TaxType = SalesTaxType.Normal
            };
        }

        public static VendorInvoice CreateVendorInvoice(Vendor vendor, int childRowCount)
        {
            var invoice = VendorInvoice.Create(
                vendor: vendor,
                status: VendorInvoiceStatus.Open,
                documentType: VendorInvoiceDocumentType.Invoice,
                total: VendorInvoice.MinimumValue + new Random().NextDouble() * (999999 - VendorInvoice.MinimumValue),
                vendorInvoiceNumbers: CreateVendorInvoiceNumbersList(vendor),
                invoiceNumber: $"{Utilities.RandomCharacters(7)}")
                .Value;

            var lineItems = CreateLineItems(VendorInvoiceLineItemType.Purchase, childRowCount, 1.1, 2.2, 3);
            var payments = CreatePayments(childRowCount, 1.2);
            var taxes = CreateTaxes(childRowCount, .75);

            foreach (var item in lineItems)
                invoice.AddLineItem(item);

            foreach (var payment in payments)
                invoice.AddPayment(payment);

            foreach (var tax in taxes)
                invoice.AddTax(tax);

            return invoice;
        }

        public static List<VendorInvoicePayment> CreateVendorInvoicePayments(IReadOnlyList<VendorInvoicePaymentToWrite> payments)
        {
            return (payments ?? new List<VendorInvoicePaymentToWrite>())
                .Select(payment =>
                {
                    var editedPayment = new VendorInvoicePaymentFaker(generateId: false, id: payment.Id).Generate();
                    var paymentMethod = new VendorInvoicePaymentMethodFaker(generateId: false, payment.PaymentMethod.Id).Generate();
                    editedPayment.SetAmount(payment.Amount);
                    editedPayment.SetPaymentMethod(paymentMethod);
                    return editedPayment;
                })
                .ToList();
        }

        public static List<VendorInvoiceLineItem> CreateVendorInvoiceLineItems(List<VendorInvoiceLineItemToWrite> lineItems)
        {
            return (lineItems ?? new List<VendorInvoiceLineItemToWrite>())
                .Select(lineItem =>
                {
                    var editedLineItem = new VendorInvoiceLineItemFaker(generateId: false, id: lineItem.Id).Generate();
                    editedLineItem.SetCore(lineItem.Core);
                    editedLineItem.SetCost(lineItem.Cost);
                    editedLineItem.SetItem(VendorInvoiceItem.Create(lineItem.Item.PartNumber, lineItem.Item.Description).Value);
                    editedLineItem.SetPONumber(lineItem.PONumber);
                    editedLineItem.SetQuantity(lineItem.Quantity);
                    editedLineItem.SetTransactionDate(lineItem.TransactionDate);
                    editedLineItem.SetType(lineItem.Type);
                    return editedLineItem;
                })
                .ToList();
        }
    }
}
