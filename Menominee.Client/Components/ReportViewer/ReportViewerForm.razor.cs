using Microsoft.AspNetCore.Components;
using Telerik.ReportViewer.BlazorNative.Tools;
using Telerik.ReportViewer.BlazorNative;
using Telerik.DataSource.Extensions;

namespace Menominee.Client.Components.ReportViewer
{
    // TODO: MEN-522 Decide what, if any, commented code is useful after Telerik completes changes
    public partial class ReportViewerForm
    {
        [Parameter]
        public string ReportName { get; set; } = "Report Catalog.trdp";

        //[Parameter]
        //public bool PreviewMode { get; set; } = true;

        //[Parameter]
        //public bool DialogVisible { get; set; } = false;

        [Parameter]
        public EventCallback OnDone { get; set; }

        [Parameter]
        public Dictionary<string, object>? ReportParameters { get; set; }

        public Telerik.ReportViewer.BlazorNative.ReportViewer? Report { get; set; }

        public ScaleMode ScaleMode { get; set; } = ScaleMode.Specific;
        public ViewMode ViewMode { get; set; } = ViewMode.Interactive;
        public PrintMode PrintMode { get; set; } = PrintMode.ForcePDFPlugin;    //PrintMode.AutoSelect
        public bool ParametersAreaVisible { get; set; }
        public bool DocumentMapVisible { get; set; }
        public double Scale { get; set; } = 1.0;

        //public ReportSourceOptions ReportSource { get; set; } = new ReportSourceOptions(ReportName, new Dictionary<string, object>
        //{
        //    // Add parameters if applicable
        //});

        public List<IReportViewerTool> Tools = new List<IReportViewerTool>();

        public ReportSourceOptions? ReportSource { get; set; } = null;

        protected override void OnParametersSet()
        {
            //if (!PreviewMode)
            //{
            //    Tools.Add(new Refresh());
            //    Tools.Add(new NavigateBackward());
            //    Tools.Add(new NavigateForward());
            //    Tools.Add(new StopRendering());
            //}
            Tools.Add(new FirstPage());
            Tools.Add(new PreviousPage());
            Tools.Add(new PageNumber());
            Tools.Add(new NextPage());
            Tools.Add(new LastPage());
            Tools.Add(new ToggleViewMode());
            Tools.Add(new ToggleScaleMode());
            Tools.Add(new Export());
            Tools.Add(new SendEmail());
            Tools.Add(new Print());
            //if (!PreviewMode)
            //{
            //    Tools.Add(new ToggleDocumentMap());
            //    Tools.Add(new ToggleParametersArea());
            //    Tools.Add(new ZoomIn());
            //    Tools.Add(new ZoomOut());
            //    Tools.Add(new Search());
            //}

            //if (PreviewMode)
            {
                ScaleMode = ScaleMode.FitPageWidth;
                ViewMode = ViewMode.PrintPreview;
            }

            if (ReportName is null)
                ReportName = "Report Catalog.trdp";
            ReportSource = new ReportSourceOptions(ReportName, new Dictionary<string, object>
            {
                // Add parameters if applicable
            });

            if (ReportParameters?.Count > 0)
                ReportSource.Parameters.AddRange(ReportParameters);
        }
    }
}