using Menominee.Client.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Navigations;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.Shared;

public partial class ModuleMenu
{
    [Inject]
    public IJSRuntime? Js { get; set; }

    [Parameter]
    public string? ModuleName { get; set; }

    [Parameter]
    public List<MenuItem>? MenuItems { get; set; }

    [Parameter]
    public int MenuWidth { get; set; }

    [Parameter]
    public EventCallback<MenuItem> OnItemSelected { get; set; }

    [Parameter]
    public string IconName { get; set; } = "m-empty-icon";

    [CascadingParameter(Name = "MainLayout")]
    private MainLayout? MainLayout { get; set; }

    public WindowSize CurrentWindowSize { get; set; }
    //private List<MenuItem> hamburgerMenuItems = new();
    private SfMenu<MenuItem>? MenuObj;
    private TelerikMenu<MenuItem>? HorizontalMenuObj;
    private bool parametersSet = false;
    private IJSObjectReference? JsModule;
    private double ModuleTitleWidth { get; set; } = 0;

    protected override async Task OnInitializedAsync()
    {
        JsModule = Js is not null 
            ? await Js.InvokeAsync<IJSObjectReference>("import", "./js/elementWidth.js") 
            : null;
    }

    protected override async Task OnParametersSetAsync()
    {
        if (!parametersSet && JsModule != null)
        {
            parametersSet = true;
            ModuleTitleWidth = await JsModule.InvokeAsync<double>("elementWidthById", "moduleName");
        }
    }

    private string GetMenuBreakpoint()
    {
        double expandedMin = 190 + ModuleTitleWidth + MenuWidth;
        double shrunkenMin = 80 + ModuleTitleWidth + MenuWidth;
        return MainLayout is not null
            ? (MainLayout.DrawerExpanded ? $"(min-width: {expandedMin}px)" : $"(min-width: {shrunkenMin}px)")
            : string.Empty;
    }

    protected async Task OnClickHandler(MenuItem item)
    {
        await OnItemSelected.InvokeAsync(item);
    }

    private async Task OnOtherMenuItemClicked(MenuEventArgs<MenuItem> args)
    {
        if (int.Parse(args.Item.Id) >= 0 && MenuObj is not null)
        {
            await MenuObj.CloseAsync();
            var menuItem = FindMenuItem(args.Item.Id);
            if (menuItem != null)
                await OnItemSelected.InvokeAsync(menuItem);
        }
    }

    private MenuItem? FindMenuItem(string itemId)
    {
        return MenuItems is not null
            ? MenuItems.Where(x => x.Id == itemId).FirstOrDefault()
            : null;
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender && Js is not null)
        {
            var lDotNetReference = DotNetObjectReference.Create(this);
            Js.InvokeVoidAsync("GLOBAL.SetDotnetReference", lDotNetReference);
        }
    }

    [JSInvokable("HamburgerMenu")]
    public void DocumentClick()
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
