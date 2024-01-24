using Menominee.Shared.Models.Customers;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.Customers
{
    public partial class CustomersListGrid : ComponentBase
    {
        [Parameter]
        public IReadOnlyList<CustomerToReadInList> CustomersList { get; set; }

        [Parameter]
        public EventCallback<CustomerToReadInList> CustomerSeleted { get; set; }

        private CustomerToReadInList SelectedCustomer { get; set; }

        private void OnRowDoubleClickHandler(GridRowClickEventArgs args)
        {
            CustomerSeleted.InvokeAsync(args.Item as CustomerToReadInList);
        }

        private void OnRowClickHandler(GridRowClickEventArgs args)
        {
            if (args is not null)
            {
                SelectedCustomer = (CustomerToReadInList)args.Item;
            }
        }

        private void SelectCustomer()
        {
            if (SelectedCustomer is not null)
            {
                CustomerSeleted.InvokeAsync(SelectedCustomer);
            }
        }

    }
}
