using Bogus;
using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using FluentAssertions;
using System;
using TestingHelperLibrary.Fakers;
using Xunit;

namespace CustomerVehicleManagement.Tests.Entities
{
    public class RepairOrderServiceShould
    {
        [Fact]
        public void Create_RepairOrderService()
        {
            var faker = new Faker();
            var serviceName = faker.Company.CompanyName();
            var saleCode = new SaleCodeFaker(true).Generate();
            var shopSuppliesTotal = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);

            var result = RepairOrderService.Create(serviceName, saleCode, shopSuppliesTotal);

            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void SetServiceName()
        {
            var service = new RepairOrderServiceFaker(true).Generate();
            var newName = new Faker().Company.CompanyName();

            var result = service.SetServiceName(newName);

            result.IsSuccess.Should().BeTrue();
            service.ServiceName.Should().Be(newName);
        }

        [Fact]
        public void Return_Failure_On_Set_Invalid_ServiceName()
        {
            var service = new RepairOrderServiceFaker(true).Generate();
            var invalidName = new Faker().Random.String(RepairOrderService.MaximumLength + 1);

            var result = service.SetServiceName(invalidName);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void AddLineItem()
        {
            var service = new RepairOrderServiceFaker(true).Generate();
            var lineItem = new RepairOrderLineItemFaker(true).Generate();

            var result = service.AddLineItem(lineItem);

            result.IsSuccess.Should().BeTrue();
            service.LineItems.Should().Contain(lineItem);
        }

        [Fact]
        public void Return_Failure_On_Add_Null_LineItem()
        {
            var service = new RepairOrderServiceFaker(true).Generate();

            var result = service.AddLineItem(null);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void RemoveLineItem()
        {
            var service = new RepairOrderServiceFaker(true).Generate();
            var lineItem = new RepairOrderLineItemFaker(true).Generate();
            service.AddLineItem(lineItem);

            var result = service.RemoveLineItem(lineItem);

            result.IsSuccess.Should().BeTrue();
            service.LineItems.Should().NotContain(lineItem);
        }

        [Fact]
        public void Return_Failure_On_Remove_Null_LineItem()
        {
            var service = new RepairOrderServiceFaker(true).Generate();
            var lineItem = new RepairOrderLineItemFaker(true).Generate();
            service.AddLineItem(lineItem);

            var result = service.RemoveLineItem(null);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void AddTechnician()
        {
            var service = new RepairOrderServiceFaker(true).Generate();
            var technician = new RepairOrderServiceTechnicianFaker(true).Generate();

            var result = service.AddTechnician(technician);

            result.IsSuccess.Should().BeTrue();
            service.Technicians.Should().Contain(technician);
        }

        [Fact]
        public void Return_Failure_On_Add_Null_Technician()
        {
            var service = new RepairOrderServiceFaker(true).Generate();

            var result = service.AddTechnician(null);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void RemoveTechnician()
        {
            var service = new RepairOrderServiceFaker(true).Generate();
            var technician = new RepairOrderServiceTechnicianFaker(true).Generate();
            service.AddTechnician(technician);

            var result = service.RemoveTechnician(technician);

            result.IsSuccess.Should().BeTrue();
            service.Technicians.Should().NotContain(technician);
        }

        [Fact]
        public void Return_Failure_On_Remove_Null_Technician()
        {
            var service = new RepairOrderServiceFaker(true).Generate();
            var technician = new RepairOrderServiceTechnicianFaker(true).Generate();
            service.AddTechnician(technician);

            var result = service.RemoveTechnician(null);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void AddTax()
        {
            var service = new RepairOrderServiceFaker(true).Generate();
            var tax = new RepairOrderServiceTaxFaker(true).Generate();

            var result = service.AddTax(tax);

            result.IsSuccess.Should().BeTrue();
            service.Taxes.Should().Contain(tax);
        }

        [Fact]
        public void Return_Failure_On_Add_Null_Tax()
        {
            var service = new RepairOrderServiceFaker(true).Generate();

            var result = service.AddTax(null);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void RemoveTax()
        {
            var service = new RepairOrderServiceFaker(true).Generate();
            var tax = new RepairOrderServiceTaxFaker(true).Generate();
            service.AddTax(tax);

            var result = service.RemoveTax(tax);

            result.IsSuccess.Should().BeTrue();
            service.Taxes.Should().NotContain(tax);
        }

        [Fact]
        public void Return_Failure_On_Remove_Null_Tax()
        {
            var service = new RepairOrderServiceFaker(true).Generate();
            var tax = new RepairOrderServiceTaxFaker(true).Generate();
            service.AddTax(tax);

            var result = service.RemoveTax(null);

            result.IsFailure.Should().BeTrue();
        }
    }
}
