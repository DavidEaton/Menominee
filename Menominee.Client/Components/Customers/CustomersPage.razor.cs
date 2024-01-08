using CSharpFunctionalExtensions;
using FluentValidation;
using Menominee.Client.Services.Customers;
using Menominee.Domain.Enums;
using Menominee.Shared.Models.Customers;
using Menominee.Shared.Models.Persons;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.Customers
{
    public partial class CustomersPage : ComponentBase
    {
        [Inject]
        public ICustomerDataService? CustomerDataService { get; set; }

        [Inject]
        public ILogger<CustomersPage> Logger { get; set; }

        [Inject]
        private IValidator<CustomerToWrite>? CustomerValidator { get; set; }

        public IReadOnlyList<CustomerToReadInList>? Customers;
        public FormMode FormMode { get; set; } = FormMode.View;

        private CustomerToWrite Customer = new();
        private long Id { get; set; }
        private string Caption { get; set; } = string.Empty;
        private List<CustomerTypeEnumModel> CustomerTypeEnumData { get; set; } = new List<CustomerTypeEnumModel>();
        private List<EntityTypeEnumModel> EntityTypeEnumData { get; set; } = new List<EntityTypeEnumModel>();

        protected override async Task OnInitializedAsync()
        {
            await GetCustomers();

            foreach (CustomerType item in Enum.GetValues(typeof(CustomerType)))
            {
                CustomerTypeEnumData.Add(new CustomerTypeEnumModel { DisplayText = item.ToString(), Value = item });
            }

            foreach (EntityType item in Enum.GetValues(typeof(EntityType)))
            {
                EntityTypeEnumData.Add(new EntityTypeEnumModel { DisplayText = item.ToString(), Value = item });
            }

            // TODO: Remove invalid rows added for validation during development after feature is Done
            CustomerTypeEnumData.Add(new CustomerTypeEnumModel { DisplayText = "Invalid", Value = (CustomerType)999 });
            EntityTypeEnumData.Add(new EntityTypeEnumModel { DisplayText = "Invalid", Value = (EntityType)999 });
        }

        private async Task GetCustomers()
        {
            await CustomerDataService.GetAllAsync()
                .Match(
                    success => Customers = success,
                    failure => Logger.LogError(failure)
                );
        }

        private async Task EditAsync(GridRowClickEventArgs args)
        {
            Id = (args.Item as CustomerToReadInList).Id;
            FormMode = FormMode.Edit;
            Customers = null;

            var customerResult = await CustomerDataService.GetAsync(Id)
                .OnFailure(error =>
                {
                    Logger.LogError(error);
                    // TODO: Replace exception with toast message
                    throw new Exception(error);
                });

            Customer = CustomerHelper.ConvertReadToWriteDto(customerResult.Value);

            if (customerResult.Value.EntityType == EntityType.Person)
            {
                Caption = $"Editing Customer: {customerResult.Value.Person.Name}";
            }

            if (customerResult.Value.EntityType == EntityType.Business)
            {
                Caption = $"Editing Customer: {customerResult.Value.Business.Name}";
            }
        }

        private void Add()
        {
            Caption = "Adding new Customer";
            Customers = new List<CustomerToReadInList>();
            var name = new PersonNameToWrite();
            Customer = new()
            {
                CustomerType = CustomerType.Retail,
                EntityType = EntityType.Person,
                Person = new(),
                Business = new()
            };

            Customer.Person.Name = name;

            FormMode = FormMode.Add;
        }

        protected async Task Submit(CustomerToWrite customer)
        {
            if (FormMode == FormMode.Add)
            {
                await CustomerDataService.AddAsync(customer);
            }
            else if (FormMode == FormMode.Edit)
            {
                await CustomerDataService.UpdateAsync(customer);
            }

            await Close();
        }

        //protected async Task EndEditAsync()
        //{
        //    Caption = string.Empty;
        //    await GetCustomers();
        //    FormMode = FormMode.View;
        //}

        protected async Task Close()
        {
            FormMode = FormMode.View;
            Customer = new CustomerToWrite();
            await GetCustomers();
        }

        private class CustomerTypeEnumModel
        {
            public CustomerType Value { get; set; }
            public string DisplayText { get; set; }
        }

        private class EntityTypeEnumModel
        {
            public EntityType Value { get; set; }
            public string DisplayText { get; set; }
        }

    }
}
