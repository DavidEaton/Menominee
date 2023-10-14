using FluentAssertions;
using Menominee.Common.Enums;
using Menominee.Domain.Entities;
using Menominee.Shared.Models.Contactable;
using Menominee.Shared.Models.Customers;
using Menominee.Shared.Models.Pagination;
using Menominee.Shared.Models.Vehicles;
using Menominee.TestingHelperLibrary.Fakers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
        var customerFromDatabase = DbContext.Customers.First();

        var customerFromEndpoint = await HttpClient.GetFromJsonAsync<CustomerToRead>($"{Route}/{customerFromDatabase.Id}");

        customerFromEndpoint.Should().BeOfType<CustomerToRead>();
    }

    [Fact]
    public async Task Add_a_Person_Customer()
    {
        var person = DbContext.Persons.First();
        var customer = Customer.Create(person, CustomerType.Retail, Code).Value;

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
        customerFromEndpoint.Person.Gender.Should().Be(request.Person.Gender);
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

        var request = CustomerHelper.ConvertToWriteDto(customer);

        var result = await PostCustomer(request);
        var id = JsonSerializerHelper.GetIdFromString(result);
        var customerFromEndpoint = await HttpClient
            .GetFromJsonAsync<CustomerToRead>($"{Route}/{id}");

        customerFromEndpoint.Should().BeOfType<CustomerToRead>();
        customerFromEndpoint.Business.Should().NotBeNull();
        customerFromEndpoint.Business.Id.Should().BeGreaterThan(0);
        customerFromEndpoint.Business.Name.Should().BeEquivalentTo(request.Business.Name);
        customerFromEndpoint.Business.Address.Should().BeEquivalentTo(request.Business.Address, options => options.ExcludingMissingMembers());
        foreach (var email in request.Business.Emails)
            customerFromEndpoint.Business.Emails.Should().ContainEquivalentOf(email, options => options.Excluding(c => c.Id));
        foreach (var phone in request.Business.Phones)
            customerFromEndpoint.Business.Phones.Should().ContainEquivalentOf(phone, options => options.Excluding(c => c.Id));
        customerFromEndpoint.Business.Should()
            .BeEquivalentTo(CustomerHelper.ConvertToReadDto(customer).Business, options => options.Excluding(c => c.Id));
        customerFromEndpoint.Vehicles.Should()
            .BeEquivalentTo(CustomerHelper.ConvertToReadDto(customer).Vehicles, options => options.Excluding(v => v.Id));
    }

    [Fact]
    public async Task Add_a_new_Business_and_a_new_Customer()
    {
        var generateId = false;
        var includeAddress = true;
        var collectionCount = 3;
        var business = new BusinessFaker(
            generateId: generateId,
            includeAddress: includeAddress,
            emailsCount: collectionCount,
            phonesCount: collectionCount).Generate();
        var customer = Customer.Create(business, CustomerType.Retail, Code).Value;

        var request = CustomerHelper.ConvertToWriteDto(customer);

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
        var customerToUpdate = DbContext.Customers.First();
        var originalPhoneCount = customerToUpdate.Phones.Count;
        var phoneToAdd = new PhoneFaker(false).Generate();
        var updatedCustomer = CustomerHelper.ConvertToWriteDto(customerToUpdate);
        updatedCustomer.Person?.Phones
                .Add(PhoneHelper.ConvertToWriteDto(phoneToAdd));
        updatedCustomer.Business?.Phones
                .Add(PhoneHelper.ConvertToWriteDto(phoneToAdd));

        var response = await HttpClient
            .PutAsJsonAsync($"{Route}/{customerToUpdate.Id}", updatedCustomer);
        response.EnsureSuccessStatusCode();
        var customerFromEndpoint = await HttpClient
            .GetFromJsonAsync<CustomerToRead>($"{Route}/{customerToUpdate.Id}");

        customerFromEndpoint.Should().NotBeNull();
        customerFromEndpoint.Phones
            .Should().ContainEquivalentOf(PhoneHelper.ConvertToReadDto(phoneToAdd), options => options.Excluding(p => p.Id));
        customerFromEndpoint.Phones.Count.Should().Be(originalPhoneCount + 1);
    }

    [Fact]
    public async Task Add_an_Email()
    {
        var customerToUpdate = DbContext.Customers.First();
        var originalEmailCount = customerToUpdate.Emails.Count;
        var emailToAdd = new EmailFaker(false).Generate();

        var updatedCustomer = CustomerHelper.ConvertToWriteDto(customerToUpdate);
        updatedCustomer.Person?.Emails
                .Add(EmailHelper.ConvertToWriteDto(emailToAdd));
        updatedCustomer.Business?.Emails
                .Add(EmailHelper.ConvertToWriteDto(emailToAdd));

        var response = await HttpClient
            .PutAsJsonAsync($"{Route}/{customerToUpdate.Id}", updatedCustomer);
        response.EnsureSuccessStatusCode();
        var customerFromEndpoint = await HttpClient
            .GetFromJsonAsync<CustomerToRead>($"{Route}/{customerToUpdate.Id}");

        customerFromEndpoint.Should().NotBeNull();
        customerFromEndpoint.Emails
            .Should().ContainEquivalentOf(EmailHelper.ConvertToReadDto(emailToAdd), options => options.Excluding(e => e.Id));
        customerFromEndpoint.Emails.Count.Should().Be(originalEmailCount + 1);
    }

    [Fact]
    public async Task Add_a_Vehicle()
    {
        var customerToUpdate = DbContext.Customers.First();
        var originalVehicleCount = customerToUpdate.Vehicles.Count;
        var vehicleToAdd = new VehicleFaker(false).Generate();
        var updatedCustomer = CustomerHelper.ConvertToWriteDto(customerToUpdate);
        updatedCustomer.Vehicles
                .Add(VehicleHelper.ConvertToWriteDto(vehicleToAdd));

        var response = await HttpClient
            .PutAsJsonAsync($"{Route}/{customerToUpdate.Id}", updatedCustomer);
        response.EnsureSuccessStatusCode();
        var customerFromEndpoint = await HttpClient
            .GetFromJsonAsync<CustomerToRead>($"{Route}/{customerToUpdate.Id}");

        customerFromEndpoint.Should().NotBeNull();
        customerFromEndpoint.Vehicles.Count.Should().Be(originalVehicleCount + 1);
    }

    [Fact]
    public async Task Update_a_Business_Customer()
    {
        // TODO: Test more updates customerFromEndpoint properties
        var business = DbContext.Businesses.First();
        var customerType = CustomerType.Retail;
        var customer = Customer.Create(business, customerType, Code).Value;

        var result = await PostCustomer(CustomerHelper.ConvertToWriteDto(customer));
        var id = JsonSerializerHelper.GetIdFromString(result);
        var customerFromEndpoint = await HttpClient
            .GetFromJsonAsync<CustomerToRead>($"{Route}/{id}");

        var updatedCustomer = CustomerHelper.ConvertToWriteDto(customerFromEndpoint);
        var updatedCode = "Updated code";
        updatedCustomer.Code = updatedCode;
        var updatedCustomerType = CustomerType.Fleet;
        updatedCustomer.CustomerType = updatedCustomerType;

        var response = await HttpClient.PutAsJsonAsync($"{Route}/{updatedCustomer.Id}", updatedCustomer);
        response.EnsureSuccessStatusCode();

        customerFromEndpoint = await HttpClient
            .GetFromJsonAsync<CustomerToRead>($"{Route}/{updatedCustomer.Id}");

        customerFromEndpoint.Should().NotBeNull();
        customerFromEndpoint.Business?.Name.Should().Be(updatedCustomer.Business.Name);
        //customerFromEndpoint.Code.Should().Be(updatedCustomer.Code);
        //customerFromEndpoint.CustomerType.Should().Be(updatedCustomerType);
    }

    [Fact]
    public async Task Update_Customer_Vehicles()
    {
        var customerToUpdate = DbContext.Customers.First();
        var originalVehiclesIdYear = customerToUpdate.Vehicles
            .ToDictionary(vehicle => vehicle.Id, vehicle => vehicle.Year);
        var updatedCustomer = CustomerHelper.ConvertToWriteDto(customerToUpdate);
        updatedCustomer.Vehicles = updatedCustomer.Vehicles
            .Select(vehicle =>
            {
                return new VehicleToWrite()
                {
                    Id = vehicle.Id,
                    VIN = vehicle.VIN,
                    Year = vehicle.Year - 1,
                    Make = vehicle.Make,
                    Model = vehicle.Model,
                    PlateStateProvince = vehicle.PlateStateProvince,
                    Active = vehicle.Active,
                    Color = vehicle.Color,
                    NonTraditionalVehicle = vehicle.NonTraditionalVehicle,
                    Plate = vehicle.Plate,
                    UnitNumber = vehicle.UnitNumber
                };
            })
            .ToList();
        var updatedVehiclesIdYear = updatedCustomer.Vehicles
            .ToDictionary(vehicle => vehicle.Id, vehicle => vehicle.Year);

        var response = await HttpClient.PutAsJsonAsync($"{Route}/{customerToUpdate.Id}", updatedCustomer);
        response.EnsureSuccessStatusCode();

        var customerFromEndpoint = await HttpClient
            .GetFromJsonAsync<CustomerToRead>($"{Route}/{customerToUpdate.Id}");

        var endpointVehiclesIdYear = customerFromEndpoint.Vehicles
            .ToDictionary(vehicle => vehicle.Id, vehicle => vehicle.Year);

        foreach (var originalEntry in originalVehiclesIdYear)
        {
            endpointVehiclesIdYear.Should().ContainKey(originalEntry.Key);
            endpointVehiclesIdYear[originalEntry.Key].Should().Be(originalEntry.Value - 1);
        }
    }

    [Fact]
    public async Task Delete_a_Customer()
    {
        var customerToDelete = await DbContext.Customers.FirstAsync();

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
        var phonesCount = 2;
        var phonesToAdd = new PhoneFaker(false).Generate(phonesCount);
        var person = DbContext.Persons.First();
        var customer = Customer.Create(person, CustomerType.Retail, Code).Value;
        DbContext.Customers.Add(customer);
        DbContext.SaveChanges();
        foreach (var phone in phonesToAdd)
            customer.AddPhone(phone);
        DbContext.SaveChanges();
        customer.Phones.Count.Should().Be(phonesCount);
        var updatedCustomer = CustomerHelper.ConvertToWriteDto(customer);
        updatedCustomer.Person.Phones.Count.Should().Be(phonesCount);
        var phoneToDelete = updatedCustomer.Person.Phones[index];
        phoneToDelete.Should().NotBeNull();
        // Delete (remove) the phone fro the collection
        updatedCustomer.Person.Phones.Remove(phoneToDelete);
        // Put to controller/Save to database
        var response = await HttpClient.PutAsJsonAsync($"{Route}/{customer.Id}", updatedCustomer);
        response.EnsureSuccessStatusCode();
        // Get updated customer from controller
        var customerFromEndpoint = await HttpClient
            .GetFromJsonAsync<CustomerToRead>($"{Route}/{customer.Id}");
        // Assert that one phone has been removed from the customer in the databas
        customerFromEndpoint.Should().NotBeNull();
        customerFromEndpoint.Phones.Count.Should().Be(phonesCount - 1);
    }

    [Fact]
    public async Task Delete_an_Email()
    {
        var index = 0;
        var emailsCount = 2;
        var emailsToAdd = new EmailFaker(false).Generate(emailsCount);
        var person = DbContext.Persons.First();
        var customer = Customer.Create(person, CustomerType.Retail, Code).Value;
        DbContext.Customers.Add(customer);
        DbContext.SaveChanges();
        foreach (var email in emailsToAdd)
            customer.AddEmail(email);
        DbContext.SaveChanges();
        customer.Emails.Count.Should().Be(emailsCount);
        var updatedCustomer = CustomerHelper.ConvertToWriteDto(customer);
        updatedCustomer.Person.Emails.Count.Should().Be(emailsCount);
        var emailToDelete = updatedCustomer.Person.Emails[index];
        emailToDelete.Should().NotBeNull();
        // Delete (remove) the email fro the collection
        updatedCustomer.Person.Emails.Remove(emailToDelete);
        // Put to controller/Save to database
        var response = await HttpClient.PutAsJsonAsync($"{Route}/{customer.Id}", updatedCustomer);
        response.EnsureSuccessStatusCode();
        // Get updated customer from controller
        var customerFromEndpoint = await HttpClient
            .GetFromJsonAsync<CustomerToRead>($"{Route}/{customer.Id}");
        // Assert that one email has been removed from the customer in the databas
        customerFromEndpoint.Should().NotBeNull();
        customerFromEndpoint.Emails.Count.Should().Be(emailsCount - 1);
    }

    [Fact]
    public async Task Delete_a_Vehicle()
    {
        var customerToUpdate = DbContext.Customers.First();
        var index = 0;

        // Add another vehicle
        var vehicleToAdd = new VehicleFaker(false).Generate();
        var updatedCustomer = CustomerHelper.ConvertToWriteDto(customerToUpdate);
        updatedCustomer.Vehicles
                .Add(VehicleHelper.ConvertToWriteDto(vehicleToAdd));
        var originalVehicleCount = updatedCustomer.Vehicles.Count;
        var vehicleToDelete = customerToUpdate.Vehicles[index];
        var deletedVehicleId = vehicleToDelete.Id;
        updatedCustomer.Vehicles.RemoveAt(index);
        var updatedVehicleCount = updatedCustomer.Vehicles.Count;

        var response = await HttpClient.PutAsJsonAsync($"{Route}/{customerToUpdate.Id}", updatedCustomer);
        response.EnsureSuccessStatusCode();

        var customerFromEndpoint = await HttpClient
            .GetFromJsonAsync<CustomerToRead>($"{Route}/{customerToUpdate.Id}");

        originalVehicleCount.Should().Be(updatedVehicleCount + 1);
        customerFromEndpoint.Vehicles.Count.Should().Be(updatedVehicleCount);
        customerFromEndpoint.Should().NotBeNull();
        customerFromEndpoint.Vehicles.Should().NotContainEquivalentOf(VehicleHelper.ConvertToReadDto(vehicleToDelete));
        customerFromEndpoint.Vehicles.Should().NotContain(vehicle => vehicle.Id == deletedVehicleId);
    }

    [Fact]
    public async Task GetCustomersAsync_With_Code()
    {
        var pagination = new Pagination
        {
            PageNumber = 1,
            PageSize = 10
        };

        var url = $"/api/customers/{Code}?PageNumber={pagination.PageNumber}&PageSize={pagination.PageSize}";
        var response = await HttpClient.GetAsync(url);
        response.IsSuccessStatusCode.Should().BeTrue();
        var raw = await response.Content.ReadAsStringAsync();
        Console.WriteLine(raw);
        raw.Should().NotBeNullOrEmpty();

        var responseModel = JsonSerializer.Deserialize<PagedList<CustomerToRead>>(raw);
        var customerList = responseModel.Items;

        var pagedList = new PagedList<CustomerToRead>();
        pagedList.Page = responseModel.Page;
        pagedList.PageSize = responseModel.PageSize;
        pagedList.TotalPages = responseModel.TotalPages;

        var customerCount = customerList.Count();

        pagedList.Should().NotBeNull();
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

        var customers = new List<Customer>();

        for (var i = 0; i < count; i++)
        {
            var customerType = (i % 2 == 0) ? CustomerType.Retail : CustomerType.Business;

            if (customerType == CustomerType.Business)
            {
                var customer = Customer.Create(
                    businesses[i],
                    customerType,
                    Code)
                    .Value;

                customers.Add(customer);
            }

            if (customerType == CustomerType.Retail)
            {
                var customer = Customer.Create(
                    persons[i],
                    customerType,
                    Code)
                    .Value;

                customers.Add(customer);
            }
        }

        DataSeeder.Save(customers);

        var vehicles = new VehicleFaker(false).Generate(count);
        DataSeeder.Save(vehicles);

        for (var i = 0; i < count; i++)
        {
            customers[i].AddVehicle(vehicles[i]);
        }

        DataSeeder.Save(customers);
    }
}
