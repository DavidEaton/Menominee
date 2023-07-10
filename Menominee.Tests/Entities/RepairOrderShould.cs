using Bogus;
using CSharpFunctionalExtensions;
using Menominee.Domain.Entities.RepairOrders;
using Menominee.Shared.Models.RepairOrders.Payments;
using Menominee.Shared.Models.RepairOrders.Services;
using Menominee.Shared.Models.RepairOrders.Statuses;
using Menominee.Shared.Models.RepairOrders.Taxes;
using Menominee.Shared.Models.SaleCodes;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using TestingHelperLibrary.Fakers;
using TestingHelperLibrary.RepairOrders;
using Xunit;

namespace Menominee.Tests.Entities
{
    public class RepairOrderShould
    {
        [Fact]
        public void Create_RepairOrder()
        {
            var faker = new Faker();
            var accountingDate = faker.Date.Between(DateTime.Today.AddDays(RepairOrder.AccountingDateGracePeriodInDays), DateTime.Today).AddYears(-1);
            var repairOrderNumbers = new List<long>();
            var lastInvoiceNumber = faker.Random.Long(1000, 100000);

            var result = RepairOrder.Create(
                new CustomerFaker(true),
                new VehicleFaker(true),
                accountingDate,
                repairOrderNumbers,
                lastInvoiceNumber);

            result.Should().NotBeNull();
            result.Value.Should().BeOfType<RepairOrder>();
        }

        [Fact]
        public void Initialize_With_New_Status_On_Create()
        {
            var repairOrder = new RepairOrderFaker(true).Generate();

            repairOrder.Status.Should().Be(Status.New);
        }

        [Fact]
        public void Return_True_For_IsDeclined_If_All_LineItems_Are_Declined()
        {
            var service = new RepairOrderServiceFaker(true).Generate();
            var lineItems = new List<RepairOrderLineItem>
            {
                new RepairOrderLineItemFaker(true).RuleFor(item => item.IsDeclined, true).Generate(),
                new RepairOrderLineItemFaker(true).RuleFor(item => item.IsDeclined, true).Generate(),
                new RepairOrderLineItemFaker(true).RuleFor(item => item.IsDeclined, true).Generate()
            };

            lineItems.ForEach(lineItem =>
                service.AddLineItem(lineItem));

            service.IsDeclined.Should().BeTrue();
        }

        [Fact]
        public void Return_False_For_IsDeclined_If_Any_LineItem_Is_Not_Declined()
        {
            var service = new RepairOrderServiceFaker(true).Generate();
            var lineItems = new List<RepairOrderLineItem>
            {
                new RepairOrderLineItemFaker(true).RuleFor(item => item.IsDeclined, true).Generate(),
                new RepairOrderLineItemFaker(true).RuleFor(item => item.IsDeclined, false).Generate(),
                new RepairOrderLineItemFaker(true).RuleFor(item => item.IsDeclined, true).Generate()
            };

            lineItems.ForEach(lineItem =>
                service.AddLineItem(lineItem));

            service.IsDeclined.Should().BeFalse();
        }

        [Fact]
        public void Return_Failure_On_Create_RepairOrder_With_AccountingDate_In_The_Future()
        {
            var customer = new CustomerFaker(true);
            var vehicle = new VehicleFaker(true);
            var accountingDate = DateTime.Now.AddDays(11);
            var repairOrderNumbers = new List<long>();
            var lastInvoiceNumber = 0;

            var result = RepairOrder.Create(customer, vehicle, accountingDate, repairOrderNumbers, lastInvoiceNumber);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrder.AccountingDateInvalidMessage);
        }

        [Fact]
        public void SetCustomer()
        {
            var repairOrder = CreateRepairOrder().Value;
            var newCustomer = new CustomerFaker(true);

            repairOrder.SetCustomer(newCustomer);

            repairOrder.Customer.Should().NotBeNull();
        }

        [Fact]
        public void Return_Failure_On_Set_Missing_Customer()
        {
            var repairOrder = CreateRepairOrder().Value;

            var result = repairOrder.SetCustomer(null);

            repairOrder.Customer.Should().NotBeNull();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrder.RequiredMessage);
        }

        [Fact]
        public void SetVehicle()
        {
            var repairOrder = CreateRepairOrder().Value;
            var newVehicle = new VehicleFaker(true);

            var result = repairOrder.SetVehicle(newVehicle);

            repairOrder.Vehicle.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void Return_Failure_On_Set_Missing_Vehicle()
        {
            var repairOrder = CreateRepairOrder().Value;

            var result = repairOrder.SetVehicle(null);

            repairOrder.Vehicle.Should().NotBeNull();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrder.RequiredMessage);
        }

        [Fact]
        public void SetInvoiceNumber()
        {
            var repairOrder = CreateRepairOrder().Value;
            var newInvoiceNumber = 1234567890L;

            var result = repairOrder.SetInvoiceNumber(newInvoiceNumber);

            result.IsSuccess.Should().BeTrue();
            repairOrder.InvoiceNumber.Should().Be(newInvoiceNumber);
        }

        [Fact]
        public void Return_Failure_On_Set_Invalid_Invoice_Number()
        {
            var repairOrder = CreateRepairOrder().Value;

            var result = repairOrder.SetInvoiceNumber(-1);

            repairOrder.Vehicle.Should().NotBeNull();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrder.InvalidNumberMessage);
        }

        [Fact]
        public void SetDateModified()
        {
            var repairOrder = CreateRepairOrder().Value;
            var newDate = DateTime.Now.AddDays(-1);
            var result = repairOrder.SetDateModified(newDate);
            result.IsSuccess.Should().BeTrue();
            repairOrder.DateModified.Should().Be(newDate);
        }

        [Fact]
        public void Return_Failure_On_Set_Invalid_DateModified()
        {
            var repairOrder = CreateRepairOrder().Value;
            var futureDate = DateTime.Now.AddHours(1);
            var result = repairOrder.SetDateModified(futureDate);
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrder.DateInvalidMessage);
        }

        [Fact]
        public void SetAccountingDate()
        {
            var repairOrder = CreateRepairOrder().Value;
            var expectedDate = new DateTime(2023, 6, 1);

            var result = repairOrder.SetAccountingDate(expectedDate);

            result.IsSuccess.Should().BeTrue();
            repairOrder.AccountingDate.Should().Be(expectedDate);
        }

        [Fact]
        public void Return_Failure_On_Set_Invalid_AccountingDate()
        {
            var repairOrder = CreateRepairOrder().Value;

            var result = repairOrder.SetAccountingDate(
                DateTime.Today.AddDays(RepairOrder.AccountingDateGracePeriodInDays + 1));

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrder.AccountingDateInvalidMessage);
        }
        [Fact]
        public void Return_Correct_DateInvoiced()
        {
            var repairOrder = CreateRepairOrder().Value;
            var statuses = new List<Status>
            {
                Status.New,
                Status.EstimateQuote,
                Status.Approved,
                Status.InProgress,
                Status.Invoiced,
                Status.PickedUp,
                Status.Completed
            };
            var descriptions = new List<string>
            {
                "test description New",
                "test description EstimateQuote",
                "test description Approved",
                "test description InProgress",
                "test description 2",
                "test description 2",
                "test description 2"
            };

            var createdStatuses = statuses
                .Select((status, index) => RepairOrderStatus.Create(status, descriptions[index]).Value)
                .ToList();

            createdStatuses.ForEach(status => repairOrder.AddStatus(status, new List<long>()));

            var statusInvoiced = createdStatuses
                .FirstOrDefault(status => status.Type == Status.Invoiced);

            repairOrder.DateInvoiced.Should().Be(statusInvoiced.Date);

            createdStatuses
                .Where(status => status != statusInvoiced)
                .ToList()
                .ForEach(status => status.Date.Should().NotBe(repairOrder.DateInvoiced));
        }


        [Fact]
        public void Return_Most_Recent_Status()
        {
            var repairOrder = new RepairOrderFaker(true, 3, 3, 3, 3).Generate();

            var status = RepairOrderStatus.Create(Status.Completed, "test description").Value;
            var result = repairOrder.AddStatus(status, new List<long>());

            result.IsFailure.Should().BeFalse();
            repairOrder.Status.Should().Be(Status.Completed);
        }

        [Fact]
        public void AddStatus()
        {
            var repairOrder = CreateRepairOrder().Value;
            var status = RepairOrderStatus.Create(Status.Completed, "test description").Value;

            var result = repairOrder.AddStatus(status, new List<long>());

            result.IsSuccess.Should().BeTrue();
            repairOrder.Statuses.Should().Contain(status);
        }

        [Fact]
        public void Not_Add_Null_Status()
        {
            var repairOrder = CreateRepairOrder().Value;

            var result = repairOrder.AddStatus(null, new List<long>());

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrder.RequiredMessage);
        }

        [Fact]
        public void RemoveStatus()
        {
            var repairOrder = CreateRepairOrder().Value;
            var status = RepairOrderStatus.Create(Status.Completed, "test description").Value;
            repairOrder.AddStatus(status, new List<long>());

            var result = repairOrder.RemoveStatus(status);

            result.IsSuccess.Should().BeTrue();
            repairOrder.Statuses.Should().NotContain(status);
        }

        [Fact]
        public void Return_Failure_On_Remove_Null_Status()
        {
            var repairOrder = CreateRepairOrder().Value;

            var result = repairOrder.RemoveStatus(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrder.RequiredMessage);
        }

        [Fact]
        public void Return_Failure_On_Remove_Nonexistent_Status()
        {
            var repairOrder = CreateRepairOrder().Value;
            var status = RepairOrderStatus.Create(Status.Completed, "test description").Value;

            var result = repairOrder.RemoveStatus(status);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrder.RequiredMessage);
        }

        [Fact]
        public void AddService()
        {
            var repairOrder = CreateRepairOrder().Value;
            var saleCode = new SaleCodeFaker(true).Generate();
            var service = RepairOrderService.Create("Test Service", saleCode, 0).Value;

            var result = repairOrder.AddService(service);

            result.IsSuccess.Should().BeTrue();
            repairOrder.Services.Should().Contain(service);
        }

        [Fact]
        public void Return_Failure_On_Add_Null_Service()
        {
            var repairOrder = CreateRepairOrder().Value;

            var result = repairOrder.AddService(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrder.RequiredMessage);
        }

        [Fact]
        public void RemoveService()
        {
            var repairOrder = CreateRepairOrder().Value;
            var saleCode = new SaleCodeFaker(true).Generate();
            var service = RepairOrderService.Create("Test Service", saleCode, 0).Value;
            repairOrder.AddService(service);

            var result = repairOrder.RemoveService(service);

            result.IsSuccess.Should().BeTrue();
            repairOrder.Services.Should().NotContain(service);
        }

        [Fact]
        public void Return_Failure_On_Remove_Null_Service()
        {
            var repairOrder = CreateRepairOrder().Value;

            var result = repairOrder.RemoveService(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrder.RequiredMessage);
        }

        [Fact]
        public void Return_Failure_On_Remove_Nonexistent_Service()
        {
            var repairOrder = CreateRepairOrder().Value;
            var saleCode = new SaleCodeFaker(true).Generate();
            var service = RepairOrderService.Create("Test Service", saleCode, 0).Value;

            var result = repairOrder.RemoveService(service);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrder.RequiredMessage);
        }

        [Fact]
        public void AddTax()
        {
            var repairOrder = CreateRepairOrder().Value;
            var tax = RepairOrderTax.Create(
                PartTax.Create(.06, 1.23).Value,
                LaborTax.Create(44.44, 444.44).Value)
                .Value;

            var result = repairOrder.AddTax(tax);

            result.IsSuccess.Should().BeTrue();
            repairOrder.Taxes.Should().Contain(tax);
        }

        [Fact]
        public void Return_Failure_On_Add_Null_Tax()
        {
            var repairOrder = CreateRepairOrder().Value;

            var result = repairOrder.AddTax(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrder.RequiredMessage);
        }

        [Fact]
        public void RemoveTax()
        {
            var repairOrder = CreateRepairOrder().Value;
            var tax = RepairOrderTax.Create(
                PartTax.Create(.06, 1.23).Value,
                LaborTax.Create(44.44, 444.44).Value)
                .Value;
            repairOrder.AddTax(tax);

            var result = repairOrder.RemoveTax(tax);

            result.IsSuccess.Should().BeTrue();
            repairOrder.Taxes.Should().NotContain(tax);
        }

        [Fact]
        public void Return_Failure_On_Remove_Null_Tax()
        {
            var repairOrder = CreateRepairOrder().Value;

            var result = repairOrder.RemoveTax(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrder.RequiredMessage);
        }

        [Fact]
        public void Return_Failure_On_Remove_Nonexistent_Tax()
        {
            var repairOrder = CreateRepairOrder().Value;
            var tax = RepairOrderTax.Create(
                PartTax.Create(.06, 1.23).Value,
                LaborTax.Create(44.44, 444.44).Value)
                .Value;

            var result = repairOrder.RemoveTax(tax);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrder.RequiredMessage);
        }

        [Fact]
        public void AddPayment()
        {
            var repairOrder = CreateRepairOrder().Value;
            var payment = new RepairOrderPaymentFaker(true).Generate();

            var result = repairOrder.AddPayment(payment);

            result.IsSuccess.Should().BeTrue();
            repairOrder.Payments.Should().Contain(payment);
        }

        [Fact]
        public void Return_Failure_On_Add_Null_Payment()
        {
            var repairOrder = CreateRepairOrder().Value;

            var result = repairOrder.AddPayment(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrder.RequiredMessage);
        }

        [Fact]
        public void Return_Failure_On_Remove_Nonexistent_Payment()
        {
            var repairOrder = CreateRepairOrder().Value;
            var payment = new RepairOrderPaymentFaker(true).Generate();

            var result = repairOrder.RemovePayment(payment);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrder.RequiredMessage);
        }

        [Fact]
        public void RemovePayment()
        {
            var repairOrder = CreateRepairOrder().Value;
            var payment = new RepairOrderPaymentFaker(true).Generate();
            repairOrder.AddPayment(payment);

            var result = repairOrder.RemovePayment(payment);

            result.IsSuccess.Should().BeTrue();
            repairOrder.Payments.Should().NotContain(payment);
        }

        [Fact]
        public void Not_Remove_Null_Payment()
        {
            var repairOrder = CreateRepairOrder().Value;

            var result = repairOrder.RemovePayment(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrder.RequiredMessage);
        }

        [Fact]
        public void Return_Earliest_Invoiced_Date()
        {
            var repairOrder = new RepairOrderFaker(true, 0).Generate();
            var repairOrderNumbers = new List<long>();

            var earlier = repairOrder.AddStatus(RepairOrderStatus.Create(
                Status.New, "test description").Value,
                repairOrderNumbers).Value;

            var later = repairOrder.AddStatus(RepairOrderStatus.Create(
                Status.Invoiced, "test description").Value,
                repairOrderNumbers).Value;

            var greaterDate = later.Date > earlier.Date ? later.Date : earlier.Date;

            greaterDate.Should().BeAfter(earlier.Date);
            repairOrder.Status.Should().Be(Status.Invoiced);
        }

        [Theory]
        [InlineData(new long[] { 20230523001, 20230523002 }, 20230523003)]
        [InlineData(new long[] { 20230523001, 20230523002, 20230523003 }, 20230523004)]
        [InlineData(new long[] { }, 20230523001)]
        [InlineData(new long[] { 20230523001 }, 20230523002)]
        public void Set_Correct_RepairOrderNumber(long[] existingRepairOrderNumbers, long expectedRepairOrderNumber)
        {
            var repairOrder = CreateRepairOrder().Value;
            var currentDate = new DateTime(2023, 05, 23);

            var result = repairOrder.SetRepairOrderNumber(existingRepairOrderNumbers.ToList(), currentDate);

            result.IsSuccess.Should().BeTrue();
            repairOrder.RepairOrderNumber.Should().Be(expectedRepairOrderNumber);
        }

        public double CalculateTotal(IEnumerable<RepairOrderService> services, Func<RepairOrderLineItem, double> totalSelector)
        {
            return services.Select(service => service.LineItems.Select(totalSelector).Sum()).Sum();
        }

        [Fact]
        public void Return_Correct_Service_Totals()
        {
            var repairOrder = new RepairOrderFaker(true).Generate();
            var services = new RepairOrderServiceFaker(true, lineItemsCount: 3).Generate(count: 3);
            services.ForEach(service => repairOrder.AddService(service));

            var partsTotal = CalculateTotal(repairOrder.Services, lineItem => lineItem.SellingPrice * lineItem.QuantitySold);
            var laborTotal = CalculateTotal(repairOrder.Services, lineItem => lineItem.LaborAmount.Amount * lineItem.QuantitySold);
            var discountTotal = CalculateTotal(repairOrder.Services, lineItem => lineItem.DiscountAmount.Amount * lineItem.QuantitySold);
            var exciseFeesTotal = CalculateTotal(repairOrder.Services, lineItem => lineItem.ExciseFeesTotal);
            var serviceTaxTotal = CalculateTotal(repairOrder.Services, lineItem => lineItem.Taxes.Sum(tax => tax.LaborTax.Amount + tax.PartTax.Amount));
            var shopSuppliesTotal = repairOrder.Services.Select(service =>
                service.ShopSuppliesTotal).Sum();
            var total = repairOrder.Services.Select(service =>
                service.PartsTotal + service.LaborTotal + service.DiscountTotal + service.ExciseFeesTotal + service.ShopSuppliesTotal)
                .Sum();
            var taxTotal = repairOrder.Taxes.Select(tax =>
                tax.PartTax.Amount + tax.LaborTax.Amount).Sum();
            var totalWithTax = total + taxTotal;

            repairOrder.PartsTotal.Should().Be(partsTotal);
            repairOrder.LaborTotal.Should().Be(laborTotal);
            repairOrder.DiscountTotal.Should().Be(discountTotal);
            repairOrder.ExciseFeesTotal.Should().Be(exciseFeesTotal);
            repairOrder.ServiceTaxTotal.Should().Be(serviceTaxTotal);
            repairOrder.ShopSuppliesTotal.Should().Be(shopSuppliesTotal);
            repairOrder.Total.Should().Be(total);
            repairOrder.TaxTotal.Should().Be(taxTotal);
            repairOrder.TotalWithTax.Should().Be(totalWithTax);
        }

        [Fact]
        public void Add_Added_Payments_On_UpdatePayments()
        {
            var paymentsCount = 3;
            var repairOrder = new RepairOrderFaker(true, paymentsCount: paymentsCount).Generate();
            var addedPayments = new RepairOrderPaymentFaker(generateId: false).Generate(paymentsCount);
            var updatedPayments = repairOrder.Payments.Concat(addedPayments).ToList();
            repairOrder.Payments.Should().NotBeEquivalentTo(addedPayments);

            var result = repairOrder.UpdatePayments(updatedPayments);

            result.IsSuccess.Should().BeTrue();
            repairOrder.Payments.Should().NotBeEquivalentTo(addedPayments);
            repairOrder.Payments.Count.Should().Be(paymentsCount + paymentsCount);
        }

        [Fact]
        public void Update_Edited_Payments_On_UpdatePayments()
        {
            var paymentsCount = 3;
            var repairOrder = new RepairOrderFaker(true, paymentsCount: paymentsCount).Generate();
            // Faker randomly selects PaymentMethod in Payments so reset to known:
            foreach (var payment in repairOrder.Payments)
                payment.SetPaymentMethod(Menominee.Common.Enums.PaymentMethod.Card);

            var originalPayments = repairOrder.Payments.Select(payment =>
            {
                return new RepairOrderPaymentToWrite
                {
                    Id = payment.Id,
                    PaymentMethod = payment.PaymentMethod,
                    Amount = payment.Amount,
                };
            }).ToList();
            var editedPayments = repairOrder.Payments.Select(payment =>
            {
                return new RepairOrderPaymentToWrite
                {
                    Id = payment.Id,
                    PaymentMethod = payment.PaymentMethod,
                    Amount = payment.Amount + 1,
                };
            }).ToList();
            repairOrder.Payments.Should().NotBeEquivalentTo(editedPayments);
            var updatedPayments = RepairOrderTestHelper.CreateRepairOrderPayments(editedPayments);

            var result = repairOrder.UpdatePayments(updatedPayments);

            result.IsSuccess.Should().BeTrue();
            repairOrder.Payments.Count.Should().Be(paymentsCount);
            foreach (var payment in repairOrder.Payments)
            {
                var paymentFromCaller = editedPayments.Single(callerPayment => callerPayment.Id == payment.Id);
                paymentFromCaller.Should().NotBeNull();
                paymentFromCaller.Amount.Should().Be(payment.Amount);
                paymentFromCaller.PaymentMethod.Should().Be(payment.PaymentMethod);
            }
            repairOrder.Payments.Should().NotBeEquivalentTo(originalPayments);
        }

        [Fact]
        public void Delete_Deleted_Payments_On_UpdatePayments()
        {
            var paymentsCount = 3;
            var repairOrder = new RepairOrderFaker(true, paymentsCount: paymentsCount).Generate();
            var originalPayments = repairOrder.Payments.Select(payment =>
            {
                return new RepairOrderPaymentToWrite
                {
                    Id = payment.Id,
                    PaymentMethod = payment.PaymentMethod,
                    Amount = payment.Amount,
                };
            }).ToList();
            var deletedPayments = repairOrder.Payments.ToList();
            deletedPayments.RemoveAt(0);
            repairOrder.Payments.Should().NotBeEquivalentTo(deletedPayments);
            var updatedPayments = deletedPayments;

            var result = repairOrder.UpdatePayments(updatedPayments);

            result.IsSuccess.Should().BeTrue();
            repairOrder.Payments.Count.Should().Be(paymentsCount - 1);
            repairOrder.Payments.Should().NotBeEquivalentTo(originalPayments);
        }

        [Fact]
        public void Add_Added_Taxes_On_UpdateTaxes()
        {
            var taxesCount = 3;
            var repairOrder = new RepairOrderFaker(true, taxesCount: taxesCount).Generate();
            var addedTaxes = new RepairOrderTaxFaker(generateId: false).Generate(taxesCount);
            var updatedTaxes = repairOrder.Taxes.Concat(addedTaxes).ToList();
            repairOrder.Taxes.Should().NotBeEquivalentTo(addedTaxes);

            var result = repairOrder.UpdateTaxes(updatedTaxes);

            result.IsSuccess.Should().BeTrue();
            repairOrder.Taxes.Should().NotBeEquivalentTo(addedTaxes);
            repairOrder.Taxes.Count.Should().Be(taxesCount + taxesCount);
        }

        [Fact]
        public void Update_Edited_Taxes_On_UpdateTaxes()
        {
            var taxesCount = 3;
            var repairOrder = new RepairOrderFaker(true, taxesCount: taxesCount).Generate();
            var originalTaxes = repairOrder.Taxes.Select(tax =>
            {
                return new RepairOrderTaxToWrite
                {
                    Id = tax.Id,
                    PartTax = tax.PartTax is null
                        ? new()
                        : new()
                        {
                            Amount = tax.PartTax.Amount,
                            Rate = tax.PartTax.Rate
                        },
                    LaborTax = tax.LaborTax is null
                        ? new()
                        : new()
                        {
                            Amount = tax.LaborTax.Amount,
                            Rate = tax.LaborTax.Rate
                        }
                };
            }).ToList();
            var editedTaxes = repairOrder.Taxes.Select(tax =>
            {
                return new RepairOrderTaxToWrite
                {
                    Id = tax.Id,
                    PartTax = tax.PartTax is null
                        ? new()
                        : new()
                        {
                            Amount = tax.PartTax.Amount + 1,
                            Rate = tax.PartTax.Rate
                        },
                    LaborTax = tax.LaborTax is null
                        ? new()
                        : new()
                        {
                            Amount = tax.LaborTax.Amount + 1,
                            Rate = tax.LaborTax.Rate
                        }
                };
            }).ToList();
            repairOrder.Taxes.Should().NotBeEquivalentTo(editedTaxes);
            var updatedTaxes = RepairOrderTestHelper.CreateRepairOrderTaxes(editedTaxes);

            var result = repairOrder.UpdateTaxes(updatedTaxes);

            result.IsSuccess.Should().BeTrue();
            repairOrder.Taxes.Count.Should().Be(taxesCount);
            repairOrder.Taxes.Should().BeEquivalentTo(editedTaxes);
            repairOrder.Taxes.Should().NotBeEquivalentTo(originalTaxes);
        }

        [Fact]
        public void Delete_Deleted_Taxes_On_UpdateTaxes()
        {
            var taxesCount = 3;
            var repairOrder = new RepairOrderFaker(true, taxesCount: taxesCount).Generate();
            var originalTaxes = repairOrder.Taxes.Select(tax =>
            {
                return new RepairOrderTaxToWrite
                {
                    Id = tax.Id,
                    PartTax = tax.PartTax is null
                        ? new()
                        : new()
                        {
                            Amount = tax.PartTax.Amount,
                            Rate = tax.PartTax.Rate
                        },
                    LaborTax = tax.LaborTax is null
                        ? new()
                        : new()
                        {
                            Amount = tax.LaborTax.Amount,
                            Rate = tax.LaborTax.Rate
                        }
                };
            }).ToList();

            var deletedTaxes = repairOrder.Taxes.ToList();
            deletedTaxes.RemoveAt(0);
            repairOrder.Taxes.Should().NotBeEquivalentTo(deletedTaxes);
            var updatedTaxes = deletedTaxes;

            var result = repairOrder.UpdateTaxes(updatedTaxes);

            result.IsSuccess.Should().BeTrue();
            repairOrder.Taxes.Count.Should().Be(taxesCount - 1);
            repairOrder.Taxes.Should().NotBeEquivalentTo(originalTaxes);
        }

        [Fact]
        public void Add_Added_Statuses_On_UpdateStatuses()
        {
            var statusesCount = 3;
            var repairOrder = new RepairOrderFaker(true, statusesCount: statusesCount).Generate();
            var addedStatuses = new RepairOrderStatusFaker(generateId: false).Generate(statusesCount);
            var updatedStatuses = repairOrder.Statuses.Concat(addedStatuses).ToList();
            repairOrder.Statuses.Should().NotBeEquivalentTo(addedStatuses);
            var repairOrderNumbers = new List<long>();

            var result = repairOrder.UpdateStatuses(updatedStatuses, repairOrderNumbers);

            result.IsSuccess.Should().BeTrue();
            repairOrder.Statuses.Should().NotBeEquivalentTo(addedStatuses);
            repairOrder.Statuses.Count.Should().Be(statusesCount + statusesCount);
        }

        [Fact]
        public void Update_Edited_Statuses_On_UpdateStatuses()
        {
            var updatedDescription = "a description updated by test";
            var statusesCount = 3;
            var repairOrder = new RepairOrderFaker(true, statusesCount: statusesCount).Generate();
            var originalStatuses = repairOrder.Statuses.Select(status =>
                new RepairOrderStatusToWrite
                {
                    Id = status.Id,
                    Status = status.Type,
                    Description = status.Description,
                    Date = status.Date
                }).ToList();
            var editedStatuses = repairOrder.Statuses.Select(status =>
                new RepairOrderStatusToWrite
                {
                    Id = status.Id,
                    Status = status.Type,
                    Description = updatedDescription,
                    Date = status.Date
                }).ToList();
            repairOrder.Statuses.Should().NotBeEquivalentTo(editedStatuses);
            var updatedStatuses = RepairOrderTestHelper.CreateRepairOrderStatuses(editedStatuses);
            var repairOrderNumbers = new List<long>();

            var result = repairOrder.UpdateStatuses(updatedStatuses, repairOrderNumbers);

            result.IsSuccess.Should().BeTrue();
            repairOrder.Statuses.Count.Should().Be(statusesCount);
            foreach (var status in repairOrder.Statuses)
            {
                var item = editedStatuses.FirstOrDefault(item => item.Id == status.Id);
                item.Status.Should().Be(status.Type);
                item.Description.Should().Be(status.Description);
                item.Date.Should().Be(status.Date);
            }
            repairOrder.Statuses.Should().NotBeEquivalentTo(originalStatuses);
        }

        [Fact]
        public void Delete_Deleted_Statuses_On_UpdateStatuses()
        {
            var statusesCount = 3;
            var repairOrder = new RepairOrderFaker(true, statusesCount: statusesCount).Generate();
            var originalStatuses = repairOrder.Statuses.Select(status =>
                new RepairOrderStatusToWrite
                {
                    Id = status.Id,
                    Status = status.Type,
                    Description = status.Description,
                    Date = status.Date
                }).ToList();
            var repairOrderNumbers = new List<long>();
            var deletedStatuses = repairOrder.Statuses.ToList();
            deletedStatuses.RemoveAt(0);
            repairOrder.Statuses.Should().NotBeEquivalentTo(deletedStatuses);
            var updatedStatuses = deletedStatuses;

            var result = repairOrder.UpdateStatuses(updatedStatuses, repairOrderNumbers);

            result.IsSuccess.Should().BeTrue();
            repairOrder.Statuses.Count.Should().Be(statusesCount - 1);
            repairOrder.Statuses.Should().NotBeEquivalentTo(originalStatuses);
        }

        [Fact]
        public void Add_Added_Services_On_UpdateServices()
        {
            var servicesCount = 3;
            var repairOrder = new RepairOrderFaker(true, servicesCount: servicesCount).Generate();
            var addedServices = new RepairOrderServiceFaker(generateId: false).Generate(servicesCount);
            var updatedServices = repairOrder.Services.Concat(addedServices).ToList();
            repairOrder.Services.Should().NotBeEquivalentTo(addedServices);

            var result = repairOrder.UpdateServices(updatedServices);

            result.IsSuccess.Should().BeTrue();
            repairOrder.Services.Should().NotBeEquivalentTo(addedServices);
            repairOrder.Services.Count.Should().Be(servicesCount + servicesCount);
        }

        [Fact]
        public void Update_Edited_Services_On_UpdateServices()
        {
            var updatedServiceName = "a ServiceName updated by test";
            var servicesCount = 3;
            var repairOrder = new RepairOrderFaker(true, servicesCount: servicesCount).Generate();
            var originalServices = repairOrder.Services.Select(service =>
                new RepairOrderServiceToWrite
                {
                    Id = service.Id,
                    ServiceName = service.ServiceName,
                    SaleCode = SaleCodeHelper.ConvertToReadDto(service.SaleCode),
                    IsCounterSale = service.IsCounterSale,
                    IsDeclined = service.IsDeclined,
                    PartsTotal = service.PartsTotal,
                    LaborTotal = service.LaborTotal,
                    DiscountTotal = service.DiscountTotal
                }).ToList();
            var editedServices = repairOrder.Services.Select(service =>
                new RepairOrderServiceToWrite
                {
                    Id = service.Id,
                    ServiceName = updatedServiceName,
                    SaleCode = SaleCodeHelper.ConvertToReadDto(service.SaleCode),
                    IsCounterSale = service.IsCounterSale,
                    IsDeclined = service.IsDeclined,
                    PartsTotal = service.PartsTotal,
                    LaborTotal = service.LaborTotal,
                    DiscountTotal = service.DiscountTotal
                }).ToList();
            repairOrder.Services.Should().NotBeEquivalentTo(editedServices);
            var updatedServices = RepairOrderTestHelper.CreateRepairOrderServices(editedServices);

            var result = repairOrder.UpdateServices(updatedServices);

            result.IsSuccess.Should().BeTrue();
            repairOrder.Services.Count.Should().Be(servicesCount);
            foreach (var service in repairOrder.Services)
            {
                var editedService = editedServices.FirstOrDefault(item => item.Id == service.Id);
                editedService.ServiceName.Should().Be(service.ServiceName);
                editedService.SaleCode.Id.Should().Be(service.SaleCode.Id);
                editedService.SaleCode.Code.Should().Be(service.SaleCode.Code);
                editedService.SaleCode.DesiredMargin.Should().Be(service.SaleCode.DesiredMargin);
                editedService.SaleCode.LaborRate.Should().Be(service.SaleCode.LaborRate);
                editedService.SaleCode.Name.Should().Be(service.SaleCode.Name);
            }
            repairOrder.Services.Should().NotBeEquivalentTo(originalServices);
        }

        [Fact]
        public void Delete_Deleted_Services_On_UpdateServices()
        {
            var servicesCount = 3;
            var repairOrder = new RepairOrderFaker(true, servicesCount: servicesCount).Generate();
            var originalServices = repairOrder.Services.Select(status =>
                new RepairOrderServiceToWrite
                {
                    Id = status.Id,
                    DiscountTotal = status.DiscountTotal,
                    IsCounterSale = status.IsCounterSale,
                    IsDeclined = status.IsDeclined,
                    LaborTotal = status.LaborTotal,
                    PartsTotal = status.PartsTotal,
                    SaleCode = SaleCodeHelper.ConvertToReadDto(status.SaleCode),
                    ServiceName = status.ServiceName,
                    Total = status.Total
                }).ToList();
            var deletedServices = repairOrder.Services.ToList();
            deletedServices.RemoveAt(0);
            repairOrder.Services.Should().NotBeEquivalentTo(deletedServices);
            var updatedServices = deletedServices;

            var result = repairOrder.UpdateServices(updatedServices);

            result.IsSuccess.Should().BeTrue();
            repairOrder.Services.Count.Should().Be(servicesCount - 1);
            repairOrder.Services.Should().NotBeEquivalentTo(originalServices);
        }

        private static Result<RepairOrder> CreateRepairOrder(
            List<RepairOrderStatus> statuses = null)
        {
            var customer = new CustomerFaker(true);
            var vehicle = new VehicleFaker(true);

            return RepairOrder.Create(
                customer,
                vehicle,
                DateTime.Now,
                new List<long>(),
                1,
                statuses ?? new List<RepairOrderStatus>()
            );
        }

    }
}
