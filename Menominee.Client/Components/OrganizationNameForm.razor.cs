using Microsoft.AspNetCore.Components;
using SharedKernel.ValueObjects;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Menominee.Client.Components
{
    public partial class OrganizationNameForm
    {
        OrganizationNameProperties Name { get; set; } = new OrganizationNameProperties();
        public OrganizationName OrganizationName { get; set; }

        [Parameter]
        public EventCallback<bool> CloseEventCallback { get; set; }

        protected async Task Validate()
        {
            if (Name.IsValid)
            {
                var organizationNameOrError = OrganizationName.Create(Name.Name);

                if (organizationNameOrError.IsSuccess)
                {
                    OrganizationName = organizationNameOrError.Value;
                    await CloseEventCallback.InvokeAsync(true);
                    StateHasChanged();
                }

                await CloseEventCallback.InvokeAsync(false);
            }
            else
            {
                // TODO: display invalid message(s)

            }
        }

        private class OrganizationNameProperties
        {
            [Required(ErrorMessage = "Organization Name is required")]
            [StringLength(255, ErrorMessage = "Please limit Name to 255 characters")]
            public string Name { get; set; }

            public bool IsValid { get => !string.IsNullOrWhiteSpace(Name); }
        }
    }
}
