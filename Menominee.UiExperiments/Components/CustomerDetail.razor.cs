using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Menominee.UiExperiments.Pages.Customers;
using SharedKernel.Enums;
using System.Collections.Generic;

namespace Menominee.UiExperiments.Components
{
    public partial class CustomerDetail : ComponentBase
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        [Parameter]
        public long Id { get; set; }
        public CustomerList Customer { get; set; }

        // Screen state
        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;
        protected bool Saved;

        protected override async Task OnParametersSetAsync()
        {
            Saved = false;

            await GetCustomers();
        }

        private async Task GetCustomers()
        {
            CustomerList[] customers = await HttpClient.GetFromJsonAsync<CustomerList[]>("sample-data/customers.json");
            customers = FormatPersonData(customers);

            foreach (var customer in customers)
            {
                if (customer.Id == Id && Id != 0)
                {
                    if (customer.EntityType == EntityType.Person)
                    {
                        customer.FirstName = customer.PersonName.FirstName;
                        customer.MiddleName = customer.PersonName.MiddleName;
                        customer.LastName = customer.PersonName.LastName;
                    }

                    Customer = customer;
                }
            }
        }

        private static CustomerList[] FormatPersonData(CustomerList[] customers)
        {
            foreach (var customer in customers)
            {
                if (!string.IsNullOrEmpty(customer.Phone))
                    customer.Phone = Regex.Replace(customer.Phone, @"(\d{3})(\d{3})(\d{4})", "($1) $2-$3");
            }

            return customers;
        }

        public void Close()
        {
            Saved = true;
            Message = string.Empty;
            StatusClass = string.Empty;
            Id = -1;
            Customer.Id = 0;
            StateHasChanged();
        }
    }
}
