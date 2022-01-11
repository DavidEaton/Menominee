using Syncfusion.Blazor.Schedule;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Client.Pages.Calendar
{
    public partial class Calendar
    {
        private View CurrentView = View.TimelineDay;

        public DateTime CurrentDate = DateTime.Now;
        static int[] WorkingDays = new int[] { 1, 2, 3, 4, 5, 6 };
        private DateTime? startTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 7, 0, 0);
        private DateTime? endTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 18, 0, 0);
        public string StartWorkingHours = "07:00";
        public string EndWorkingHours = "18:00";
        //public void StartWorkHours(Syncfusion.Blazor.Calendars.ChangeEventArgs<DateTime?> args)
        //{
        //    this.StartWorkingHours = args.Text;
        //}
        //public void EndWorkHours(Syncfusion.Blazor.Calendars.ChangeEventArgs<DateTime?> args)
        //{
        //    this.EndWorkingHours = args.Text;
        //}

        ValidationRules RequiredRule = new() { /*Required = true*/ };

        public void OnEventRendered(EventRenderedArgs<AppointmentData> args)
        {
            var fgColor = "black";
            var bgColor = "gray";
            var service = Services.FirstOrDefault(x => x.Id == args.Data.ServiceId);
            if (service != null)
            {
                fgColor = service.TextColor;
                bgColor = service.BackgroundColor;
                args.Data.Service = service.Text;
            }
            else
            {
                args.Data.Service = "Unknown";
            }

            Dictionary<string, object> attributes = new Dictionary<string, object>();
            attributes.Add("style", $"background: {bgColor}; color: {fgColor};");
            args.Attributes = attributes;
        }

        public void OnPopupOpen(PopupOpenEventArgs<AppointmentData> args)
        {
            if (args.Type == PopupType.QuickInfo)
            {
                args.Cancel = true;
            }
        }

        List<AppointmentData> DataSource = new List<AppointmentData>()
        {
            new AppointmentData { Id=1, TeamId=1, StartTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 10,0,0), EndTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 11,30,0), /*Service="Oil Change",*/ TechId=2, Vehicle="2019 Ford Focus", ServiceId=1},
            new AppointmentData { Id=2, TeamId=3, StartTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 13,30,0), EndTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 16,30,0), /*Service="Alignment",*/ TechId=5, Vehicle="2018 Honda Civic", ServiceId=2},
            new AppointmentData { Id=3, TeamId=2, StartTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 15,30,0), EndTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 16,0,0), /*Service="Radiator Flush",*/ TechId=4, Vehicle="2015 Toyota Prius", ServiceId=3}
        };

        private string[] GroupData = new string[] { "Teams", "Techs" };

        private List<ResourceData> TeamData { get; set; } = new List<ResourceData>
        {
            new ResourceData {Text = "Team Ben", Id= 1, Color= "#cb6bb2"},
            new ResourceData {Text = "Team Josh", Id= 2, Color= "#56ca85"},
            new ResourceData {Text = "Team Rob", Id= 3, Color= "#df5286"}
        };

        private List<ResourceData> TechData { get; set; } = new List<ResourceData>
        {
            new ResourceData { Text = "Phil", Id = 1, TeamId = 1, Color = "#df5286" },
            new ResourceData { Text = "Steven", Id = 2, TeamId = 1, Color = "#7fa900" },
            new ResourceData { Text = "Robert", Id = 3, TeamId = 2, Color = "#ea7a57" },
            new ResourceData { Text = "Samuel", Id = 4, TeamId = 2, Color = "#5978ee" },
            new ResourceData { Text = "Michael", Id = 5, TeamId = 3, Color = "#df5286" },
            new ResourceData { Text = "Larry", Id = 6, TeamId = 3, Color = "#00bdae" }
        };

        private List<Service> Services { get; set; } = new List<Service>
        {
            new Service { Id = 1, Text = "Air Conditioning", BackgroundColor = "blue", TextColor = "white" },
            new Service { Id = 2, Text = "Alignment", BackgroundColor = "yellow" },
            new Service { Id = 3, Text = "Battery", BackgroundColor = "orange" },
            new Service { Id = 4, Text = "Brakes", BackgroundColor = "lightblue" },
            new Service { Id = 5, Text = "Exhaust", BackgroundColor = "burlywood" },
            new Service { Id = 6, Text = "Lube Oil & Filter", BackgroundColor = "cadetblue", TextColor = "white" },
            new Service { Id = 7, Text = "Shocks/Struts", BackgroundColor = "cornflowerblue", TextColor = "white" },
            new Service { Id = 8, Text = "Starting/Charging", BackgroundColor = "darkseagreen" },
            new Service { Id = 9, Text = "Tires", BackgroundColor = "plum" },
            new Service { Id = 10, Text = "Transmission", BackgroundColor = "green", TextColor = "white" },
            new Service { Id = 11, Text = "Lunch", BackgroundColor = "darkblue", TextColor = "white" },
            new Service { Id = 12, Text = "Off", BackgroundColor = "black", TextColor = "white" }
        };

        public class Service
        {
            public long Id { get; set; }
            public string Text { get; set; }
            public string TextColor { get; set; } = "black";
            public string BackgroundColor { get; set; }
        }

        public class ResourceData
        {
            public string Text { get; set; }
            public long Id { get; set; }
            public int TeamId { get; set; }
            public string Color { get; set; }
        }

        public class AppointmentData
        {
            public long Id { get; set; }
            public int TeamId { get; set; }
            public int TechId { get; set; }
            public string Service { get; set; }
            public int ServiceId { get; set; }
            public string Notes { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public string Vehicle { get; set; }
        }
    }
}
