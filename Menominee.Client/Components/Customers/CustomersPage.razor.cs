using CustomerVehicleManagement.Shared.Models;
using CustomerVehicleManagement.Shared.Models.Customers;
using CustomerVehicleManagement.Shared.Models.Persons;
using Menominee.Client.Services.Customers;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.Customers
{
    public partial class CustomersPage : ComponentBase
    {
        [Inject]
        public ICustomerDataService CustomerDataService { get; set; }

        public IReadOnlyList<CustomerToReadInList> Customers;
        public FormMode FormMode { get; set; } = FormMode.View;
        private CustomerToWrite Customer { get; set; }
        private long Id { get; set; }
        private string Caption { get; set; } = string.Empty;
        List<CustomerTypeEnumModel> CustomerTypeEnumData { get; set; } = new List<CustomerTypeEnumModel>();
        List<EntityTypeEnumModel> EntityTypeEnumData { get; set; } = new List<EntityTypeEnumModel>();

        protected override async Task OnInitializedAsync()
        {
            Customers = (await CustomerDataService.GetAllCustomers()).ToList();
    
            foreach (CustomerType item in Enum.GetValues(typeof(CustomerType)))
                CustomerTypeEnumData.Add(new CustomerTypeEnumModel { DisplayText = item.ToString(), Value = item });

            foreach (EntityType item in Enum.GetValues(typeof(EntityType)))
                EntityTypeEnumData.Add(new EntityTypeEnumModel { DisplayText = item.ToString(), Value = item });
        }

        private async Task EditAsync(GridRowClickEventArgs args)
        {
            Id = (args.Item as CustomerToReadInList).Id;
            FormMode = FormMode.Edit;
            Customers = null;

            CustomerToRead customerReadDto = await CustomerDataService.GetCustomer(Id);

            Customer = CustomerHelper.CreateWriteFromReadDto(customerReadDto);

            if (customerReadDto.EntityType == EntityType.Person)
                Caption = $"Editing Customer: {customerReadDto.Person.FirstMiddleLast}";

            if (customerReadDto.EntityType == EntityType.Organization)
                Caption = $"Editing Customer: {customerReadDto.Organization.Name}";

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
                Organization = new()
            };

            Customer.Person.Name = name;

            FormMode = FormMode.Add;
        }

        private void EntityTypeChanged()
        {
            if (Customer.EntityType == EntityType.Organization)
            {
                if (Customer.Organization is null)
                    Customer.Organization = new();

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
                Customer.Organization = null;
            }
        }

        protected async Task AddSubmit()
        {
            await CustomerDataService.AddCustomer(Customer);
            await Close();
        }

        protected async Task EditSubmit()
        {
            await CustomerDataService.UpdateCustomer(Customer, Id);
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
            Customers = (await CustomerDataService.GetAllCustomers()).ToList();
            FormMode = FormMode.View;
        }

        protected async Task Close()
        {
            FormMode = FormMode.View;
            Customers = (await CustomerDataService.GetAllCustomers()).ToList();
            Caption = string.Empty;
            Customer = null;
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
