using Menominee.Common.Enums;
using Menominee.Shared.Models.Customers;
using Menominee.Shared.Models.Persons;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Menominee.Client.Components.Customers
{
    public partial class CustomerEditor
    {
        [Parameter]
        public CustomerToWrite Customer { get; set; }

        [Parameter]
        public FormMode FormMode { get; set; }

        [Parameter]
        public EventCallback<CustomerToWrite> OnSave { get; set; }

        [Parameter]
        public EventCallback OnDiscard { get; set; }

        private string Title => $"{FormMode} Customer";
        private EditContext EditContext { get; set; } = default!;
        private CustomerValidator CustomerValidator { get; set; } = new();
        private CustomerToWrite CustomerModel { get; set; } = new();
        private List<CustomerTypeEnumModel> CustomerTypeEnumData { get; set; } = new List<CustomerTypeEnumModel>();
        private List<EntityTypeEnumModel> EntityTypeEnumData { get; set; } = new List<EntityTypeEnumModel>();
        private bool Submitting { get; set; } = false;

        protected override void OnInitialized()
        {
            if (FormMode.Equals(FormMode.Edit) && Customer is not null)
            {
                CustomerModel = Customer;
            }

            foreach (CustomerType item in Enum.GetValues(typeof(CustomerType)))
            {
                CustomerTypeEnumData.Add(new CustomerTypeEnumModel { DisplayText = EnumExtensions.GetDisplayName(item), Value = item });
            }

            foreach (EntityType item in Enum.GetValues(typeof(EntityType)))
            {
                EntityTypeEnumData.Add(new EntityTypeEnumModel { DisplayText = item.ToString(), Value = item });
            }

            EditContext = new EditContext(CustomerModel);
            if (FormMode.Equals(FormMode.Add))
            {
                CustomerModel.CustomerType = CustomerType.Retail;
            }

            CustomerTypeChanged();

            base.OnInitialized();
        }

        private async Task HandleSubmit(EditContext editContext)
        {
            if (!Submitting)
            {
                return;
            }
            Submitting = false; // reset in case of error

            var isValid = editContext.Validate();

            if (!isValid)
            {
                return;
            }

            var customer = editContext.Model as CustomerToWrite;
            customer!.Id = CustomerModel.Id;

            await OnSave.InvokeAsync(customer);
        }

        private async Task HandleDiscard()
        {
            await OnDiscard.InvokeAsync();
        }

        protected void Submit()
        {
            Submitting = true;
        }

        protected void Close()
        {

        }

        private void CustomerTypeChanged()
        {
            switch (CustomerModel.CustomerType)
            {
                case CustomerType.Retail:
                case CustomerType.Employee:
                    CustomerModel.EntityType = EntityType.Person;
                    break;
                case CustomerType.Business:
                case CustomerType.Fleet:
                case CustomerType.BillingCenter:
                case CustomerType.BillingCenterPrepaid:
                    CustomerModel.EntityType = EntityType.Business;
                    break;
                default:
                    CustomerModel.EntityType = EntityType.Person;
                    break;
            }

            EntityTypeChanged();
        }

        private void EntityTypeChanged()
        {
            if (CustomerModel.EntityType == EntityType.Business)
            {
                if (CustomerModel.Business is null)
                {
                    CustomerModel.Business = new()
                    {
                        Address = new()
                    };
                }

                CustomerModel.Person = null;
            }

            if (CustomerModel.EntityType == EntityType.Person)
            {
                var name = new PersonNameToWrite();

                if (CustomerModel.Person is null)
                {
                    CustomerModel.Person = new()
                    {
                        Address = new()
                    };
                }

                CustomerModel.Person.Name = name;
                CustomerModel.Business = null;
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
