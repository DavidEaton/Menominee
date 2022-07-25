using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Menominee.Client.Services.Payables.Vendors;
using CustomerVehicleManagement.Shared.Models.Payables.Vendors;

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

        protected override async Task OnInitializedAsync()
        {
            if (Id == 0)
            {
                Vendor = new();
            }
            else
            {
                var readDto = await vendorDataService.GetVendorAsync(Id);
                Vendor = new VendorToWrite()
                {
                    Id = readDto.Id,
                    VendorCode = readDto.VendorCode,
                    Name = readDto.Name,
                    IsActive = readDto.IsActive
                };
            }
        }

        private async Task Save()
        {
            //if (!string.IsNullOrWhiteSpace(VendorToAdd.VendorId) && !string.IsNullOrWhiteSpace(VendorToAdd.Name))
            if (Id == 0)
            {
                var vendor = await vendorDataService.AddVendorAsync(Vendor);
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
