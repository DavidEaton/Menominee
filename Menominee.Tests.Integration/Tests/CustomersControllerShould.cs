using FluentAssertions;
using Menominee.Domain.Entities;
using Menominee.Domain.Enums;
using Menominee.Shared.Models.Contactable;
using Menominee.Shared.Models.Customers;
using Menominee.Shared.Models.Pagination;
using Menominee.Shared.Models.Vehicles;
using Menominee.TestingHelperLibrary.Fakers;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TestingHelperLibrary;
using TestingHelperLibrary.Fakers;
using Xunit;

namespace Menominee.Tests.Integration.Tests;

[Collection("Integration")]
public class CustomersControllerShould : IntegrationTestBase
{
    private const string Route = "customers";
    private const string Code = "code";

    public CustomersControllerShould(IntegrationTestWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Get_Invalid_Route_Returns_NotFound()
    {
        var response = await HttpClient.GetAsync($"{Route}-invalid");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Get_Invalid_Id_Returns_NotFound()
    {
        var response = await HttpClient.GetAsync($"{Route}/0");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Get_Valid_Id_Returns_Customer()
    {
        var customerToUpdate = await CreatePersonCustomerRequestAsync();

        var customerFromEndpoint = await HttpClient
            .GetFromJsonAsync<CustomerToRead>($"{Route}/{customerToUpdate.Id}");

        customerFromEndpoint.Should().BeOfType<CustomerToRead>();
    }

    [Fact]
    public async Task Add_a_Person_Customer()
    {
        var person = DbContext.Persons.First();
        var customer = Customer.Create(person, CustomerType.Retail, Code).Value;
        var request = CustomerHelper.ConvertToWriteDto(customer);
        var result = await PostCustomer(request);
        var id = JsonSerializerHelper.GetIdFromString(result);
        var customerFromEndpoint = await HttpClient
            .GetFromJsonAsync<CustomerToRead>($"{Route}/{id}");

        customerFromEndpoint.Should().BeOfType<CustomerToRead>();
        customerFromEndpoint.Person.Should()
            .BeEquivalentTo(CustomerHelper.ConvertToReadDto(customer).Person, options => options.Excluding(c => c.Id));
        customerFromEndpoint.Vehicles.Should()
            .BeEquivalentTo(CustomerHelper.ConvertToReadDto(customer).Vehicles, options => options.Excluding(v => v.Id));
    }

    [Fact]
    public async Task Add_a_new_Person_and_a_new_Customer()
    {
        var generateId = false;
        var includeAddress = true;
        var includeDriverseLicense = true;
        var collectionCount = 3;
        var person = new PersonFaker(
            generateId: generateId,
            includeAddress: includeAddress,
            includeDriversLicense: includeDriverseLicense,
            emailsCount: collectionCount,
            phonesCount: collectionCount).Generate();
        var customer = Customer.Create(person, CustomerType.Retail, Code).Value;

        var request = CustomerHelper.ConvertToWriteDto(customer);

        var result = await PostCustomer(request);
        var id = JsonSerializerHelper.GetIdFromString(result);
        var customerFromEndpoint = await HttpClient
            .GetFromJsonAsync<CustomerToRead>($"{Route}/{id}");

        customerFromEndpoint.Should().BeOfType<CustomerToRead>();
        customerFromEndpoint.Person.Should().NotBeNull();
        customerFromEndpoint.Person.Id.Should().BeGreaterThan(0);
        customerFromEndpoint.Person.Name.Should().BeEquivalentTo(request.Person.Name, options => options.ExcludingMissingMembers());
        customerFromEndpoint.Person.Birthday.Should().Be(request.Person.Birthday);
        customerFromEndpoint.Person.Address.Should().BeEquivalentTo(request.Person.Address, options => options.ExcludingMissingMembers());
        foreach (var email in request.Person.Emails)
            customerFromEndpoint.Person.Emails.Should().ContainEquivalentOf(email, options => options.Excluding(c => c.Id));
        foreach (var phone in request.Person.Phones)
            customerFromEndpoint.Person.Phones.Should().ContainEquivalentOf(phone, options => options.Excluding(c => c.Id));
        customerFromEndpoint.Person.DriversLicense.Should().BeEquivalentTo(request.Person.DriversLicense, options => options.ExcludingMissingMembers());
    }

    [Fact]
    public async Task Add_a_Business_Customer()
    {
        var business = DbContext.Businesses.First();
        var customer = Customer.Create(business, CustomerType.Retail, Code).Value;

        var result = await PostCustomer(CustomerHelper.ConvertToWriteDto(customer));
        var id = JsonSerializerHelper.GetIdFromString(result);
        var customerFromEndpoint = await HttpClient
            .GetFromJsonAsync<CustomerToRead>($"{Route}/{id}");

        customerFromEndpoint.Should().BeOfType<CustomerToRead>();
        customerFromEndpoint.Person.Should()
            .BeEquivalentTo(CustomerHelper.ConvertToReadDto(customer).Person, options => options.Excluding(c => c.Id));
        customerFromEndpoint.Vehicles.Should()
            .BeEquivalentTo(CustomerHelper.ConvertToReadDto(customer).Vehicles, options => options.Excluding(v => v.Id));
    }

    [Fact]
    public async Task Add_a_Phone()
    {
        var customerToUpdate = await CreateBusinessCustomerRequestAsync();
        var originalPhoneCount = customerToUpdate.Business.Phones.Count;
        var phoneToAdd = new PhoneFaker(false).Generate();
        customerToUpdate.Business.Phones.Add(PhoneHelper.ConvertToWriteDto(phoneToAdd));

        var response = await HttpClient
            .PutAsJsonAsync($"{Route}/{customerToUpdate.Id}", customerToUpdate);

        response.EnsureSuccessStatusCode();
        var customerFromEndpoint = await HttpClient
            .GetFromJsonAsync<CustomerToRead>($"{Route}/{customerToUpdate.Id}");
        customerFromEndpoint.Should().NotBeNull();
        customerFromEndpoint.Business.Phones
            .Should().ContainEquivalentOf(PhoneHelper.ConvertToReadDto(phoneToAdd), options => options.Excluding(p => p.Id));
        customerFromEndpoint.Business.Phones.Count.Should().Be(originalPhoneCount + 1);
    }


    [Fact]
    public async Task Add_an_Email()
    {
        var customerToUpdate = await CreateBusinessCustomerRequestAsync();
        var originalEmailCount = customerToUpdate.Business.Emails.Count;
        var emailToAdd = new EmailFaker(false).Generate();
        customerToUpdate.Business.Emails.Add(EmailHelper.ConvertToWriteDto(emailToAdd));

        var response = await HttpClient
            .PutAsJsonAsync($"{Route}/{customerToUpdate.Id}", customerToUpdate);

        response.EnsureSuccessStatusCode();
        var customerFromEndpoint = await HttpClient
            .GetFromJsonAsync<CustomerToRead>($"{Route}/{customerToUpdate.Id}");
        customerFromEndpoint.Should().NotBeNull();
        customerFromEndpoint.Business.Emails
            .Should().ContainEquivalentOf(EmailHelper.ConvertToReadDto(emailToAdd), options => options.Excluding(p => p.Id));
        customerFromEndpoint.Business.Emails.Count.Should().Be(originalEmailCount + 1);
    }

    [Fact]
    public async Task Add_a_Vehicle()
    {
        var customerToUpdate = await CreateBusinessCustomerRequestAsync();

        var originalVehicleCount = customerToUpdate.Vehicles.Count;
        var vehicleToAdd = new VehicleFaker(false).Generate();
        customerToUpdate.Vehicles.Add(VehicleHelper.ConvertToWriteDto(vehicleToAdd));
        var response = await HttpClient
            .PutAsJsonAsync($"{Route}/{customerToUpdate.Id}", customerToUpdate);

        response.EnsureSuccessStatusCode();
        var customerFromEndpoint = await HttpClient
            .GetFromJsonAsync<CustomerToRead>($"{Route}/{customerToUpdate.Id}");
        customerFromEndpoint.Should().NotBeNull();
        customerFromEndpoint.Vehicles.Count.Should().Be(originalVehicleCount + 1);
    }

    [Fact]
    public async Task Update_a_Business_Customer()
    {
        var customerToUpdate = await CreateBusinessCustomerRequestAsync();
        var updatedCode = "Updated code";
        customerToUpdate.Code = updatedCode;
        var updatedCustomerType = CustomerType.Fleet;
        customerToUpdate.CustomerType = updatedCustomerType;

        var response = await HttpClient.PutAsJsonAsync($"{Route}/{customerToUpdate.Id}", customerToUpdate);

        response.EnsureSuccessStatusCode();
        var customerFromEndpoint = await HttpClient
            .GetFromJsonAsync<CustomerToRead>($"{Route}/{customerToUpdate.Id}");
        customerFromEndpoint.Should().NotBeNull();
        customerFromEndpoint.Business?.Name.Should().Be(customerToUpdate.Business.Name.Name);
        customerFromEndpoint.Code.Should().Be(customerToUpdate.Code);
        customerFromEndpoint.CustomerType.Should().Be(updatedCustomerType);
    }

    [Fact]
    public async Task Update_a_Vehicle()
    {
        var customerToUpdate = await CreateBusinessCustomerRequestAsync();
        var vehicleToAdd = new VehicleFaker(false).Generate();
        customerToUpdate.Vehicles.Add(VehicleHelper.ConvertToWriteDto(vehicleToAdd));
        var response = await HttpClient
            .PutAsJsonAsync($"{Route}/{customerToUpdate.Id}", customerToUpdate);
        response.EnsureSuccessStatusCode();
        var customerFromEndpoint = await HttpClient
            .GetFromJsonAsync<CustomerToRead>($"{Route}/{customerToUpdate.Id}");
        var customerVehicle = customerFromEndpoint.Vehicles.FirstOrDefault();

        var originalVehicle = new VehicleToRead()
        {
            Id = customerVehicle.Id,
            Make = customerVehicle.Make,
            Model = customerVehicle.Model,
            VIN = customerVehicle.VIN,
            Year = customerVehicle.Year,
            Active = customerVehicle.Active,
            Color = customerVehicle.Color,
            NonTraditionalVehicle = customerVehicle.NonTraditionalVehicle,
            Plate = customerVehicle.Plate,
            UnitNumber = customerVehicle.UnitNumber,
            PlateStateProvince = customerVehicle.PlateStateProvince
        };

        var make = "MAKE";
        var model = "MODEL";
        var unitNumber = "UNIT";
        var year = 2000;
        var vin = "JH4DA9460LS000685";
        var color = "COLOR";
        var plate = "PLATE";
        var state = State.MI;
        var updatedVehicle = new VehicleToWrite()
        {
            Id = customerVehicle.Id,
            Make = make,
            Model = model,
            VIN = vin,
            Year = year,
            Active = !customerVehicle.Active,
            Color = color,
            NonTraditionalVehicle = !customerVehicle.NonTraditionalVehicle,
            Plate = plate,
            UnitNumber = unitNumber,
            PlateStateProvince = state
        };

        var updatedCustomer = CustomerHelper.ConvertReadToWriteDto(customerFromEndpoint);
        updatedCustomer.Vehicles[0] = updatedVehicle;

        response = await HttpClient.PutAsJsonAsync($"{Route}/{customerToUpdate.Id}", updatedCustomer);
        response.EnsureSuccessStatusCode();

        customerFromEndpoint = await HttpClient
            .GetFromJsonAsync<CustomerToRead>($"{Route}/{customerToUpdate.Id}");

        customerFromEndpoint.Vehicles[0].Should().NotBeEquivalentTo(originalVehicle);
        customerFromEndpoint.Vehicles[0].Should().BeEquivalentTo(updatedVehicle);

    }

    [Fact]
    public async Task Delete_a_Customer()
    {
        var customerToDelete = await CreateBusinessCustomerRequestAsync();
        customerToDelete.Should().NotBeNull();

        var response = await HttpClient.DeleteAsync($"{Route}/{customerToDelete.Id}");
        response.EnsureSuccessStatusCode();

        var deletedCustomerFromDatabase = DbContext.Customers
            .FirstOrDefault(e => e.Id == customerToDelete.Id);

        deletedCustomerFromDatabase.Should().BeNull();
    }

    [Fact]
    public async Task Delete_a_Phone()
    {
        var index = 0;
        var customerToUpdate = await CreateBusinessCustomerRequestAsync();
        var phonesCount = 2;
        var phonesToAdd = new PhoneFaker(false).Generate(phonesCount);
        customerToUpdate.Business.Phones.AddRange(PhoneHelper.ConvertToWriteDtos(phonesToAdd));
        customerToUpdate.Business.Phones.Count.Should().Be(phonesCount);
        var response = await HttpClient.PutAsJsonAsync($"{Route}/{customerToUpdate.Id}", customerToUpdate);
        response.EnsureSuccessStatusCode();
        var customerFromEndpoint = await HttpClient
            .GetFromJsonAsync<CustomerToRead>($"{Route}/{customerToUpdate.Id}");
        customerFromEndpoint.Business.Phones.Count.Should().Be(phonesCount);
        var phoneToDelete = customerFromEndpoint.Business.Phones[index];
        phoneToDelete.Should().NotBeNull();
        customerToUpdate = CustomerHelper.ConvertReadToWriteDto(customerFromEndpoint);
        customerToUpdate.Business.Phones.RemoveAt(index);

        response = await HttpClient.PutAsJsonAsync($"{Route}/{customerToUpdate.Id}", customerToUpdate);

        response.EnsureSuccessStatusCode();
        customerFromEndpoint = await HttpClient
            .GetFromJsonAsync<CustomerToRead>($"{Route}/{customerToUpdate.Id}");
        customerFromEndpoint.Should().NotBeNull();
        customerFromEndpoint.Business.Phones.Count.Should().Be(phonesCount - 1);
    }

    [Fact]
    public async Task Delete_an_Email()
    {
        var index = 0;
        var customerToUpdate = await CreateBusinessCustomerRequestAsync();
        var emailsCount = 2;
        var emailsToAdd = new EmailFaker(false).Generate(emailsCount);
        customerToUpdate.Business.Emails.AddRange(EmailHelper.ConvertToWriteDtos(emailsToAdd));
        customerToUpdate.Business.Emails.Count.Should().Be(emailsCount);
        var response = await HttpClient.PutAsJsonAsync($"{Route}/{customerToUpdate.Id}", customerToUpdate);
        response.EnsureSuccessStatusCode();
        var customerFromEndpoint = await HttpClient
            .GetFromJsonAsync<CustomerToRead>($"{Route}/{customerToUpdate.Id}");
        customerFromEndpoint.Business.Emails.Count.Should().Be(emailsCount);
        var emailToDelete = customerFromEndpoint.Business.Emails[index];
        emailToDelete.Should().NotBeNull();
        customerToUpdate = CustomerHelper.ConvertReadToWriteDto(customerFromEndpoint);
        customerToUpdate.Business.Emails.RemoveAt(index);

        response = await HttpClient.PutAsJsonAsync($"{Route}/{customerToUpdate.Id}", customerToUpdate);

        response.EnsureSuccessStatusCode();
        customerFromEndpoint = await HttpClient
            .GetFromJsonAsync<CustomerToRead>($"{Route}/{customerToUpdate.Id}");
        customerFromEndpoint.Should().NotBeNull();
        customerFromEndpoint.Business.Emails.Count.Should().Be(emailsCount - 1);
    }

    [Fact]
    public async Task Delete_a_Vehicle()
    {
        var customerToUpdate = await CreateBusinessCustomerRequestAsync();
        var originalVehicleCount = 2;
        var vehiclesToAdd = new VehicleFaker(false).Generate(originalVehicleCount);
        customerToUpdate.Vehicles.AddRange(VehicleHelper.ConvertToWriteDtos(vehiclesToAdd));
        var response = await HttpClient
            .PutAsJsonAsync($"{Route}/{customerToUpdate.Id}", customerToUpdate);
        response.EnsureSuccessStatusCode();
        var customerFromEndpoint = await HttpClient
            .GetFromJsonAsync<CustomerToRead>($"{Route}/{customerToUpdate.Id}");
        customerFromEndpoint.Should().NotBeNull();
        customerFromEndpoint.Vehicles.Count.Should().Be(originalVehicleCount);
        var deletedVehicleId = customerFromEndpoint.Vehicles[0].Id;
        customerFromEndpoint.Vehicles.Should().Contain(vehicle => vehicle.Id == deletedVehicleId);
        customerFromEndpoint.Vehicles.RemoveAt(0);
        var updatedVehicleCount = customerFromEndpoint.Vehicles.Count;
        customerToUpdate = CustomerHelper.ConvertReadToWriteDto(customerFromEndpoint);

        response = await HttpClient.PutAsJsonAsync($"{Route}/{customerToUpdate.Id}", customerToUpdate);

        response.EnsureSuccessStatusCode();
        customerFromEndpoint = await HttpClient
            .GetFromJsonAsync<CustomerToRead>($"{Route}/{customerToUpdate.Id}");
        customerFromEndpoint.Should().NotBeNull();
        customerFromEndpoint.Vehicles.Count.Should().Be(updatedVehicleCount);
        customerFromEndpoint.Vehicles.Should().NotContain(vehicle => vehicle.Id == deletedVehicleId);
    }

    [Fact]
    public async Task GetCustomersAsync_With_Code()
    {
        var customerToUpdate = await CreatePersonCustomerRequestAsync();
        var pageSize = 10;
        var pageNumber = 1;
        var pagination = new Pagination
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };
        var url = $"/api/customers/{Code}?PageNumber={pagination.PageNumber}&PageSize={pagination.PageSize}";

        var customerFromEndpoint = await HttpClient
            .GetFromJsonAsync<PagedList<CustomerToRead>>(url);

        var customer = customerFromEndpoint.Items[0];
        var pagedList = new PagedList<CustomerToRead>();
        pagedList.Page = customerFromEndpoint.Page;
        pagedList.PageSize = customerFromEndpoint.PageSize;
        pagedList.TotalPages = customerFromEndpoint.TotalPages;
        pagedList.Should().NotBeNull();
        customerFromEndpoint.PageSize.Should().Be(pageSize);
        customerFromEndpoint.TotalPages.Should().Be(pageNumber);
        customerToUpdate.Id.Should().Be(customer.Id);
        customerToUpdate.IsBusiness.Should().BeFalse();
        customerToUpdate.IsPerson.Should().BeTrue();
        customerToUpdate.Code.Should().Be(customer.Code);
        customerToUpdate.CustomerType.Should().Be(customer.CustomerType);
        customerToUpdate.EntityType.Should().Be(customer.EntityType);
        customerToUpdate.Person.Id.Should().Be(customer.Person.Id);
        customerToUpdate.Person.Address.AddressLine1.Should().Be(customer.Person.Address.AddressLine1);
        customerToUpdate.Person.Address.AddressLine2.Should().Be(customer.Person.Address.AddressLine2);
        customerToUpdate.Person.Address.City.Should().Be(customer.Person.Address.City);
        customerToUpdate.Person.Address.PostalCode.Should().Be(customer.Person.Address.PostalCode);
        customerToUpdate.Person.Address.State.Should().Be(customer.Person.Address.State);
        customerToUpdate.Person.Birthday.Should().Be(customer.Person.Birthday);
    }

    private async Task<CustomerToWrite> CreateBusinessCustomerRequestAsync()
    {
        var business = DbContext.Businesses.First();
        var customer = Customer.Create(business, CustomerType.Retail, Code).Value;
        var request = CustomerHelper.ConvertToWriteDto(customer);
        var result = await PostCustomer(request);
        var id = JsonSerializerHelper.GetIdFromString(result);
        var customerFromEndpoint = await HttpClient
            .GetFromJsonAsync<CustomerToRead>($"{Route}/{id}");

        return CustomerHelper.ConvertReadToWriteDto(customerFromEndpoint);
    }

    private async Task<CustomerToWrite> CreatePersonCustomerRequestAsync()
    {
        var person = DbContext.Persons.First();
        var customer = Customer.Create(person, CustomerType.Retail, Code).Value;
        var request = CustomerHelper.ConvertToWriteDto(customer);
        var result = await PostCustomer(request);
        var id = JsonSerializerHelper.GetIdFromString(result);
        var customerFromEndpoint = await HttpClient
            .GetFromJsonAsync<CustomerToRead>($"{Route}/{id}");

        return CustomerHelper.ConvertReadToWriteDto(customerFromEndpoint);
    }

    private async Task<string> PostCustomer(CustomerToWrite customer)
    {
        var json = JsonSerializer.Serialize(customer, JsonSerializerHelper.DefaultSerializerOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await HttpClient.PostAsync(Route, content);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        var errorContent = await response.Content.ReadAsStringAsync();

        var (success, apiError) = JsonSerializerHelper.DeserializeApiError(errorContent);

        return success
            ? $"Error: {response.StatusCode} - {response.ReasonPhrase}. Message: {apiError.Message}"
            : throw new JsonException("Failed to deserialize ApiError");
    }

    public override void SeedData()
    {
        var count = 3;
        var persons = new PersonFaker(generateId: false, includeAddress: true)
            .Generate(count);
        var businesses = new BusinessFaker(generateId: false, includeAddress: true)
            .Generate(count);

        DataSeeder.Save(persons);
        DataSeeder.Save(businesses);
    }
}
