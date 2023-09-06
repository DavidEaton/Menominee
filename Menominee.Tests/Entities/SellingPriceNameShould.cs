using Bogus;
using FluentAssertions;
using Menominee.Domain.Entities.Inventory;
using Menominee.TestingHelperLibrary.Fakers;
using Xunit;

namespace Menominee.Tests.Entities;

public class SellingPriceNameShould
{
    [Fact]
    public void Create_SellingPriceName()
    {
        var faker = new Faker();
        var name = faker.Random.String2(10);

        var result = SellingPriceName.Create(name);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeOfType<SellingPriceName>();
    }

    [Fact]
    public void Not_Create_SellingPriceName_With_Invalid_Name()
    {
        var invalidName = Utilities.RandomCharacters(SellingPriceName.MaximumNameLength + 1);

        var result = SellingPriceName.Create(invalidName);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(SellingPriceName.InvalidLengthMessage);
    }

    [Fact]
    public void Not_Create_SellingPriceName_With_Null_Name()
    {
        var result = SellingPriceName.Create(null);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(SellingPriceName.RequiredMessage);
    }

    [Fact]
    public void Not_Create_SellingPriceName_With_Empty_Name()
    {
        var result = SellingPriceName.Create(string.Empty);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(SellingPriceName.RequiredMessage);
    }

    [Fact]
    public void Set_Name()
    {
        var faker = new Faker();
        var sellingPriceName = new SellingPriceNameFaker(false).Generate();
        var name = faker.Random.String2(10);

        sellingPriceName.SetName(name);

        sellingPriceName.Name.Should().Be(name);
    }

    [Fact]
    public void Not_Set_Name_With_Invalid_Name()
    {
        var sellingPriceName = new SellingPriceNameFaker(false).Generate();
        var invalidName = Utilities.RandomCharacters(SellingPriceName.MaximumNameLength + 1);

        var result = sellingPriceName.SetName(invalidName);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(SellingPriceName.InvalidLengthMessage);
    }

    [Fact]
    public void Not_Set_Name_With_Null_Name()
    {
        var sellingPriceName = new SellingPriceNameFaker(false).Generate();

        var result = sellingPriceName.SetName(null);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(SellingPriceName.RequiredMessage);
    }

    [Fact]
    public void Not_Set_Name_With_Empty_Name()
    {
        var sellingPriceName = new SellingPriceNameFaker(false).Generate();

        var result = sellingPriceName.SetName(string.Empty);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(SellingPriceName.RequiredMessage);
    }
}
