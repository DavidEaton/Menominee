using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.OrganizationDataContracts.Components
{
    public partial class ContactableTemplate<TItem>
        where TItem : class
    {
        [Parameter]
        public RenderFragment TableHeader { get; set; }

        [Parameter]
        public RenderFragment<TItem> RowTemplate { get; set; }

        [Parameter]
        public IEnumerable<TItem> Items { get; set; }

        [Parameter]
        public EventCallback<TItem> OnRowClick{ get; set; }

        protected async Task EditAsync(TItem item)
        {
            await OnRowClick.InvokeAsync(item);
        }
    }
}
