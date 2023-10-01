using Menominee.Client.Services.Payables.Vendors;
using Menominee.Common.Enums;
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

        [Inject]
        ILogger<VendorEditor> Logger { get; set; }

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
                var result = await VendorDataService.GetAsync(Id);

                if (result.IsFailure)
                {
                    // TODO: Need to handle this gracefully
                    Logger.LogError($"Failed to get vendor with id {Id}");
                    Vendor = new();
                    FormMode = FormMode.View;
                }

                Vendor = VendorHelper.ConvertReadToWriteDto(result.Value);
                FormMode = FormMode.Edit;
            }
        }

        private async Task Save()
        {
            if (Id == 0)
            {
                var result = await VendorDataService.AddAsync(Vendor);

                if (result.IsSuccess)
                    Id = result.Value.Id;

                if (result.IsFailure)
                    // TODO: log it, display toast message
                    return;
            }

            else
                await VendorDataService.UpdateAsync(Vendor);

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
