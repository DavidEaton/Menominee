using MenomineePlayWASM.Shared.Dtos.Payables.Vendors;
using MenomineePlayWASM.Shared.Services.Payables.Vendors;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Client.Pages.Payables
{
    public partial class VendorEdit : ComponentBase
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
                var readDto = await vendorDataService.GetVendor(Id);
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
                var vendor = await vendorDataService.AddVendor(Vendor);
                Id = vendor.Id;
            }
            else
            {
                await vendorDataService.UpdateVendor(Vendor, Id);
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
