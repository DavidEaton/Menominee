using Microsoft.AspNetCore.Components;

namespace Menominee.OrganizationDtos.Components
{
    public partial class OrganizationTemplate<TItem> : ComponentBase
        where TItem : class
    {
        [Parameter]
        public RenderFragment<TItem> Organization { get; set; }
    }
}
