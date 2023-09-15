using CSharpFunctionalExtensions;
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
            await BusinessDataService.GetAllBusinesses()
                .Match(
                    success => Businesses = success,
                    failure => Logger.LogError(failure)
            );
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
            var id = (args.Item as BusinessToReadInList)?.Id ?? Id;
            var formMode = args.Item switch
            {
                BusinessToReadInList => FormMode.Edit,
                _ => BusinessFormMode
            };

            var businessResult = await BusinessDataService.GetBusiness(id)
                .OnFailure(error =>
                {
                    Logger.LogError(error);
                    // TODO: Replace exception with toast message
                    throw new Exception(error);
                });

            var business = businessResult.Value;

            Business = new BusinessToWrite
            {
                Name = business.Name,
                Notes = business.Notes,
                Address = business.Address is not null
                ? new AddressToWrite
                {
                    AddressLine1 = business.Address.AddressLine1,
                    City = business.Address.City,
                    State = business.Address.State,
                    PostalCode = business.Address.PostalCode,
                    AddressLine2 = business.Address.AddressLine2
                }
                : null
            };

            Business.Emails.AddRange(
                business?.Emails.Select(email => new EmailToWrite
                {
                    Address = email.Address,
                    IsPrimary = email.IsPrimary
                })
                ?? Enumerable.Empty<EmailToWrite>()
            );

            Business.Phones.AddRange(
                business?.Phones?.Select(phone => new PhoneToWrite
                {
                    Id = phone.Id,
                    Number = phone.Number,
                    PhoneType = phone.PhoneType,
                    IsPrimary = phone.IsPrimary
                })
                ?? Enumerable.Empty<PhoneToWrite>()
            );
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
            await BusinessDataService.GetAllBusinesses()
                .Match(
                    success => Businesses = success,
                    failure => Logger.LogError(failure));
        }
    }
}
