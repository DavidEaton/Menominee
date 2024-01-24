using Menominee.Domain.Enums;
using Menominee.Shared.Models.RepairOrders;
using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components.RepairOrders
{
    public partial class RepairOrderCustomer : ComponentBase
    {
        //[Parameter]
        //public string CustomerName { get; set; }    // customer name as it was when repair order was saved

        //[Parameter]
        //public Customer Customer { get; set; }      // customer record as it exists now

        [Parameter]
        public RepairOrderToWrite RepairOrderToEdit { get; set; }

        [Parameter]
        public EventCallback<RepairOrderToWrite> RepairOrderToEditChanged { get; set; }

        [Parameter]
        public EventCallback OnAdd { get; set; }

        [Parameter]
        public EventCallback OnEdit { get; set; }

        [Parameter]
        public EventCallback<CustomerLookupMode> OnLookup { get; set; }

        [Parameter]
        public EventCallback OnCustomerLookup { get; set; }

        private bool CanEdit => RepairOrderToEdit.Customer is not null && RepairOrderToEdit.Customer.Id > 0;

        private async Task AddCustomer()
        {
            await OnAdd.InvokeAsync();
        }

        private async Task EditCustomer()
        {
            await OnEdit.InvokeAsync();
        }

        private async Task LookupCustomer()
        {
            await OnCustomerLookup.InvokeAsync();
        }

        private async Task Lookup()
        {
            Console.WriteLine("Lookup invoked from RepairOrderCustomer!");
        }
    }
}
