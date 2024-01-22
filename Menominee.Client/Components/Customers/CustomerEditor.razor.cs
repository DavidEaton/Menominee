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
            Customer.Person.Address = new();
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
            // Determine the target entity type based on the customer type.
            EntityType targetType = (Customer.CustomerType == CustomerType.Retail || Customer.CustomerType == CustomerType.Employee)
                                    ? EntityType.Person : EntityType.Business;

            // Only change the entity type if it's different from the current one.
            if (Customer.EntityType != targetType)
            {
                Customer.EntityType = targetType;
                EntityTypeChanged();
            }
        }


        private void EntityTypeChanged()
        {
            if (Customer.EntityType == EntityType.Business)
            {
                if (Customer.Business is null)
                {
                    Customer.Business = new BusinessToWrite
                    {
                        // Preserving the name if it already exists.
                        Name = Customer.Business?.Name ?? new(),
                    };
                }

                // Clear the Person object only if it's not the current entity type.
                if (Customer.CustomerType is not CustomerType.Employee && Customer.CustomerType is not CustomerType.Retail)
                {
                    Customer.Person = null;
                }
            }
            else if (Customer.EntityType == EntityType.Person)
            {
                if (Customer.Person is null)
                {
                    Customer.Person = new PersonToWrite
                    {
                        // Preserving the name if it already exists.
                        Name = Customer.Person?.Name ?? new(),
                    };
                }

                // Clear the Business object only if it's not the current entity type.
                if (Customer.CustomerType is not CustomerType.Business && Customer.CustomerType is not CustomerType.Fleet
                    && Customer.CustomerType is not CustomerType.BillingCenter && Customer.CustomerType is not CustomerType.BillingCenterPrepaid)
                {
                    Customer.Business = null;
                }
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
