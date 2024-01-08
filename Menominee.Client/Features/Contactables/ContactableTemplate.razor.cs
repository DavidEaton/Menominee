using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Features.Contactables
{
    public partial class ContactableTemplate<TItem>
        where TItem : class
    {
        [Parameter]
        public RenderFragment TableHeader { get; set; }

        [Parameter]
        public RenderFragment<TItem> RowTemplate { get; set; }

        [Parameter]
        public IEnumerable<TItem> Items { get; set; } = Enumerable.Empty<TItem>();

        [Parameter]
        public EventCallback<TItem> OnRowClick { get; set; }

        protected async Task EditAsync(TItem item)
        {
            await OnRowClick.InvokeAsync(item);
        }
    }
}