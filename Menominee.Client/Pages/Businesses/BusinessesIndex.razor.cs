using Menominee.Client.Components.Address;
using Menominee.Client.Services.Businesses;
using Menominee.Common.Enums;
using Menominee.Shared.Models.Addresses;
using Menominee.Shared.Models.Businesses;
using Menominee.Shared.Models.Contactable;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;

namespace Menominee.Client.Pages.Businesses
{
    public partial class BusinessesIndex : ComponentBase
    {
        [Inject]
        public IBusinessDataService BusinessDataService { get; set; }

        [Inject]
        public ILogger<BusinessesIndex> Logger { get; set; }

        public IReadOnlyList<BusinessToReadInList> Businesses;

        public TelerikGrid<BusinessToReadInList> Grid { get; set; }
        private long Id { get; set; }

        private FormMode BusinessFormMode = FormMode.Unknown;

        private FormMode AddressFormMode = FormMode.Unknown;
        private BusinessToWrite Business { get; set; }

        private readonly AddressEditor addressEditor;
        protected override async Task OnInitializedAsync()
        {
            Businesses = (await BusinessDataService.GetAllBusinesses()).ToList();
        }

        private void AddAddress()
        {
            Business.Address = new();
            AddressFormMode = FormMode.Add;
        }

        private void EditAddress()
        {
            AddressFormMode = FormMode.Edit;
        }
        private void CancelEditAddress()
        {
            if (AddressFormMode == FormMode.Edit)
            {
                addressEditor.Cancel();
                AddressFormMode = FormMode.Unknown;
            }

            if (BusinessFormMode == FormMode.Add && Business.Address is not null)
            {
                AddressFormMode = FormMode.Unknown;
                StateHasChanged();  //TODO: When Business is new and Address is new, how to cancel?
                                    //Dialog does not close, console error: null reference exception
                Business.Address = null;
            }
        }

        private async Task EditAsync(GridRowClickEventArgs args)
        {
            Id = (args.Item as BusinessToReadInList).Id;
            BusinessFormMode = FormMode.Edit;
            Businesses = null; // TODO: Huh?

            var readDto = await BusinessDataService.GetBusiness(Id);
            Business = new BusinessToWrite
            {
                Name = readDto.Name,
                Notes = readDto.Notes
            };

            if (readDto.Address != null)
            {
                Business.Address = new AddressToWrite
                {
                    AddressLine1 = readDto.Address.AddressLine1,
                    City = readDto.Address.City,
                    State = readDto.Address.State,
                    PostalCode = readDto.Address.PostalCode,
                    AddressLine2 = readDto.Address.AddressLine2
                };
            }

            foreach (var email in readDto?.Emails)
            {
                Business.Emails.Add(new EmailToWrite
                {
                    Address = email.Address,
                    IsPrimary = email.IsPrimary
                });
            }

            foreach (var phone in readDto?.Phones)
            {
                Business.Phones.Add(new PhoneToWrite
                {
                    Id = phone.Id,
                    Number = phone.Number,
                    PhoneType = phone.PhoneType,
                    IsPrimary = phone.IsPrimary
                });
            }

        }

        private void Add()
        {
            BusinessFormMode = FormMode.Add;
            Businesses = null;
            Business = new();
        }

        protected async Task HandleAddSubmit()
        {
            if (!string.IsNullOrWhiteSpace(Business.Name))
            {
                await BusinessDataService.AddBusiness(Business);
                await EndAddEditAsync();
            }
        }

        protected async Task HandleEditSubmit()
        {
            if (!string.IsNullOrWhiteSpace(Business.Name))
            {
                await BusinessDataService.UpdateBusiness(Business, Id);
                await EndAddEditAsync();
            }
        }

        protected async Task SubmitHandlerAsync()
        {
            if (!string.IsNullOrWhiteSpace(Business.Name))
            {
                if (BusinessFormMode == FormMode.Add)
                    await HandleAddSubmit();

                if (BusinessFormMode == FormMode.Edit)
                    await HandleEditSubmit();
            }
        }

        protected async Task EndAddEditAsync()
        {
            BusinessFormMode = FormMode.Unknown;
            AddressFormMode = FormMode.Unknown;
            Businesses = (await BusinessDataService.GetAllBusinesses()).ToList();
        }
    }
}
