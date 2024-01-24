using CSharpFunctionalExtensions;
using Menominee.Client.Services.Customers;
using Menominee.Shared.Models.Customers;
using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components.Customers
{
    public partial class CustomerLookupSelectDialog : ComponentBase
    {
        [Inject]
        private ICustomerDataService CustomerDataService { get; set; }

        [Inject]
        public ILogger<CustomerLookupSelectDialog> Logger { get; set; }

        [Parameter]
        public bool DialogVisible { get; set; }

        public IReadOnlyList<CustomerToReadInList>? Customers;

        [Parameter]
        public EventCallback<CustomerToReadInList> OnSelected { get; set; }

        public EventCallback OnCancel { get; set; }

        private void CustomerSeleted(CustomerToReadInList customer)
        {
            OnSelected.InvokeAsync(customer);
        }

        protected override async Task OnInitializedAsync()
        {
            await GetCustomers();
        }

        private async Task GetCustomers()
        {
            await CustomerDataService.GetAllAsync()
                .Match(
                    success => Customers = success,
                    failure => Logger.LogError(failure)
                );
        }
    }
}
