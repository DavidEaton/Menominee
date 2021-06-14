using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Menominee.UiExperiments.Components;
using Menominee.UiExperiments.Models;
using SharedKernel.Enums;
using SharedKernel.ValueObjects;

namespace Menominee.UiExperiments.Pages
{
    public partial class Customers
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        public IEnumerable<CustomerList> CustomersList;
        public int SelectedId { get; set; }
        protected CustomerDetail CustomerDetail { get; set; }
        protected override async Task OnInitializedAsync()
        {
            CustomersList = await HttpClient.GetFromJsonAsync<CustomerList[]>("sample-data/customers.json");
            CustomersList = FormatPersonData(CustomersList);
        }

        private static IEnumerable<CustomerList> FormatPersonData(IEnumerable<CustomerList> customers)
        {
            foreach (var customer in customers)
            {
                if (!string.IsNullOrEmpty(customer.Phone))
                    customer.Phone = Regex.Replace(customer.Phone, @"(\d{3})(\d{3})(\d{4})", "($1) $2-$3");
            }

            return customers;
        }

        public void RowSelectedHandler(RowSelectEventArgs<CustomerList> args)
        {
            SelectedId = args.Data.Id;
        }

        public class CustomerList
        {
            public int Id { get; set; }
            public EntityType EntityType { get; set; }
            public string Name { get; set; }
            public PersonName PersonName { get; set; }
            public string LastName { get; set; }
            public string MiddleName { get; set; }
            public string FirstName { get; set; }
            public CustomerType CustomerType { get; set; }
            public string AddressLine { get; set; }
            public string AddressCity { get; set; }
            public string AddressState { get; set; }
            public string AddressPostalCode { get; set; }
            public string Phone { get; set; }
            public string AddressFull { get => $"{AddressLine} {AddressCity}, {AddressState}  {AddressPostalCode}"; }
            public string CustomerName
            {
                get
                {
                    if (EntityType == EntityType.Person)
                        return PersonName.LastFirstMiddle;

                    if (EntityType == EntityType.Organization)
                        return Name;

                    return string.Empty;
                }
            }
        }
    }
}
