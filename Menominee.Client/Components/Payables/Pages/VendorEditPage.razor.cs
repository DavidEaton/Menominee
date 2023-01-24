using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Menominee.Client.Services.Payables.Vendors;
using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using Menominee.Common.Enums;

namespace Menominee.Client.Components.Payables.Pages
{
    public partial class VendorEditPage : ComponentBase
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        public IVendorDataService VendorDataService { get; set; }

        [Parameter]
        public long Id { get; set; }

        private VendorToWrite Vendor { get; set; }
        private FormMode FormMode { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (Id == 0)
            {
                Vendor = new()
                {
                    IsActive = true
                };
                FormMode = FormMode.Add;
            }
            else
            {
                VendorToRead vendorToRead = await VendorDataService.GetVendorAsync(Id);

                if (vendorToRead is not null)
                {
                    Vendor = VendorHelper.ConvertReadToWriteDto(vendorToRead);
                    FormMode = FormMode.Edit;
                }
                else
                {
                    // TODO: Need to handle this gracefully
                    Vendor = null;
                    FormMode = FormMode.View;
                }
            }
        }

        private async Task Save()
        {
            if (Id == 0)
            {
                await VendorDataService.AddVendorAsync(Vendor);
            }
            else
                await VendorDataService.UpdateVendorAsync(Vendor, Id);

            EndEdit();
        }

        private void Discard()
        {
            EndEdit();
        }

        protected void EndEdit()
        {
            NavigationManager.NavigateTo($"payables/vendors/listing/{Id}");
        }
    }
}
