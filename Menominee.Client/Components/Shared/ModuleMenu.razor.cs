using Menominee.Client.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Navigations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.Shared
{
    public partial class ModuleMenu
    {
        [Inject]
        public IJSRuntime _js { get; set; }

        [Parameter]
        public string ModuleName { get; set; }

        [Parameter]
        public List<MenuItem> MenuItems { get; set; }

        [Parameter]
        public int MenuWidth { get; set; }

        [Parameter]
        public EventCallback<MenuItem> OnItemSelected { get; set; }

        [Parameter]
        public string IconName { get; set; } = "m-empty-icon";

        [CascadingParameter(Name = "MainLayout")]
        MainLayout MainLayout { get; set; }

        public WindowSize CurrentWindowSize { get; set; }
        private List<MenuItem> hamburgerMenuItems = new List<MenuItem>();
        private SfMenu<MenuItem> MenuObj;
        private TelerikMenu<MenuItem> HorizontalMenuObj;
        private bool parametersSet = false;
        private IJSObjectReference _jsModule;
        private double ModuleTitleWidth { get; set; } = 0;

        protected override async Task OnInitializedAsync()
        {
            _jsModule = await _js.InvokeAsync<IJSObjectReference>("import", "./js/elementWidth.js");
        }

        protected override async Task OnParametersSetAsync()
        {
            if (!parametersSet && _jsModule != null)
            {
                parametersSet = true;
                ModuleTitleWidth = await _jsModule.InvokeAsync<double>("elementWidthById", "moduleName");
                Console.WriteLine($"{ModuleTitleWidth}");
            }
        }

        private string GetMenuBreakpoint()
        {
            //int expandedMin = 340 + MenuWidth;
            //int shrunkenMin = 230 + MenuWidth;
            double expandedMin = 190 + ModuleTitleWidth + MenuWidth;
            double shrunkenMin = 80 + ModuleTitleWidth + MenuWidth;
            Console.WriteLine(MainLayout.DrawerExpanded ? $"(min-width: {expandedMin}px)" : $"(min-width: {shrunkenMin}px)");
            return MainLayout.DrawerExpanded ? $"(min-width: {expandedMin}px)" : $"(min-width: {shrunkenMin}px)";
        }

        protected async Task OnClickHandler(MenuItem item)
        {
            await OnItemSelected.InvokeAsync(item);
        }

        private async Task OnOtherMenuItemClicked(MenuEventArgs<MenuItem> args)
        {
            if (Int32.Parse(args.Item.Id) >= 0)
            {
                await MenuObj.CloseAsync();
                var menuItem = FindMenuItem(args.Item.Id);
                if (menuItem != null)
                    await OnItemSelected.InvokeAsync(menuItem);
            }
        }

        private MenuItem FindMenuItem(string itemId)
        {
            return MenuItems.Where(x => x.Id == itemId).FirstOrDefault();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                var lDotNetReference = DotNetObjectReference.Create(this);
                _js.InvokeVoidAsync("GLOBAL.SetDotnetReference", lDotNetReference);
            }
        }

        [JSInvokable("HamburgerMenu")]
        public void documentClick()
        {
            if (MenuObj != null)
            {
                MenuObj.CloseAsync();
                StateHasChanged();
            }
        }

        //public void SmallMediaQueryChange(bool matchesMediaQuery)
        //{
        //    if (matchesMediaQuery)
        //    {
        //        CurrentWindowSize = WindowSize.Small;
        //    }
        //}

        //public void MediumMediaQueryChange(bool matchesMediaQuery)
        //{
        //    if (matchesMediaQuery)
        //    {
        //        CurrentWindowSize = WindowSize.Medium;
        //    }
        //}

        //public void LargeMediaQueryChange(bool matchesMediaQuery)
        //{
        //    if (matchesMediaQuery)
        //    {
        //        CurrentWindowSize = WindowSize.Large;
        //    }
        //}

        public void MediaQueryChange(bool matchesMediaQuery)
        {
            CurrentWindowSize = matchesMediaQuery ? WindowSize.Large : WindowSize.Small;
        }

        //private string SmallBreakpoint { get; set; } = "(max-width: 559.98px)";
        //private string MediumBreakpoint { get; set; } = "(min-width: 560px) and (max-width: 767.98px)";
        //private string LargeBreakpoint { get; set; } = "(min-width: 768px)";

        public enum WindowSize
        {
            Small,
            Medium,
            Large
        }
    }
}
