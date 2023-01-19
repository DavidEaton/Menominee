using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Menominee.Client.Services.Payables.Vendors;
using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using Menominee.Common.Enums;
using System;

namespace Menominee.Client.Components.Payables.Pages
{
    public partial class VendorEditPage : ComponentBase
    {
        [Inject]
        private NavigationManager navigationManager { get; set; }

        [Inject]
        public IVendorDataService vendorDataService { get; set; }

        [Parameter]
        public long Id { get; set; }

        private VendorToWrite Vendor { get; set; }
        private FormMode FormMode { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (Id == 0)
            {
                Vendor = new();
                FormMode = FormMode.Add;
            }
            else
            {
                VendorToRead vendorToRead = await vendorDataService.GetVendorAsync(Id);

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
            //if (!string.IsNullOrWhiteSpace(VendorToAdd.VendorId) && !string.IsNullOrWhiteSpace(VendorToAdd.Name))
            if (Id == 0)
            {
                var vendor = await vendorDataService.AddVendorAsync(Vendor);
                // TODO: this could fail leaving vendor null
                Id = vendor.Id;
            }
            else
            {
                await vendorDataService.UpdateVendorAsync(Vendor, Id);
            }

            EndEdit();
        }

        private void Discard()
        {
            EndEdit();
        }

        protected void EndEdit()
        {
            navigationManager.NavigateTo($"payables/vendors/listing/{Id}");
        }
    }
}
