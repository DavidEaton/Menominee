using CustomerVehicleManagement.Shared.Models;
using Microsoft.AspNetCore.Components;
using Menominee.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Menominee.Client.Components
{
    public partial class AddressForm : ComponentBase
    {
        AddressProperties AddressValidators { get; set; } = new AddressProperties();
        public AddressToAdd EntityAddress { get; set; }

        [Parameter]
        public EventCallback<bool> CloseEventCallback { get; set; }

        protected async Task Validate()
        {
            if (AddressValidators.IsValid)
            {
                EntityAddress = new AddressToAdd
                {
                    AddressLine = AddressValidators.AddressLine,
                    City = AddressValidators.City,
                    State = AddressValidators.State,
                    PostalCode = AddressValidators.PostalCode
                };

                await CloseEventCallback.InvokeAsync(true);
                StateHasChanged();
            }
            else
            {
                // TODO: display invalid message(s)

            }
        }

        private class AddressProperties
        {
            [StringLength(100, ErrorMessage = "Please limit Address to 100 characters")]
            public string AddressLine { get; set; }

            [StringLength(100, ErrorMessage = "Please limit City to 100 characters")]
            public string City { get; set; }

            public State State { get; set; }

            [StringLength(50, ErrorMessage = "Please limit Postal Code to 50 characters")]
            public string PostalCode { get; set; }

            public bool IsValid =>
                       AddressLine?.Length < 100
                    && City?.Length < 100
                    && PostalCode?.Length < 50;
        }
    }
}
