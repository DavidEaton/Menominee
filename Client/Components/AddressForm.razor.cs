using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CustomerVehicleManagement.Domain.ValueObjects;

namespace Client.Components
{
    public partial class AddressForm : ComponentBase
    {
        AddressProperties Address { get; set; } = new AddressProperties();

        protected async Task HandleValidSubmit()
        {
            var address = new Address(Address.AddressLine, Address.City, Address.State, Address.PostalCode);


            //await CloseEventCallback.InvokeAsync(true);
            StateHasChanged();
        }

        protected async Task HandleInValidSubmit()
        {

        }

        private class AddressProperties
        {
            [Required]
            [StringLength(100, ErrorMessage = "Please limit Address to 100 characters")]
            public string AddressLine { get; set; }
            
            [Required]
            [StringLength(100, ErrorMessage = "Please limit City to 100 characters")]
            public string City { get; set; }
            
            [Required]
            public string State { get; set; }
            
            [Required]
            [StringLength(50, ErrorMessage = "Please limit Postal Code to 50 characters")]
            public string PostalCode { get; set; }
        }
    }
}
