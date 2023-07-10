using Menominee.Shared.Models;
using Menominee.Shared.Models.Addresses;
using Menominee.Shared.Models.Contactable;
using Menominee.Shared.Models.Organizations;
using Menominee.Client.Components.Address;
using Menominee.Client.Services;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace Menominee.Client.Pages.Organizations
{
    public partial class OrganizationsIndex : ComponentBase
    {
        [Inject]
        public IOrganizationDataService OrganizationDataService { get; set; }

        [Inject]
        public ILogger<OrganizationsIndex> Logger { get; set; }

        public IReadOnlyList<OrganizationToReadInList> Organizations;

        public TelerikGrid<OrganizationToReadInList> Grid { get; set; }
        private long Id { get; set; }

        private FormMode OrganizationFormMode = FormMode.Unknown;

        private FormMode AddressFormMode = FormMode.Unknown;
        private OrganizationToWrite Organization { get; set; }

        private readonly AddressEditor addressEditor;
        protected override async Task OnInitializedAsync()
        {
            Organizations = (await OrganizationDataService.GetAllOrganizations()).ToList();
        }

        private void AddAddress()
        {
            Organization.Address = new();
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

            if (OrganizationFormMode == FormMode.Add && Organization.Address is not null)
            {
                AddressFormMode = FormMode.Unknown;
                StateHasChanged();  //TODO: When Organization is new and Address is new, how to cancel?
                                    //Dialog does not close, console error: null reference exception
                Organization.Address = null;
            }
        }

        private async Task EditAsync(GridRowClickEventArgs args)
        {
            Id = (args.Item as OrganizationToReadInList).Id;
            OrganizationFormMode = FormMode.Edit;
            Organizations = null; // TODO: Huh?

            var readDto = await OrganizationDataService.GetOrganization(Id);
            Organization = new OrganizationToWrite
            {
                Name = readDto.Name,
                Notes = readDto.Notes
            };

            if (readDto.Address != null)
            {
                Organization.Address = new AddressToWrite
                {
                    AddressLine = readDto.Address.AddressLine,
                    City = readDto.Address.City,
                    State = readDto.Address.State,
                    PostalCode = readDto.Address.PostalCode
                };
            }

            foreach (var email in readDto?.Emails)
            {
                Organization.Emails.Add(new EmailToWrite
                {
                    Address = email.Address,
                    IsPrimary = email.IsPrimary
                });
            }

            foreach (var phone in readDto?.Phones)
            {
                Organization.Phones.Add(new PhoneToWrite
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
            OrganizationFormMode = FormMode.Add;
            Organizations = null;
            Organization = new();
        }

        protected async Task HandleAddSubmit()
        {
            if (!string.IsNullOrWhiteSpace(Organization.Name))
            {
                await OrganizationDataService.AddOrganization(Organization);
                await EndAddEditAsync();
            }
        }

        protected async Task HandleEditSubmit()
        {
            if (!string.IsNullOrWhiteSpace(Organization.Name))
            {
                await OrganizationDataService.UpdateOrganization(Organization, Id);
                await EndAddEditAsync();
            }
        }

        protected async Task SubmitHandlerAsync()
        {
            if (!string.IsNullOrWhiteSpace(Organization.Name))
            {
                if (OrganizationFormMode == FormMode.Add)
                    await HandleAddSubmit();

                if (OrganizationFormMode == FormMode.Edit)
                    await HandleEditSubmit();
            }
        }

        protected async Task EndAddEditAsync()
        {
            OrganizationFormMode = FormMode.Unknown;
            AddressFormMode = FormMode.Unknown;
            Organizations = (await OrganizationDataService.GetAllOrganizations()).ToList();
        }
    }
}
