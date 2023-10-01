using CSharpFunctionalExtensions;
using Menominee.Client.Services.Customers;
using Menominee.Common.Enums;
using Menominee.Shared.Models.Customers;
using Menominee.Shared.Models.Persons;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.Customers
{
    public partial class CustomersPage : ComponentBase
    {
        [Inject]
        public ICustomerDataService CustomerDataService { get; set; }

        [Inject]
        public ILogger<CustomersPage> Logger { get; set; }

        public IReadOnlyList<CustomerToReadInList> Customers;
        public FormMode FormMode { get; set; } = FormMode.View;
        private CustomerToWrite Customer { get; set; }
        private long Id { get; set; }
        private string Caption { get; set; } = string.Empty;
        List<CustomerTypeEnumModel> CustomerTypeEnumData { get; set; } = new List<CustomerTypeEnumModel>();
        List<EntityTypeEnumModel> EntityTypeEnumData { get; set; } = new List<EntityTypeEnumModel>();

        protected override async Task OnInitializedAsync()
        {
            await GetCustomers();

            foreach (CustomerType item in Enum.GetValues(typeof(CustomerType)))
                CustomerTypeEnumData.Add(new CustomerTypeEnumModel { DisplayText = item.ToString(), Value = item });

            foreach (EntityType item in Enum.GetValues(typeof(EntityType)))
                EntityTypeEnumData.Add(new EntityTypeEnumModel { DisplayText = item.ToString(), Value = item });
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
                Caption = $"Editing Customer: {customerResult.Value.Person.FirstMiddleLast}";

            if (customerResult.Value.EntityType == EntityType.Business)
                Caption = $"Editing Customer: {customerResult.Value.Business.Name}";
        }

        private void Add()
        {
            Caption = "Adding new Customer";
            Customers = null;
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

        private void EntityTypeChanged()
        {
            if (Customer.EntityType == EntityType.Business)
            {
                if (Customer.Business is null)
                    Customer.Business = new();

                Customer.Person = null;
            }

            if (Customer.EntityType == EntityType.Person)
            {
                var name = new PersonNameToWrite();

                if (Customer.Person is null)
                {
                    Customer.Person = new();
                }

                Customer.Person.Name = name;
                Customer.Business = null;
            }
        }

        protected async Task AddSubmit()
        {
            await CustomerDataService.AddAsync(Customer);
            await Close();
        }

        protected async Task EditSubmit()
        {
            await CustomerDataService.UpdateAsync(Customer);
            await EndEditAsync();
        }

        protected async Task Submit()
        {
            if (FormMode == FormMode.Add)
                await AddSubmit();

            if (FormMode == FormMode.Edit)
                await EditSubmit();
        }

        protected async Task EndEditAsync()
        {
            Caption = string.Empty;
            await GetCustomers();
            FormMode = FormMode.View;
        }

        protected async Task Close()
        {
            Customer = null;
            FormMode = FormMode.View;
            Caption = string.Empty;
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
