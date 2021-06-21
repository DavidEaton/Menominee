using CustomerVehicleManagement.Shared.Models;
using Menominee.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Client.Pages
{
    public partial class Customers
    {
        [Inject]
        public ICustomerDataService CustomerDataService { get; set; }

        [Inject]
        public ILogger<Customers> Logger { get; set; }

        public IReadOnlyList<CustomerInListDto> CustomersList;
        public int SelectedId { get; set; }

        //protected AddCustomerDialog AddCustomerDialog { get; set; }
        //protected CustomerDetail CustomerDetail { get; set; }
        protected override async Task OnInitializedAsync()
        {
            Logger.LogInformation("Customers.OnInitializedAsync()");
            CustomersList = await CustomerDataService.GetAllCustomers();
        }

        protected void AddCustomer()
        {
            //CustomerDetail?.Close();
            //AddCustomerDialog.Show();
        }
        private void SetSelectedId(int id)
        {
            SelectedId = id;
        }
    }
}
