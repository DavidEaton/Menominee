using Menominee.Client.Services.Payables.Vendors;
using Menominee.Common.Enums;
using Menominee.Common.Http;
using Menominee.Shared.Models.Payables.Vendors;
using Microsoft.AspNetCore.Components;

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
                PostResponse result = await VendorDataService.AddVendorAsync(Vendor);
                Id = result.Id;
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
