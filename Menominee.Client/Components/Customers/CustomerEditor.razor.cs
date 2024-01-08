using FluentValidation;
using Menominee.Domain.Enums;
using Menominee.Shared.Models.Addresses;
using Menominee.Shared.Models.Businesses;
using Menominee.Shared.Models.Customers;
using Menominee.Shared.Models.Persons;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.Customers
{
    public partial class CustomerEditor
    {
        private EditContext? CustomerEditContext;

        [Parameter]
        public CustomerToWrite? Customer { get; set; }

        private TelerikForm CustomerForm { get; set; }

        [Parameter]
        public FormMode FormMode { get; set; }

        [Parameter]
        public EventCallback<CustomerToWrite> OnSave { get; set; }
        [Inject] private IValidator<CustomerToWrite>? CustomerValidator { get; set; }

        [Parameter]
        public EventCallback OnDiscard { get; set; }

        private string Title => $"{FormMode} Customer";
        private List<CustomerTypeEnumModel> CustomerTypeEnumData { get; set; } = new List<CustomerTypeEnumModel>();
        private List<EntityTypeEnumModel> EntityTypeEnumData { get; set; } = new List<EntityTypeEnumModel>();
        private bool Submitting { get; set; } = false;

        protected override void OnInitialized()
        {

            foreach (CustomerType item in Enum.GetValues(typeof(CustomerType)))
            {
                CustomerTypeEnumData.Add(new CustomerTypeEnumModel { DisplayText = EnumExtensions.GetDisplayName(item), Value = item });
            }

            foreach (EntityType item in Enum.GetValues(typeof(EntityType)))
            {
                EntityTypeEnumData.Add(new EntityTypeEnumModel { DisplayText = item.ToString(), Value = item });
            }

            base.OnInitialized();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            CustomerEditContext = CustomerForm?.EditContext;
            base.OnAfterRender(firstRender);
        }

        private async Task HandleSubmit()
        {
            if (!Submitting)
            {
                return;
            }

            Submitting = false; // reset in case of error

            await OnSave.InvokeAsync(Customer);
        }

        private void TrimCustomerRequest()
        {
            if (Customer is null)
            {
                return;
            }

            TrimPersonDetails(Customer?.Person);
            TrimBusinessDetails(Customer?.Business);
            Customer.Code = Customer.Code?.Trim();
        }

        private static void TrimPersonDetails(PersonToWrite person)
        {
            if (person?.Name is null)
            {
                return;
            }

            person.Name.FirstName = person.Name.FirstName?.Trim();
            person.Name.LastName = person.Name.LastName?.Trim();
            person.Name.MiddleName = person.Name.MiddleName?.Trim();

            TrimAddress(person.Address);
        }

        private static void TrimBusinessDetails(BusinessToWrite business)
        {
            if (business?.Name is null)
            {
                return;
            }

            business.Name.Name = business.Name.Name?.Trim();

            TrimAddress(business.Address);
        }

        private static void TrimAddress(AddressToWrite address)
        {
            if (address is null || address.IsEmpty)
            {
                return;
            }

            address.AddressLine1 = address.AddressLine1?.Trim();
            address.AddressLine2 = address.AddressLine2?.Trim();
            address.City = address.City?.Trim();
            address.PostalCode = address.PostalCode?.Trim();
        }

        private async Task HandleDiscard()
        {
            await OnDiscard.InvokeAsync();
        }

        public bool IsValid => CustomerValidator is not null
                    && CustomerValidator.Validate(Customer).IsValid;

        private void ValidateCustomerForm()
        {
            CustomerForm?.EditContext?.NotifyValidationStateChanged();
        }

        protected void DeleteAddress()
        {
            if (Customer.IsBusiness)
            {
                DeleteBusinessAddress();
            }

            if (Customer.IsPerson)
            {
                DeletePersonAddress();
            }
        }

        private void DeletePersonAddress()
        {
            Customer.Person.Address = null;
        }

        private void DeleteBusinessAddress()
        {
            Customer.Business.Address = null;
        }

        protected void Submit()
        {
            TrimCustomerRequest();
            Submitting = true;
        }

        protected void Close()
        {

        }

        private void CustomerTypeChanged()
        {
            switch (Customer.CustomerType)
            {
                case CustomerType.Retail:
                case CustomerType.Employee:
                    Customer.EntityType = EntityType.Person;
                    break;
                case CustomerType.Business:
                case CustomerType.Fleet:
                case CustomerType.BillingCenter:
                case CustomerType.BillingCenterPrepaid:
                    Customer.EntityType = EntityType.Business;
                    break;
                default:
                    Customer.EntityType = EntityType.Person;
                    break;
            }

            EntityTypeChanged();
        }

        private void EntityTypeChanged()
        {
            if (Customer.EntityType == EntityType.Business)
            {
                if (Customer.Business is null)
                {
                    Customer.Business = new()
                    {
                        Address = new()
                    };
                }

                Customer.Person = null;
            }

            if (Customer.EntityType == EntityType.Person)
            {
                var name = new PersonNameToWrite();

                if (Customer.Person is null)
                {
                    Customer.Person = new()
                    {
                        Address = new()
                    };
                }

                Customer.Person.Name = name;
                Customer.Business = null;
            }
        }

        private void TabChangedHandler(int newIndex)
        {
            if (newIndex == 1)
            {

            }
        }

        private class CustomerTypeEnumModel
        {
            public CustomerType Value { get; set; }
            public string DisplayText { get; set; } = string.Empty;
        }

        private class EntityTypeEnumModel
        {
            public EntityType Value { get; set; }
            public string DisplayText { get; set; } = string.Empty;
        }

    }
}
