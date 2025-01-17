﻿using Menominee.Domain.Entities;
using FluentAssertions;
using TestingHelperLibrary.Fakers;
using Xunit;

namespace Menominee.Tests.Entities
{
    public class CompanyShould
    {
        [Fact]
        public void Create_Company()
        {
            var business = new BusinessFaker(true).Generate();
            var result = Company.Create(business, 999);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeOfType<Company>();
            result.Value.NextInvoiceNumberOrSeed.Should().Be(999);
        }

        [Fact]
        public void Return_Failure_On_Create_Company_With_Null_Business()
        {
            var result = Company.Create(null, 1000);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Company.RequiredMessage);
        }

        [Fact]
        public void Return_Failure_On_Create_Company_With_Invalid_Seed()
        {
            var business = new BusinessFaker(true).Generate();
            var result = Company.Create(business, Company.MinimumValue - 1);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Company.MinimumValueMessage);
        }

        [Fact]
        public void SetInvoiceNumberSeed()
        {
            var business = new BusinessFaker(true).Generate();
            var company = Company.Create(business, 1000).Value;

            var result = company.SetInvoiceNumberSeed(2000);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(2000);
            company.NextInvoiceNumberOrSeed.Should().Be(2000);
        }

        [Fact]
        public void Return_Failure_On_SetInvoiceNumberSeed_Invalid_Seed()
        {
            var business = new BusinessFaker(true).Generate();
            var company = Company.Create(business, 1000).Value;

            var result = company.SetInvoiceNumberSeed(Company.MinimumValue - 1);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Company.MinimumValueMessage);
        }
    }
}
