using Client.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Client.Pages
{
    public partial class Customers : ComponentBase
    {
        //[Inject]
        //public ICustomerDataService CustomerDataService { get; set; }
        public IEnumerable<CustomerFlatDto> CustomersList;
        public int SelectedId { get; set; }

    }
}
